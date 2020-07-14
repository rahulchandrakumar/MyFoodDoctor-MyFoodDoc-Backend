using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Exceptions
{
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message)
            : base(message)
        {
        }
    }
}
