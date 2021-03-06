using Microsoft.AspNetCore.Http;
using System;

namespace DatingApp.Api.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse httpResponse,string message){
            httpResponse.Headers.Add("Application-Error",message);
            httpResponse.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            httpResponse.Headers.Add("Access-Control-Allow-Origin","*");
        }
        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }

    }
}