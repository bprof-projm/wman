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
                   
                    
                    this.setContext(HttpStatusCode.Gone, "It's GONE", context);
                    
                    
                    break;
                default:

                    break;
            }
        }
        private void setContext(HttpStatusCode statusCode, string input, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            var json = JsonSerializer.Serialize<string>(input);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());
            context.HttpContext.Response.Body.WriteAsync(readOnlyMemory);
            context.ExceptionHandled = true;
            

        }
    }
}
