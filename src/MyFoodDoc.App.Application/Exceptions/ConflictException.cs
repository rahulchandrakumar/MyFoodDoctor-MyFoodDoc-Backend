﻿using System;

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
