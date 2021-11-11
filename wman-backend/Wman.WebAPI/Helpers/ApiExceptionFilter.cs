using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wman.WebAPI.Helpers
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ;
            switch (context.Exception)
            {
                case Exception:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Gone;
                    context.ExceptionHandled = true;
                    break;
                default:

                    break;
            }
        }
    }
}
