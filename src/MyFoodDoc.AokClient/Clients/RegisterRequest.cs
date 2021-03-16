using System;

namespace MyFoodDoc.AokClient.Clients
{
    public class RegisterRequest
    {
        public string InsuranceNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Source { get; set; } = "myfooddock";
    }
}
