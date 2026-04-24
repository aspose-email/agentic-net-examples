using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – real values should be provided in production
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected to avoid unwanted network calls
            if (serverUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = "ExportedEmails";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Path for the CSV report
            string csvPath = Path.Combine(outputDir, "report.csv");

            // Write CSV header
            using (var csvWriter = new StreamWriter(csvPath, false, Encoding.UTF8))
            {
                csvWriter.WriteLine("Subject,Folder,ExportPath");
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                foreach (var msgInfo in messages)
                {
                    // Fetch the full message using its unique URI
                    MailMessage message = client.FetchMessage(msgInfo.UniqueUri);

                    // Build a safe file name from the subject
                    string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : MakeFileNameSafe(message.Subject);
                    string exportPath = Path.Combine(outputDir, safeSubject + ".eml");

                    // Save the message to the file system
                    message.Save(exportPath);

                    // Append entry to the CSV report
                    using (var csvWriter = new StreamWriter(csvPath, true, Encoding.UTF8))
                    {
                        csvWriter.WriteLine($"\"{EscapeCsv(message.Subject)}\",\"Inbox\",\"{exportPath}\"");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    // Replaces characters that are invalid in file names with an underscore
    private static string MakeFileNameSafe(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }

    // Escapes double quotes for CSV fields
    private static string EscapeCsv(string field)
    {
        if (field == null) return string.Empty;
        return field.Replace("\"", "\"\"");
    }
}
