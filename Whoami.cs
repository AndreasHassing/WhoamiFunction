
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WhoamiFunction
{
    public static class Whoami
    {
        [FunctionName("Whoami")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string keyClaimValue = req.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("keyid", StringComparison.OrdinalIgnoreCase))?.Value;
            string codeQueryParameter = req.Query["code"];
            string functionsKeyHeader = req.Headers["x-functions-key"];

            var whoami = new
            {
                keyClaimValue,
                codeQueryParameter,
                functionsKeyHeader,
            };

            log.LogInformation(JsonConvert.SerializeObject(whoami));

            return new OkObjectResult(whoami);
        }
    }
}
