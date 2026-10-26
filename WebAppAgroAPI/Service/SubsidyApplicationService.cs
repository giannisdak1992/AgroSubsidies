using Serilog;
using System.Xml;
using WebAppAgroAPI.Core;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Exceptions;
using WebAppAgroAPI.Repositories;

namespace WebAppAgroAPI.Service
{
    public class SubsidyApplicationService : ISubsidyApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubsidyApplicationService> _logger = new LoggerFactory().AddSerilog().CreateLogger<SubsidyApplicationService>();
        public SubsidyApplicationService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public async Task<SubsidyApplicationResponseDTO> SubmitApplicationAsync(int userId, SubsidyApplicationInsertDTO dto)
        {
            try
            {
                //who farmer submits application. find the associated with the authenticated User
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer associated with userId {userId} was not found.");

                }

                // if farmer does not select parcel to get subsidy
                if (dto.ParcelIds == null || dto.ParcelIds.Count == 0)
                {
                    throw new InvalidArgumentException("SubsidyApplication", "At least one parcel must be selected.");

                }

                if (dto.ParcelIds.Count != dto.ParcelIds.Distinct().Count()) //compare total values with the distinct values. if they are not equal then we have duplicate value
                {
                    throw new InvalidArgumentException("SubsidyApplication", "Duplicate parcels are now allowed.");
                }

                // finding the assosiated parcelId from parcel repository. react sends ids

                var parcels = new List<Parcel>();

               
                foreach (int parcelId in dto.ParcelIds)
                {
                    Parcel? parcel = await _unitOfWork.ParcelRepository.GetAsync(parcelId);

                    if (parcel == null)
                    {
                        throw new EntityNotFoundException("Parcel", $"Parcel with parcelId {parcelId} does not exist.");
                    }

                    if (parcel.FarmerId != farmer.Id)
                    {
                        throw new EntityNotAuthorizedException("Farmer", $"Farmer with farmerId {farmer.Id} is not authorized.");
                    }

                    parcels.Add(parcel);
                }
               
                if (dto.SubsidyType == null)
                {
                    throw new InvalidArgumentException("SubsidyApplication", "SubsidyType cannot be null.");
                }
                SubsidyApplication subsidyApplication = new SubsidyApplication
                {
                    FarmerId = farmer.Id,
                    SubsidyType = dto.SubsidyType.Value,
                    ApplicationStatus = ApplicationStatus.UnderReview,
                    DecisionDate = null,
                    RejectionReason = null,
                };
                // For each selected Parcel, we create an ApplicationParcel that represents
                // the connection between this SubsidyApplication and that Parcel.
                foreach (Parcel parcel in parcels)
                {
                    var applicationParcel = new ApplicationParcel
                    {
                        ParcelId = parcel.Id
                    };
                    subsidyApplication.ApplicationParcels.Add(applicationParcel);
                }
                await _unitOfWork.SubsidyApplicationRepository.AddAsync(subsidyApplication);
                bool saved = await _unitOfWork.SaveAsync();

                if (!saved)
                {
                    throw new ServerException("SubsidyApplication", "Subsidy Application could not be saved.");
                }

                _logger.LogInformation("Subsidy Application {ApplicationId} is successfully submitted for Farmer {FarmerId}.", subsidyApplication.Id, farmer.Id);

                var subsidyApplicationResponseDTO = new SubsidyApplicationResponseDTO
                {
                    Id = subsidyApplication.Id,
                    FarmerId = subsidyApplication.FarmerId,
                    SubsidyType = subsidyApplication.SubsidyType,
                    ApplicationStatus = subsidyApplication.ApplicationStatus,
                    DecisionDate = subsidyApplication.DecisionDate,
                    RejectionReason = subsidyApplication.RejectionReason
                };

                return subsidyApplicationResponseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding subsidyApplication for {UserId}",userId);
                throw;
            }
        }

        public async Task<IEnumerable<SubsidyApplicationResponseDTO>> GetApplicationsByFarmerIdAsync(int farmerId)
        {
            try 
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetAsync(farmerId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with id {farmerId} was not found.");
                }
                IEnumerable<SubsidyApplication> subsidyApplications = await _unitOfWork.SubsidyApplicationRepository.GetApplicationsByFarmerIdAsync(farmerId);

                List<SubsidyApplicationResponseDTO> applicationResponseDTOs = subsidyApplications.Select(a => new SubsidyApplicationResponseDTO
                {
                    Id = a.Id,
                    FarmerId = a.FarmerId,
                    SubsidyType = a.SubsidyType,
                    ApplicationStatus = a.ApplicationStatus,
                    DecisionDate = a.DecisionDate,
                    RejectionReason = a.RejectionReason
                }).ToList();

                return applicationResponseDTOs;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving applications for FarmerId {FarmerId}", farmerId);
                throw;
            }
        }

        public async Task<IEnumerable<SubsidyApplicationResponseDTO>> GetMyApplicationsAsync(int userId)
        {
            try 
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with userId {userId} was not found.");
                }

                IEnumerable<SubsidyApplication> subsidyApplications = await _unitOfWork.SubsidyApplicationRepository.GetApplicationsByFarmerIdAsync(farmer.Id);
                
                List<SubsidyApplicationResponseDTO> applicationResponseDTOs = subsidyApplications.Select(a => new SubsidyApplicationResponseDTO
                {
                    Id = a.Id,
                    FarmerId = a.FarmerId,
                    SubsidyType = a.SubsidyType,
                    ApplicationStatus = a.ApplicationStatus,
                    DecisionDate = a.DecisionDate,
                    RejectionReason = a.RejectionReason
                }).ToList();

                return applicationResponseDTOs;
                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving applications for userId {userId}", userId);
                throw;
            }
        }

        public async Task<SubsidyApplicationResponseDTO> EvaluateApplicationAsync(int applicationId)
        {
            SubsidyApplication? subsidyApplication = await _unitOfWork.SubsidyApplicationRepository.GetApplicationWithDetailsAsync(applicationId);

            if (subsidyApplication == null)
            {
                throw new EntityNotFoundException("Application", $"Application with id {applicationId} was not found.");

            }

            Farmer? farmer = subsidyApplication.Farmer;

            if (subsidyApplication.ApplicationStatus != ApplicationStatus.UnderReview)
            {
                throw new InvalidArgumentException("Status", "Status must be UnderReview.");
            }

            IEnumerable<Parcel> parcels = subsidyApplication.ApplicationParcels.Select(ap => ap.Parcel);

            if (!parcels.Any())
            {
                throw new InvalidArgumentException(
                    "Parcels",
                    "Application must include at least one parcel.");
            }

            decimal totalArea = parcels.Sum(p => p.Area);

            // Apply eligibility rules depending on SubsidyType
            switch (subsidyApplication.SubsidyType)
            {
                case SubsidyType.BasicSupport:

                    if (totalArea >= 4)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Approved;
                    }
                    else
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = $"Minimum required area is 4. Selected total area is {totalArea}.";
                    }

                    break;
                case SubsidyType.EcoScheme:
                    bool hasTobacco = parcels.Any(p => p.CropType == CropType.Tobacco);

                    if (totalArea < 5)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = $"Minimum required area is 5. Selected total area is {totalArea}.";
                    }
                    else if (hasTobacco)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = "Tobacco crops are not eligible gor EcoScheme.";
                    }
                    else
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Approved;
                    }
                    break;
                case SubsidyType.OrganicFarming:
                    bool allCropsAreaEligible = parcels.All(p => //all except tobacco
                        p.CropType == CropType.OliveTrees ||
                        p.CropType == CropType.Vineyard ||
                        p.CropType == CropType.Citrus ||
                        p.CropType == CropType.Vegetables);
                    if (totalArea < 3)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = $"Minimum required area is 3. Selected total area is {totalArea}.";
                    }

                    else if (!allCropsAreaEligible)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = "One or more selected crops are not eligible for Organic Farming";
                    }

                    else
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Approved;
                    }

                    break;
                case SubsidyType.YoungFarmers:

                    DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow); //Datetime.Today 15/09/2026.
                    DateOnly dateOfBirth = farmer.DateOfBirth;

                    int age = today.Year - dateOfBirth.Year;

                    DateOnly birthdayThisYear = dateOfBirth.AddYears(age);
                    if (today < birthdayThisYear)
                    {
                        age--;
                    }

                    if (age > 40)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = $"Maximum eligible age is 40. Farmer's age is {age}.";
                    }

                    else if (totalArea < 4)
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Rejected;
                        subsidyApplication.RejectionReason = $"Minimum required area is 4. Selected area is {totalArea}.";
                    }

                    else
                    {
                        subsidyApplication.ApplicationStatus = ApplicationStatus.Approved;
                    }

                    break;
                case SubsidyType.Livestock:
                    throw new InvalidArgumentException(
                        "SubsidyApplication",
                        "LiveStock subsidy is not currently supported.");
                default:
                    throw new InvalidArgumentException(
                        "SubsidyApplication",
                        "Invalid subsidy type.");
            }
            subsidyApplication.DecisionDate = DateTime.UtcNow;
            bool saved = await _unitOfWork.SaveAsync();
            if (!saved)
            {
                throw new ServerException("SubsidyApplication", "Subsidy Application could not be saved.");
            }
            _logger.LogInformation(
                "Subsidy Application {ApplicationId} evaluated with status {Status} for Farmer {FarmerId}.",
                subsidyApplication.Id,
                subsidyApplication.ApplicationStatus,
                farmer.Id); _logger.LogInformation("Subsidy Application {ApplicationId} is successfully submitted for Farmer {FarmerId}.", subsidyApplication.Id, farmer.Id);

            SubsidyApplicationResponseDTO responseDTO = new SubsidyApplicationResponseDTO
            {
                Id = subsidyApplication.Id,
                FarmerId = subsidyApplication.FarmerId,
                SubsidyType = subsidyApplication.SubsidyType,
                ApplicationStatus = subsidyApplication.ApplicationStatus,
                DecisionDate = subsidyApplication.DecisionDate,
                RejectionReason = subsidyApplication.RejectionReason
            };
            return responseDTO;
        }      
    }
}
