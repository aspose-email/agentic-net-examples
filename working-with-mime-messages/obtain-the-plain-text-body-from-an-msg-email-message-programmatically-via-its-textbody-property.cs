using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            const string msgPath = "sample.msg";

            // Verify the input file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file and convert it to MailMessage
            using (MapiMessage mapiMsg = MapiMessage.Load(msgPath))
            {
                MailConversionOptions conversionOpts = new MailConversionOptions();
                using (MailMessage mail = mapiMsg.ToMailMessage(conversionOpts))
                {
                    // Obtain the plain‑text body via the Body property
                    string plainBody = mail.Body;
                    Console.WriteLine("Plain‑text body:");
                    Console.WriteLine(plainBody);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
