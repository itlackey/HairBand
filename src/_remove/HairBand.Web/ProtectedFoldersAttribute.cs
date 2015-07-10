using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Internal;

namespace HairBand.Controllers
{
    public class ProtectedFoldersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionArguments.ContainsKey("page"))
            {
                var page = context.ActionArguments["page"].ToString();

                if (page.StartsWith("_") || page.StartsWith("app_data"))
                    context.Result = new HttpNotFoundResult();

            }

        }
    }
}
