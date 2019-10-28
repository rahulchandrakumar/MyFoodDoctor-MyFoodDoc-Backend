using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.RouteConstraints
{
    public class DateRouteConstraint : IRouteConstraint
    {
        public static string Name = "Date";

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object dateValue;
            if (values.TryGetValue("date", out dateValue))
            {
                string[] formats = { "yyyy-MM-dd" };
                if (DateTime.TryParseExact(dateValue.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {

                    return true;
                }
            }
            return false;
        }
    }
}
