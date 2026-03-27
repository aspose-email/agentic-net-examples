using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths and credentials (replace with real values)
                string msgPath = "sample.msg";
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";
                string folderName = "AdminCenterProject";

                // Verify the MSG file exists before loading
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the MSG file into a MailMessage object
                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    // Initialize the Outlook token provider (no using directive for the type)
                    Aspose.Email.Clients.TokenProvider outlookProvider =
                        Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                    // Create the Microsoft Graph client
                    using (IGraphClient client = GraphClient.GetClient(outlookProvider, tenantId))
                    {
                        // Create a new folder in the mailbox
                        client.CreateFolder(folderName);

                        // Retrieve the folder information to obtain its ItemId (not Id)
                        FolderInfo folderInfo = client.GetFolder(folderName);
                        string folderId = folderInfo.ItemId;

                        // Upload the message into the newly created folder
                        client.CreateMessage(folderId, mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception handling
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
