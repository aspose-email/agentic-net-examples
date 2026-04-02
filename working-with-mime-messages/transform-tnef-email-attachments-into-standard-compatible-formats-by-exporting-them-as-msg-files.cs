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
            // Define input and output locations
            string inputFilePath = "input.eml";
            string outputDirectory = "output";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputFilePath))
            {
                try
                {
                    string placeholderContent = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputFilePath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            // Load the email with TNEF attachment preservation
            EmlLoadOptions loadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputFilePath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading email file: {ex.Message}");
                return;
            }

            // Process each attachment
            foreach (Attachment attachment in mailMessage.Attachments)
            {
                if (attachment.IsTnef)
                {
                    // Load the TNEF attachment as a MapiMessage
                    MapiMessage tnefMessage;
                    try
                    {
                        using (Stream tnefStream = attachment.ContentStream)
                        {
                            tnefMessage = MapiMessage.LoadFromTnef(tnefStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error loading TNEF attachment '{attachment.Name}': {ex.Message}");
                        continue;
                    }

                    // Determine output MSG file path
                    string safeName = string.IsNullOrEmpty(attachment.Name) ? "attachment" : Path.GetFileNameWithoutExtension(attachment.Name);
                    string outputMsgPath = Path.Combine(outputDirectory, safeName + ".msg");

                    // Save the MapiMessage as MSG
                    try
                    {
                        tnefMessage.Save(outputMsgPath);
                        Console.WriteLine($"Saved MSG file: {outputMsgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MSG file '{outputMsgPath}': {ex.Message}");
                    }
                }
            }

            // Dispose the loaded MailMessage
            mailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
