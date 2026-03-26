using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path for a sample EML file that Thunderbird can import
            string sampleEmlPath = "sample.eml";

            // Ensure the sample EML file exists; create a minimal placeholder if missing
            if (!File.Exists(sampleEmlPath))
            {
                using (MailMessage placeholderMessage = new MailMessage())
                {
                    placeholderMessage.From = new MailAddress("sender@example.com");
                    placeholderMessage.To = new MailAddressCollection { "recipient@example.com" };
                    placeholderMessage.Subject = "Test Email for Thunderbird";
                    placeholderMessage.Body = "This is a test email generated for Thunderbird integration.";
                    placeholderMessage.Save(sampleEmlPath, SaveOptions.DefaultEml);
                }

                Console.WriteLine("Created placeholder email at: " + sampleEmlPath);
            }
            else
            {
                // Load and display basic information from the existing EML file
                using (MailMessage loadedMessage = MailMessage.Load(sampleEmlPath))
                {
                    Console.WriteLine("Loaded Email:");
                    Console.WriteLine("Subject: " + loadedMessage.Subject);
                    Console.WriteLine("From: " + loadedMessage.From);
                    Console.WriteLine("To: " + string.Join(", ", loadedMessage.To));
                }
            }

            // Directory where Thunderbird stores or can import multiple EML files
            string emailsFolder = "Emails";

            // Ensure the directory exists
            if (!Directory.Exists(emailsFolder))
            {
                Directory.CreateDirectory(emailsFolder);
            }

            // List all EML files in the directory
            string[] emlFiles = Directory.GetFiles(emailsFolder, "*.eml");
            Console.WriteLine("EML files in folder '" + emailsFolder + "':");
            foreach (string emlFile in emlFiles)
            {
                Console.WriteLine("- " + Path.GetFileName(emlFile));
            }

            // Example: Delete a specific email file if it exists
            string deleteFilePath = Path.Combine(emailsFolder, "old.eml");
            if (File.Exists(deleteFilePath))
            {
                try
                {
                    File.Delete(deleteFilePath);
                    Console.WriteLine("Deleted file: " + deleteFilePath);
                }
                catch (Exception deleteEx)
                {
                    Console.Error.WriteLine("Failed to delete file: " + deleteEx.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}