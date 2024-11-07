using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#nullable disable
namespace Business
{
    public class UserEncryptionKey
    {
        private readonly Regex whiteSpaceRegex = new Regex(@"\s+");
        private const int keyLength = 16;


        public string ValidationError { get; set; }
        public bool IsValid { get; set; } = true;

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = PasswordValidation(value);
            }
        }

        private string PasswordValidation(string input)
        {
            ValidationError = "";
            IsValid = true;
            input = whiteSpaceRegex.Replace(input, string.Empty);
            if (string.IsNullOrEmpty(input) || input.Length != keyLength)
            {
                ValidationError = $"Key length : {keyLength} characters";
                IsValid = false;
            }

            return input;

        }

    }
}
