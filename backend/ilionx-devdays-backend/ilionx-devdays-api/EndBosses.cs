using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ilionx_devdays_api
{
    public static class EndBosses
    {
        [FunctionName("EndBosses")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "tododb",
                collectionName: "tasks",
                ConnectionStringSetting = "CosmosDBConnection")]
            IAsyncCollector<object> todos,
            ILogger log)
        {

            // TODO: https://github.com/markheath/funcs-todo-csharp/blob/master/AzureFunctionsTodo/CosmosDb/TodoApiCosmosDb.cs
            // AND: https://markheath.net/post/azure-functions-rest-csharp-bindings
            if (req.Method.ToLowerInvariant() == "post")
            {
                log.LogInformation("Creating a new end boss");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic input = JsonConvert.DeserializeObject(requestBody);

                if (input == null || input.id == null) {
                    new BadRequestObjectResult("Please pass a json object in the request body with at least a (lowercase) property id");
                }

                await todos.AddAsync(input);
                return new OkObjectResult(input);
            }

            log.LogInformation($"C# Queue trigger function inserted one row");

            log.LogInformation($"C# HTTP trigger function processed a request in Function. {nameof(EndBosses)}");

            return (ActionResult)new OkObjectResult(new { EndBosses = "Representing!" });

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
