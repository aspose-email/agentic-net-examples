using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    // Author: Aspose.Email example for retrieving sender email from MSG
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgPath = "message.msg";

                // Guard file existence
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                // Load the MSG file
                MapiMessage mapiMessage = MapiMessage.Load(msgPath);

                // Convert to MailMessage to access From.Address
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Retrieve sender's email address
                    string senderEmail = mailMessage.From?.Address;
                    Console.WriteLine($"Sender Email: {senderEmail}");
                }
            }
            catch (Exception ex)
            {
                // Output any unexpected errors
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
