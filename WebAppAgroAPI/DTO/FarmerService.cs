using Serilog;
using System.Linq.Expressions;
using WebAppAgroAPI.Core;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Exceptions;
using WebAppAgroAPI.Models;
using WebAppAgroAPI.Repositories;
using WebAppAgroAPI.Security;
using WebAppAgroAPI.Service;

namespace WebAppAgroAPI.DTO
{
    public class FarmerService : IFarmerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FarmerService> _logger = new LoggerFactory().AddSerilog().CreateLogger<FarmerService>();

        public FarmerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<FarmerResponseDTO>> GetAllFarmersFilteredPaginatedAsync(
        int pageNumber,
        int pageSize,
        FarmerFiltersDTO farmerFiltersDTO)
        {
            if (farmerFiltersDTO == null)
            {
                farmerFiltersDTO = new FarmerFiltersDTO();
            }

            var predicates = new List<Expression<Func<Farmer, bool>>>();

            if (!string.IsNullOrWhiteSpace(farmerFiltersDTO.TaxIdentificationNumber))
            {
                predicates.Add(f =>
                    f.TaxIdentificationNumber.Contains(
                        farmerFiltersDTO.TaxIdentificationNumber));
            }

            PaginatedResult<Farmer> result =
                await _unitOfWork.FarmerRepository
                    .GetAllFarmersFilteredPaginatedAsync(
                        pageNumber, pageSize, predicates);

            List<FarmerResponseDTO> farmerDTOs = result.Data
                .Select(f => new FarmerResponseDTO
                {
                    Id = f.Id,
                    Username = f.User.Username,
                    Firstname = f.User.Firstname,
                    Lastname = f.User.Lastname,
                    Email = f.User.Email,
                    TaxIdentificationNumber = f.TaxIdentificationNumber,
                    DateOfBirth = f.DateOfBirth,
                    PhoneNumber = f.PhoneNumber
                })
                .ToList();

            return new PaginatedResult<FarmerResponseDTO>
            {
                Data = farmerDTOs,
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
        public async Task<FarmerResponseDTO> GetFarmerByTaxIdAsync(string taxId)
        {
           try
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByTaxIdAsync(taxId);
                if (farmer ==  null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with tax id {taxId} was not found.");
                }

                FarmerResponseDTO dto = new FarmerResponseDTO
                {
                    Id = farmer.Id,
                    Username = farmer.User.Username,
                    Firstname = farmer.User.Firstname,
                    Lastname = farmer.User.Lastname,
                    Email = farmer.User.Email,
                    TaxIdentificationNumber = farmer.TaxIdentificationNumber,
                    DateOfBirth = farmer.DateOfBirth,
                    PhoneNumber = farmer.PhoneNumber,
                };
                return dto;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving farmer with taxId {taxId}", taxId);
                throw;
            }
        }

        public async Task<FarmerResponseDTO> GetFarmerByUserIdAsync(int userId)
        {
            try
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerByUserIdAsync(userId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with userId {userId} was not found.");
                }

                FarmerResponseDTO dto = new FarmerResponseDTO
                {
                    Id = farmer.Id,
                    Username = farmer.User.Username,
                    Firstname = farmer.User.Firstname,
                    Lastname = farmer.User.Lastname,
                    Email = farmer.User.Email,
                    TaxIdentificationNumber = farmer.TaxIdentificationNumber,
                    DateOfBirth = farmer.DateOfBirth,
                    PhoneNumber = farmer.PhoneNumber,
                };
                return dto;
            }

            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while retrieving farmer with userId {UserId}", userId);
                throw;
            }
        }

        public async Task<FarmerResponseDTO> GetFarmerWithUserAsync(int farmerId)
        {
            try
            {
                Farmer? farmer = await _unitOfWork.FarmerRepository.GetFarmerWithUserAsync(farmerId);

                if (farmer == null)
                {
                    throw new EntityNotFoundException("Farmer", $"Farmer with farmerId {farmerId} was not found.");
                }

                FarmerResponseDTO dto = new FarmerResponseDTO
                {
                    Id = farmer.Id,
                    Username = farmer.User.Username,
                    Firstname = farmer.User.Firstname,
                    Lastname = farmer.User.Lastname,
                    Email = farmer.User.Email,
                    TaxIdentificationNumber = farmer.TaxIdentificationNumber,
                    DateOfBirth = farmer.DateOfBirth,
                    PhoneNumber = farmer.PhoneNumber,
                };
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving farmer with farmerId {FarmerId}", farmerId);
                throw;
            }
        }

        public async Task<FarmerResponseDTO> RegisterFarmerAsync(FarmerSignupDTO signupDTO)
        {
            try
            {
                User? existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(signupDTO.Username!);
                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("User", $"User with username {signupDTO.Username} already exists");
                }

                Farmer? existingFarmer = await _unitOfWork.FarmerRepository.GetFarmerByTaxIdAsync(signupDTO.TaxIdentificationNumber!);

                if (existingFarmer != null)
                {
                    throw new EntityAlreadyExistsException("Farmer", $"Farmer with taxId {signupDTO.TaxIdentificationNumber} already exists");

                }

                var newUser = new User
                {
                    Username = signupDTO.Username!,
                    Email = signupDTO.Email!,
                    Firstname = signupDTO.Firstname!,
                    Lastname = signupDTO.Lastname!,
                    Password = EncryptionUtil.Encrypt(signupDTO.Password!),
                    UserRole = UserRole.Farmer,
                    UserStatus = UserStatus.Pending
                };

               

                var newFarmer = new Farmer
                {
                    TaxIdentificationNumber = signupDTO.TaxIdentificationNumber!,
                    DateOfBirth = signupDTO.DateOfBirth!.Value,
                    PhoneNumber = signupDTO.PhoneNumber!,
                    User = newUser
                };

                await _unitOfWork.FarmerRepository.AddAsync(newFarmer);
                await _unitOfWork.SaveAsync();

                FarmerResponseDTO dto = new FarmerResponseDTO
                {
                    Id = newFarmer.Id,
                    Username = newFarmer.User.Username,
                    Firstname = newFarmer.User.Firstname,
                    Lastname = newFarmer.User.Lastname,
                    Email = newFarmer.User.Email,
                    TaxIdentificationNumber = newFarmer.TaxIdentificationNumber,
                    DateOfBirth = newFarmer.DateOfBirth,
                    PhoneNumber = newFarmer.PhoneNumber,
                };

                _logger.LogInformation("Farmer with TaxId {TaxId} registered successfully.", newFarmer.TaxIdentificationNumber);

                return dto;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering farmer with username {Username}.", signupDTO.Username);
                throw;
            }
        }
    }
}
