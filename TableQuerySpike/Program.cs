using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace TableQuerySpike
{
    public class OrchestrationInstanceStatus : TableEntity
    {
        public string ExecutionId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Input { get; set; }
        public string InputBlobName { get; set; }
        public string Output { get; set; }
        public string OutputBlobName { get; set; }
        public string CustomStatus { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string RuntimeStatus { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            ExecuteAsync().GetAwaiter().GetResult();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static IConfigurationRoot Configuration { get; set; }

        public static async Task ExecuteAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(Configuration["ConnectionString"]);
            var client = storageAccount.CreateCloudTableClient();
            var instanceTable = client.GetTableReference("DurableFunctionsHubInstances");
            var query = new TableQuery<OrchestrationInstanceStatus>().Where(
                TableQuery.GenerateFilterCondition("RuntimeStatus", QueryComparisons.Equal, "Running")
            );
            
            TableContinuationToken continuationToken = null;
            do
            {
                var request = await instanceTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                var instances = request.ToList();
                Console.WriteLine(JsonConvert.SerializeObject(instances));

                continuationToken = request.ContinuationToken;

            } while (continuationToken != null);



        }
    }
}
