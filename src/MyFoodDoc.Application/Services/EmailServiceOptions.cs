using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Services
{
    public class EmailServiceOptions
    {
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string SendGridApiKey { get; set; }
    }
}
