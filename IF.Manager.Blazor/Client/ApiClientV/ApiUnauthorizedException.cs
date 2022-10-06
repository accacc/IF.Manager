using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arbimed.Core.ApiClient
{
    public class ApiUnauthorizedException : Exception
    {
        public ApiUnauthorizedException()
        {
        }
        public ApiUnauthorizedException(string message) : base(message)
        {
        }
    }
}
