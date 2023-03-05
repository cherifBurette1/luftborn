using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Web;

namespace luftborn.Api.Helper
{
    /// <summary>
    /// LoggerHelper
    /// </summary>
    public static class LoggerHelper
    {
        internal static void GetLocationForApiCall(HttpContext httpContext, ActionDescriptor actionDescriptor, RouteData routeData,
            Dictionary<string, object> dict, out string location)
        {
            // example route template: api/{controller}/{id}
            var routeTemplate = actionDescriptor.AttributeRouteInfo.Template;

            var method = httpContext.Request.Method;  // GET, POST, etc.



            foreach (var key in routeData.Values.Keys)
            {
                var value = routeData.Values[key].ToString();
                if (Int64.TryParse(value, out long numeric))  // C# 7 inline declaration
                    // must be numeric part of route
                    dict.Add($"Route-{key}", value.ToString());
                else
                    routeTemplate = routeTemplate.Replace("{" + key + "}", value);
            }

            location = $"{method} {routeTemplate}";

            var qs = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value);
            var i = 0;
            foreach (string key in qs.Keys)
            {
                var newKey = string.Format("q-{0}-{1}", i++, key);
                if (!dict.ContainsKey(newKey))
                    dict.Add(newKey, qs[key]);
            }

            var referrer = httpContext.Request.Headers["Referer"].ToString();
            if (referrer != null)
            {
                if (referrer.ToLower().Contains("postman"))
                    referrer = "Postman";
                if (!dict.ContainsKey("Referrer"))
                    dict.Add("Referrer", referrer);
            }
        }
    }
}
