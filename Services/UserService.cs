using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordValidityService _passwordService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IPasswordValidityService passwordService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDTO>?> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null)
                return null;

            return _mapper.Map<IEnumerable<UserProfileDTO>>(users);
        }
        public async Task<UserProfileDTO?> RegisterAsync(RegisterAndUpdateDTO dto)
        {
            if (_passwordService.PasswordStrength(dto.Password).Strength < 2)
                return null;

            var existing = await _userRepository.GetByEmailAsync(dto.Email, -1);
            if (existing != null)
                return null;

            User user = _mapper.Map<User>(dto);
            var created = await _userRepository.RegisterAsync(user);

            return _mapper.Map<UserProfileDTO>(created);
        }

        public async Task<UserProfileDTO?> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.LoginAsync(dto.Email, dto.Password);
            //return user == null ? null : _mapper.Map<UserProfileDTO>(user);
            return _mapper.Map<UserProfileDTO>(user);

        }

        public async Task<UserProfileDTO?> UpdateAsync(int id, RegisterAndUpdateDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (_passwordService.PasswordStrength(dto.Password).Strength < 2)
                return null;

            var existing = await _userRepository.GetByEmailAsync(dto.Email, id);
            if (existing != null)
                return null;

            _mapper.Map(dto, user);
            var updated = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserProfileDTO>(updated);
        }
        public async Task<UserProfileDTO?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<IEnumerable<OrderSummaryDTO>?> GetAllOrdersAsync(int userId)
        {
            var UserOrders = await _userRepository.GetAllOrdersAsync(userId);

            if (UserOrders == null)
                return null;

            return _mapper.Map<IEnumerable<OrderSummaryDTO>>(UserOrders);
        }
    }
}
