using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MSG and output EML paths
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputEmlPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the MSG file and convert it to MailMessage
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                MailMessage mail = msg.ToMailMessage(new MailConversionOptions());

                // Save as EML with default options
                using (mail)
                {
                    var emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                    mail.Save(outputEmlPath, emlSaveOptions);
                }
            }

            // Validate the generated EML for correct MIME structure
            try
            {
                MessageValidationResult validationResult = MessageValidator.Validate(outputEmlPath);
                if (validationResult.IsSuccess)
                {
                    Console.WriteLine("EML file generated successfully and MIME boundaries are valid.");
                }
                else
                {
                    Console.Error.WriteLine("EML validation failed. Errors:");
                    foreach (var error in validationResult.Errors)
                    {
                        Console.Error.WriteLine($"- {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Validation error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
