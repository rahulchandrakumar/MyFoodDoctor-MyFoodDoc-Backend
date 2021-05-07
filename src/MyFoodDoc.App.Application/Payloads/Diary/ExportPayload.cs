using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class ExportPayload
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
