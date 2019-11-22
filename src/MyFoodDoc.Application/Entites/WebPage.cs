using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites
{
    public class WebPage
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsDeletable { get; set; }
    }
}
