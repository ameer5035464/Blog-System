using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.GlobalExceptions.ExceptionModels
{
    public class CustomBadRequest : Exception
    {
        public CustomBadRequest(string message):base(message)
        {
            
        }
    }
}
