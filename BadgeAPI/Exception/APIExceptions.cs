using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BadgeAPI.APIException
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string msg)
            : base(msg)
        {
        }
    }
    public class EnvironmentException : ApplicationException
    {

        public EnvironmentException(string msg)
            : base(msg)
        {
        }
    }
}