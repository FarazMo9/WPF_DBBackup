using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    
    public partial class Config
    {
        [NotMapped]
        public string FTPUrl { get; set; }
        [NotMapped]
        public string FTPUsername { get; set; }
        [NotMapped]
        public string FTPPassword { get; set; }
   

        [NotMapped]
        private long? _hostSizeInput;
        [NotMapped] 
        public long HostSizeInput
        {
            get
            {
                if (!_hostSizeInput.HasValue)
                    _hostSizeInput = HostSize / 1000000; // DB host size property is saved by bytes, so it will be converted to MB for the user input

                return _hostSizeInput.Value;

            }
            set
            {
                _hostSizeInput = value;
                HostSize = (_hostSizeInput??0) * 1000000;
            }
        }

        [NotMapped]
        private int? _intervalInput;
        [NotMapped]
        public int IntervalInput
        {
            get
            {
                if (!_intervalInput.HasValue)
                    _intervalInput = Interval / 60000; // DB interval property is saved by miliseconds, so it will be converted to minutes for the user input 

                return _intervalInput.Value;

            }
            set
            {
                _intervalInput=value;
                Interval = (_intervalInput??0) * 60000;
            }

        }


        [NotMapped]
        public bool IsFTPAvailable => !string.IsNullOrEmpty(FTPUrl) && !string.IsNullOrEmpty(FTPUsername) && !string.IsNullOrEmpty(FTPPassword);
        [NotMapped]
        public string ValidationMessage { get; set; }
        [NotMapped]
        public bool IsValid => ValidateConfig();

        private bool ValidateConfig()
        {
            var isValid = true;
            ValidationMessage = string.Empty;
            if (IsFTPAvailable && HostSizeInput < 1) // minimum host space is 1 MB
            {
                ValidationMessage = "Minimum host storage space : (1 MB)";
                isValid = false;
            }
            else if (IntervalInput < 10) // minimum interval is 10 minutes
            {
                ValidationMessage = "Minimum interval : (10 minutes)";
                isValid = false;
            }



            return isValid;
        }
    }

    public partial class DatabaseInfo
    {
        [NotMapped]
        public string DecryptedConnectionString { get; set; }
        [NotMapped]
        public string ValidationMessage { get; set; }
        [NotMapped]
        public bool IsValid => ValidateDatabaseInfo();
        private bool ValidateDatabaseInfo()
        {
            var isValid = true;
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(DecryptedConnectionString))
            {
                ValidationMessage = "Connection string has not been set.";
                isValid = false;
            }
            if (string.IsNullOrEmpty(Name))
            {
                ValidationMessage = "Database name has not been set.";
                isValid = false;
            }
            return isValid;
        }
    }
}
