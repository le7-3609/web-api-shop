using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repositories;
using Zxcvbn;


namespace Services
{
    public class PasswordValidityService : IPasswordValidityService
    {
        public PasswordValidity PasswordStrength(string password)
        {
            if (password != null && password != "")
            {
                var result = Zxcvbn.Core.EvaluatePassword(password);
                if (result != null)
                {
                    int score = result.Score;
                    PasswordValidity passwordValidity = new PasswordValidity();
                    passwordValidity.Password = password;
                    passwordValidity.strength = score;
                    return passwordValidity;
                }
            }
            return null;
        }
    }
}
