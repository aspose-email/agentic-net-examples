using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping contact retrieval.");
                return;
            }

            // Create Gmail client. Pass null for proxy (IWebProxy) to match the expected signature.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);
            try
            {
                // Fetch all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                // Prepare CSV output.
                string csvPath = "contacts.csv";
                string directory = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // Write CSV header.
                    writer.WriteLine("Email");

                    // Iterate contacts and write each email address.
                    foreach (Contact contact in contacts)
                    {
                        EmailAddressList emailList = contact.EmailAddresses;
                        foreach (EmailAddress email in emailList)
                        {
                            if (!string.IsNullOrEmpty(email.Address))
                            {
                                writer.WriteLine(email.Address);
                            }
                        }
                    }
                }

                Console.WriteLine($"CSV file created at: {Path.GetFullPath(csvPath)}");
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Gmail client error: {clientEx.Message}");
                return;
            }
            finally
            {
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
