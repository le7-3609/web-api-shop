using DTO;
using Entities;

namespace Services
{
    public interface IPasswordValidityService
    {
        PasswordStrengthDTO? PasswordStrength(string password);
    }
}