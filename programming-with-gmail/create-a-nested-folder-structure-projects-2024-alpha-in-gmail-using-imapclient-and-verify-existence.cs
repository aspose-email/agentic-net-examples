using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "imap.gmail.com";
            string username = "your_email@gmail.com";
            string password = "your_password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (username.Contains("your_email") || password.Contains("your_password"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail IMAP operations.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Authentication failed: " + ex.Message);
                    return;
                }

                // Define the nested folder path.
                string folderPath = "Projects/2024/Alpha";

                // Create each level of the nested folder structure.
                // IMAP does not automatically create intermediate folders, so we create them step by step.
                string[] parts = folderPath.Split('/');
                string currentPath = string.Empty;
                foreach (string part in parts)
                {
                    currentPath = string.IsNullOrEmpty(currentPath) ? part : currentPath + "/" + part;
                    // Create the folder if it does not already exist.
                    if (!client.ExistFolder(currentPath))
                    {
                        try
                        {
                            client.CreateFolder(currentPath);
                            Console.WriteLine($"Created folder: {currentPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create folder '{currentPath}': {ex.Message}");
                            return;
                        }
                    }
                }

                // Verify the final folder exists.
                bool exists = client.ExistFolder(folderPath);
                Console.WriteLine($"Folder '{folderPath}' exists: {exists}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
