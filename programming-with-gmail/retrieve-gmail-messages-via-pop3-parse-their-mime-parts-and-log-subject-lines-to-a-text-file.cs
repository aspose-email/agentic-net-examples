using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server settings (replace with real credentials)
            string host = "pop.gmail.com";
            int port = 995;
            string username = "your_username";
            string password = "your_password";

            // Guard against placeholder credentials
            if (username == "your_username" || password == "your_password")
            {
                Console.Error.WriteLine("Please provide valid Gmail POP3 credentials.");
                return;
            }

            // Output file for subject lines
            string outputPath = "subjects.txt";

            // Ensure output directory exists
            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Write subjects to the file
            try
            {
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Connect to POP3 server
                    using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                    {
                        try
                        {
                            client.ValidateCredentials();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to validate POP3 credentials: {ex.Message}");
                            return;
                        }

                        int messageCount;
                        try
                        {
                            messageCount = client.GetMessageCount();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to retrieve message count: {ex.Message}");
                            return;
                        }

                        for (int i = 1; i <= messageCount; i++)
                        {
                            try
                            {
                                using (MailMessage message = client.FetchMessage(i))
                                {
                                    string subject = message.Subject ?? "(no subject)";
                                    writer.WriteLine(subject);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error processing message #{i}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write subjects to file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
