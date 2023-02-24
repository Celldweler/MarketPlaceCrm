using System;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected static bool CheckForNull(object o)
        {
            if (o == null)
                return true;

            return false;
        }
    }
}