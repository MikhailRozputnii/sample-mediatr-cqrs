using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.Exceptions
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException(string message) : base(message)
        {
        }
    }
}
