using System;

namespace MarketPlaceCrm.WebApi.Services
{
    public static class ParseDateToStringExtension
    {
        public static string Parse(this DateTime date) 
            => date.ToString("MM/dd/yyyy hh:mm");
    }
}