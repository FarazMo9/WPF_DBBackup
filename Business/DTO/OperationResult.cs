using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public record OperationResult
    {
        public bool Success { get; set; }
        public string Message {  get; set; }
        public object? Result { get; set; }
        public object? ExtraOutput { get; set; }


        public static OperationResult Get(bool success=true,string message=GeneralInfo.SaveMessage, object? result=null)
        {
            return new OperationResult { Success = success, Message = message, Result = result  };
        }
       
        public static OperationResult Error(string message = GeneralInfo.ErrorMessage, object? result = null)
        {
            return new OperationResult { Success = false, Message = message, Result = result };

        }
    }
}
