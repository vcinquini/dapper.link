using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppTestDapper
{
    public static class Function1
    {
        static AuthorsRepository _repo = new AuthorsRepository();
        
        [FunctionName("Function1")]
        public static async Task<List<Author>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<Author>();

            outputs.Add(await context.CallActivityAsync<Author>("Function1_Single", 1));
            outputs.AddRange(await context.CallActivityAsync<List<Author>>("Function1_All", null));

            return outputs;
        }

        [FunctionName("Function1_Single")]
        public static Author GetById([ActivityTrigger] int id, ILogger log)
        {
            log.LogInformation($"Saying hello to {id}.");
            Author author = _repo.GetById(id);
            return author;
        }

        [FunctionName("Function1_All")]
        public static List<Author> GetAll([ActivityTrigger] int? id, ILogger log)
        {
            log.LogInformation($"Saying hello to all.");
            List<Author> authors = _repo.GetAll();
            return authors;
        }

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
                                                                [DurableClient] IDurableOrchestrationClient starter,
                                                                ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}