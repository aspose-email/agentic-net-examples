using System;
using Aspose.Email;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder token provider – replace with a valid implementation.
            Aspose.Email.Clients.ITokenProvider tokenProvider = null; // e.g., TokenProvider.GetInstance(...)

            // Tenant identifier – replace with your actual tenant ID.
            string tenantId = "your-tenant-id";

            // Initialize the Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Identifier of the task to be removed.
                string taskId = "task-id-to-delete";

                // Delete the task using the generic Delete method.
                client.Delete(taskId);

                Console.WriteLine($"Task with ID '{taskId}' has been deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
