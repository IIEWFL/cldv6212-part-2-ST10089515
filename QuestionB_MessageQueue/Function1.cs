using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace QuestionB_MessageQueue
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("message-queue", Connection = "MyQueueCon")] string myQueueItem, ILogger log)
        {
            string Connstri = "Server=tcp:queuestorageserver1.database.windows.net,1433;Initial Catalog=dbQueues;Persist Security Info=False;User ID=kops;Password=Dbzgt1103;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                string[] attributes = myQueueItem.Split(':');
                string id = attributes[0];

                // PROCESS THE MESSAGE
                log.LogInformation($"Processing queue ID: {myQueueItem}");

                using (SqlConnection connection = new SqlConnection(Connstri))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        // Modify the table name to 'MessagesB' and the parameter name to '@Id'
                        command.CommandText = "INSERT INTO MessagesB (Id) VALUES (@Id)";
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }

                log.LogInformation($"Queue Message Added To the 'MessagesB' Table successfully, Id = {id}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing queue message: {ex.Message}");
            }
        }
    }
}
