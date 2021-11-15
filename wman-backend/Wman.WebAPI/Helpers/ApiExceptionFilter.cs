using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wman.Logic.Helpers;

namespace Wman.WebAPI.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            ;
            switch (context.Exception)
            {
                case NotFoundException:
                     this.setContext(HttpStatusCode.NotFound, context).Wait();
                    break;
                case IncorrectPasswordException:
                     this.setContext(HttpStatusCode.Forbidden, context).Wait();
                    break;
                case NotMemberOfRoleException:
                     this.setContext(HttpStatusCode.BadRequest, context).Wait();
                    break;
                case ArgumentException:
                     this.setContext(HttpStatusCode.UnprocessableEntity, context).Wait();
                    break;
                case InvalidOperationException:
                     this.setContext(HttpStatusCode.Conflict, context).Wait();
                    break;
                default:
#if DEBUG
                    Debug.Write(context.Exception);
#endif
                    this.setContext(HttpStatusCode.InternalServerError, context).Wait();
                    break;
            }
             base.OnException(context);
        }
        private async Task setContext(HttpStatusCode statusCode, ExceptionContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";

            context.HttpContext.Response.StatusCode = (int)statusCode;

            //Json
            //context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            //var json = JsonSerializer.Serialize<string>(input);
            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());

            //Plain text
            context.HttpContext.Response.ContentType = "text/plain; charset=utf-8";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(context.Exception.Message));
            ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());

            await context.HttpContext.Response.Body.WriteAsync(readOnlyMemory); //Has to be async

            context.ExceptionHandled = true;
        }
    }
}
