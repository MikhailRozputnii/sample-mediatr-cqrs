using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserManagement.Common.Exceptions;

namespace UserManagement.Filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.Exception = null;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            base.OnException(context);
        }

    }
}
