using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Collections.Generic;

namespace ilionx_devdays_api
{
    public static class EndBosses
    {

        // TODO: Authentication (and authorization?)

        private const string Route = "endbosses";
        private const string DatabaseName = "endbossesdb";
        private const string CollectionName = "ilionx-endbosses";

        [FunctionName("EndBosses_Create")]
        public static async Task<IActionResult> CreateEndBosses(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req, 
            [CosmosDB(
                databaseName: DatabaseName,
                collectionName: CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")]
            IAsyncCollector<dynamic> endbosses,
            ILogger log)
        {
            log.LogInformation("Trying to create a new end boss. First, who is this end boss?");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic input = JsonConvert.DeserializeObject(requestBody);

            if (input == null || input.id == null)
            {
                new BadRequestObjectResult("Please pass a json object in the request body with at least a (lowercase) property id");
            }

            log.LogInformation($"Creating a new end boss with id {input.id}");

            await endbosses.AddAsync(input);
            return new OkObjectResult(input);
        }

        [FunctionName("EndBosses_GetAll")]
        public static IActionResult GetAllEndBosses(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
    [CosmosDB(
                databaseName: DatabaseName,
                collectionName: CollectionName,
                SqlQuery = "SELECT * FROM c order by c._ts desc",
                ConnectionStringSetting = "CosmosDBConnection")]
            IEnumerable<dynamic> endbosses,
            ILogger log)
        {
            log.LogInformation("Getting all endbosses.");
            return new OkObjectResult(endbosses);
        }

        [FunctionName("EndBossById_Get")]
        public static IActionResult GetEndBossesById(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")] HttpRequest req,
    [CosmosDB(
                databaseName: DatabaseName,
                collectionName: CollectionName,
                Id = "{id}",
                ConnectionStringSetting = "CosmosDBConnection")]
            dynamic endBoss,
            ILogger log, string id)
        {
            log.LogInformation("Getting end boss by id");

            if (endBoss == null)
            {
                log.LogInformation($"End boss with {id} not found");
                return new NotFoundResult();
            }
            return new OkObjectResult(endBoss);
        }

        [FunctionName("EndBoss_Update")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")]HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
                ILogger log, 
                string id)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic input = JsonConvert.DeserializeObject(requestBody);
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
            var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                            .AsEnumerable().FirstOrDefault();
            if (document == null)
            {
                return new NotFoundResult();
            }

            try
            {
                await client.ReplaceDocumentAsync(document.SelfLink, input);

                return new OkObjectResult(input);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error on updating document with id {id}...");
                throw;
            }
        }

        [FunctionName("EndBoss_Delete")]
        public static async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")]HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log, string id)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
            var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                    .AsEnumerable().FirstOrDefault();
            if (document == null)
            {
                return new NotFoundResult();
            }

            log.LogInformation($"Deleting document with id {id}.");

            await client.DeleteDocumentAsync(document.SelfLink);
            return new OkResult();
        }
    }
}
