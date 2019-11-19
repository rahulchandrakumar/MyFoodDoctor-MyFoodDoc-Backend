using System;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IAuditable
    {
        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
