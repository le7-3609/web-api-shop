using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zxcvbn;


namespace Services
{
    public class PasswordValidityService : IPasswordValidityService
    {
        public PasswordStrengthDTO? PasswordStrength(string password)
        {
            if (password != null && password != "")
            {
                var result = Zxcvbn.Core.EvaluatePassword(password);
                if (result != null)
                {
                    int score = result.Score;
                    PasswordStrengthDTO dto = new PasswordStrengthDTO();
                    dto.Strength = score;
                    return dto;
                }
            }
            return null;
        }
    }
}
