using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wman.Logic.Helpers;

namespace Wman.WebAPI.Helpers
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            ;
            switch (context.Exception)
            {
                case NotFoundException:
                    await this.setContext(HttpStatusCode.NotFound, context);
                    break;
                case IncorrectPasswordException:
                    await this.setContext(HttpStatusCode.Forbidden, context);
                    break;
                case NotMemberOfRoleException:
                    await this.setContext(HttpStatusCode.BadRequest, context);
                    break;
                case ArgumentException:
                    await this.setContext(HttpStatusCode.UnprocessableEntity, context);
                    break;
                case InvalidOperationException:
                    await this.setContext(HttpStatusCode.Conflict, context);
                    break;
                default:
                    //await this.setContext(HttpStatusCode.InternalServerError, context);
                    break;
            }
            await base.OnExceptionAsync(context);
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

            await context.HttpContext.Response.Body.WriteAsync(readOnlyMemory);

            context.ExceptionHandled = true;
        }
    }
}
