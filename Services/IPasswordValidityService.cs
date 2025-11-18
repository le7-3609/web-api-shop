using Entities;

namespace Services
{
    public interface IPasswordValidityService
    {
        PasswordValidity PasswordStrength(string password);
    }
}