using Microsoft.Extensions.Logging;

namespace MarketPlaceCrm.Data.Utils
{
    public static class Util
    {
        public static readonly ILoggerFactory loggerFactory
            = LoggerFactory.Create(builder => builder.AddConsole());
    }
}