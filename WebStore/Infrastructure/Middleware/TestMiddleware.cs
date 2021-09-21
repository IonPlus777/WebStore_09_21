using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMiddleware> _Logger;
        public TestMiddleware(RequestDelegate next,ILogger<TestMiddleware> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //context.Response.WriteAsJsonAsync();

            //Predobrabotka
            var processing = _Next(context); //zapusk sleduyuchego sloya promezutochnogo PO

            //Obrabotka parallelno

            await processing; //Ozidanie zavercheniya obrabotki sleduyuchej chastiu conveiera

            // Postobrabotka dannih
        }
    }
}
