using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Exceptions
{
    public class DeleteFailureException : ApplicationException
    {
        public DeleteFailureException(string name, object key, string message) : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
