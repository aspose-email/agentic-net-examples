using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: simple console app to modify the sender of a MSG file.
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Guard file existence
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage mapMsg = MapiMessage.Load(inputPath))
            {
                // Convert to MailMessage for easier manipulation
                using (MailMessage mailMsg = mapMsg.ToMailMessage(new MailConversionOptions()))
                {
                    // Assign the sender address
                    mailMsg.From = new MailAddress("sender@example.com");

                    // Save the modified message back to MSG format
                    mailMsg.Save(outputPath);
                }
            }

            Console.WriteLine($"Message saved with new sender to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
