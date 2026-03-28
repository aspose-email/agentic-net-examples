using System;
using System.IO;
using Aspose.Email;

namespace EmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add Aspose.Email via NuGet: Install-Package Aspose.Email
            try
            {
                string outputPath = "output.eml";
                string directoryPath = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                MailMessage message = new MailMessage();
                try
                {
                    message.From = new MailAddress("sender@example.com");
                    MailAddressCollection toAddresses = new MailAddressCollection();
                    toAddresses.Add(new MailAddress("recipient@example.com"));
                    message.To = toAddresses;
                    message.Subject = "Test Email";
                    message.Body = "This is a test email.";

                    message.Save(outputPath, SaveOptions.DefaultEml);
                }
                finally
                {
                    if (message != null)
                    {
                        message.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
