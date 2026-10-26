using Serilog;
using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Exceptions;
using WebAppAgroAPI.Models;
using WebAppAgroAPI.Repositories;

namespace WebAppAgroAPI.Service
{
    public class ParcelService : IParcelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ParcelService> _logger = new LoggerFactory().AddSerilog().CreateLogger<ParcelService>();

        public ParcelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ParcelResponseDTO> AddParcelAsync(int userId, ParcelInsertDTO parcelInsertDTO)
        {
            try
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId); // find the farmer in order to use their id to create a new parcel
                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer associated with userId {userId} was not found.");
                }

                var newParcel = new Parcel
                {
                    ParcelCode = parcelInsertDTO.ParcelCode!,
                    Area = parcelInsertDTO.Area,
                    CropType = parcelInsertDTO.CropType!,
                    OwnershipType = parcelInsertDTO.OwnershipType!,
                    RegionalUnitId = parcelInsertDTO.RegionalUnitId,
                    FarmerId = farmer.Id,
                };

                await _unitOfWork.ParcelRepository.AddAsync(newParcel);
                bool saved = await _unitOfWork.SaveAsync();

                if (!saved)
                {
                    throw new ServerException("Parcel", "Parcel could not be saved.");
                }

                _logger.LogInformation("Parcel {ParcelCode} added successfully for FarmerId {FarmerId}", newParcel.ParcelCode, farmer.Id);

                Parcel? savedParcel = await _unitOfWork.ParcelRepository.GetParcelWithDetailsAsync(newParcel.Id);

                if (savedParcel == null)
                {
                    throw new EntityNotFoundException("Parcel", $"Parcel with id {newParcel.Id} was not found.");
                }
                var parcelResponseDTO = new ParcelResponseDTO
                {
                    Id = savedParcel.Id,
                    ParcelCode = savedParcel.ParcelCode,
                    Area = savedParcel.Area,
                    CropType = savedParcel.CropType,
                    OwnershipType = savedParcel.OwnershipType,
                    RegionalUnitId = savedParcel.RegionalUnitId,
                    RegionalUnitName = savedParcel.RegionalUnit.Name
                };

                return parcelResponseDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding parcel for userId {UserId}", userId);
                throw;

            }
        }

        public async Task<PaginatedResult<ParcelResponseDTO>> GetAllParcelsFilteredPaginatedAsync(int pageNumber, int pageSize, ParcelFiltersDTO parcelFiltersDTO)
        {
            if (parcelFiltersDTO == null)
            {
                parcelFiltersDTO = new ParcelFiltersDTO();
            }

            var predicates = new List<Expression<Func<Parcel, bool>>>();

            if (parcelFiltersDTO.CropType.HasValue)
            {
                predicates.Add(p => p.CropType == parcelFiltersDTO.CropType.Value);
            }

            if (parcelFiltersDTO.OwnershipType.HasValue)
            {
                predicates.Add(p => p.OwnershipType == parcelFiltersDTO.OwnershipType.Value);
            }

            if (parcelFiltersDTO.RegionalUnitId.HasValue)
            {
                predicates.Add(p => p.RegionalUnitId == parcelFiltersDTO.RegionalUnitId.Value);
            }

            PaginatedResult<Parcel> result = await _unitOfWork.ParcelRepository.GetAllParcelsFilteredPaginatedAsync(pageNumber, pageSize, predicates);

            List<ParcelResponseDTO> parcelResponseDTOs = result.Data
                .Select(p => new ParcelResponseDTO
                {
                    Id = p.Id,
                    ParcelCode = p.ParcelCode,
                    Area = p.Area,
                    CropType = p.CropType,
                    OwnershipType = p.OwnershipType,
                    RegionalUnitId = p.RegionalUnitId,
                    RegionalUnitName = p.RegionalUnit.Name
                }).ToList();

            return new PaginatedResult<ParcelResponseDTO>
            {
                Data = parcelResponseDTOs,
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
            };
               
        }

        public async Task<IEnumerable<ParcelResponseDTO>> GetParcelsByFarmerIdAsync(int farmerId)
        {
            try
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetAsync(farmerId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with id {farmerId} was not found.");
                }
                IEnumerable<Parcel> parcels = await _unitOfWork.ParcelRepository.GetParcelsByFarmerIdAsync(farmerId);
                List<ParcelResponseDTO> parcelsDTOs = parcels.Select(p => new ParcelResponseDTO
                {
                    Id = p.Id,
                    ParcelCode = p.ParcelCode,
                    Area = p.Area,
                    CropType = p.CropType,
                    OwnershipType = p.OwnershipType,
                    RegionalUnitId = p.RegionalUnitId,
                    RegionalUnitName = p.RegionalUnit.Name,
                })
                .ToList();

                return parcelsDTOs;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving parcels for FarmerId {FarmerId}", farmerId);
                throw;
            }
        }

        public async Task<IEnumerable<ParcelStatisticsDTO>> GetParcelStatisticsAsync()
        {
            try
            {
                var statistics = await _unitOfWork.ParcelRepository.GetParcelStatisticsResultsAsync();

                var statisticsDTOs = new List<ParcelStatisticsDTO>();

                foreach (ParcelStatisticsResult result in statistics)
                {
                    var dto = new ParcelStatisticsDTO
                    {
                        RegionalUnitId = result.RegionalUnitId,
                        RegionalUnitName = result.RegionalUnitName,
                        CropType = result.CropType,
                        ParcelCount = result.ParcelCount,
                        TotalArea = result.TotalArea,
                    };

                    statisticsDTOs.Add(dto);
                }


                return statisticsDTOs;
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "Error while retrieving parcel statistics");
                throw;
            }
        }

        public async Task<ParcelResponseDTO> GetParcelWithDetailsAsync(int parcelId)
        {
            try
            {
                Parcel? parcel = await _unitOfWork.ParcelRepository.GetParcelWithDetailsAsync(parcelId);

                if (parcel == null)
                {
                    throw new EntityNotFoundException("Parcel", $"Parcel with parcelId {parcelId} was not found.");
                }

                var parcelResponseDTO = new ParcelResponseDTO
                {
                    Id = parcel.Id,
                    ParcelCode = parcel.ParcelCode,
                    Area = parcel.Area,
                    CropType = parcel.CropType,
                    OwnershipType = parcel.OwnershipType,
                    RegionalUnitId = parcel.RegionalUnitId,
                    RegionalUnitName = parcel.RegionalUnit.Name,
                };
                return parcelResponseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving parcel with {parcelId}", parcelId);
                throw;
            }
        }

        public async Task<ParcelResponseDTO> GetMyParcelWithDetailsAsync(int userId, int parcelId)
        {
            try
            {
                Farmer? farmer = await  _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId);
                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with userId {userId} was not found.");
                }
                Parcel? parcel = await _unitOfWork.ParcelRepository.GetParcelWithDetailsAsync(parcelId);
                if (parcel == null)
                {
                    throw new EntityNotFoundException("Parcel", $"Parcel with parcelId {parcelId} was not found.");
                }

                if (parcel.FarmerId != farmer.Id) // ownership check. Does this parcel belong to this farmer
                {
                    throw new EntityNotAuthorizedException("Parcel", "You are not authorized to access this parcel.");
                }

                var parcelResponseDTO = new ParcelResponseDTO
                {
                    Id = parcel.Id,
                    ParcelCode = parcel.ParcelCode,
                    Area = parcel.Area,
                    CropType = parcel.CropType,
                    OwnershipType = parcel.OwnershipType,
                    RegionalUnitId = parcel.RegionalUnitId,
                    RegionalUnitName = parcel.RegionalUnit.Name
                };

                return parcelResponseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving parcel {ParcelId} for userId {userId}", parcelId, userId);
                throw;
            }
        }

        public async Task<IEnumerable<ParcelResponseDTO>> GetMyParcelsAsync(int userId)
        {
            try
            { 
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId);
                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with userId {userId} was not found.");
                }
                IEnumerable<Parcel> parcels = await _unitOfWork.ParcelRepository.GetParcelsByFarmerIdAsync(farmer.Id);
                List<ParcelResponseDTO> parcelsDTOs = parcels.Select(p => new ParcelResponseDTO
                {
                    Id = p.Id,
                    ParcelCode = p.ParcelCode,
                    Area = p.Area,
                    CropType = p.CropType,
                    OwnershipType = p.OwnershipType,
                    RegionalUnitId = p.RegionalUnitId,
                    RegionalUnitName = p.RegionalUnit.Name,
                })
                .ToList();

                return parcelsDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving parcels for userId {userId}", userId);
                throw;
            }
        }
    }
}
