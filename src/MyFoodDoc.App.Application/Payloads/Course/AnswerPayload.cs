using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Course
{
    public class AnswerPayload
    {
        public int ChapterId { get; set; }

        public bool UserAnswer { get; set; }
    }
}
