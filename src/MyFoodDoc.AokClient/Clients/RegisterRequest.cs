using System;

namespace MyFoodDoc.AokClient.Clients
{
    public class RegisterRequest
    {
        public string InsuranceNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Source { get; set; } = "myfooddock";
    }
}
