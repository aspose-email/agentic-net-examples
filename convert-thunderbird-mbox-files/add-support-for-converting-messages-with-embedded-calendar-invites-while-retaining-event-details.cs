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
            // Input and output file paths
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder message because the input file was missing.";
                        placeholder.Save(inputMsgPath);
                        Console.WriteLine($"Placeholder message created at '{inputMsgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage mapiMsg;
            try
            {
                mapiMsg = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (mapiMsg)
            {
                // If the message contains a calendar item, retain its details during conversion
                MailConversionOptions convOptions = new MailConversionOptions();

                MailMessage mailMsg;
                try
                {
                    mailMsg = mapiMsg.ToMailMessage(convOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion to MailMessage failed: {ex.Message}");
                    return;
                }

                using (mailMsg)
                {
                    // Save the resulting EML file
                    try
                    {
                        mailMsg.Save(outputEmlPath);
                        Console.WriteLine($"Converted email saved to '{outputEmlPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
