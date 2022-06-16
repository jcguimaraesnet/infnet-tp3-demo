using System;
using Amizade.Domain.Model.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class Function2
    {
        [FunctionName("Function2")]
        public void Run([QueueTrigger("update-last-view", Connection = "AzureWebJobsStorage")] Amigo amigo, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed.");

            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var textSql = $@"UPDATE [dbo].[Amigo] SET [UltimaVisualizacao] = GETDATE() WHERE [Id] = {amigo.Id};";

                using (SqlCommand cmd = new SqlCommand(textSql, conn))
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                    log.LogInformation($"rowsAffected: {rowsAffected}");
                }
            }
        }
    }
}
