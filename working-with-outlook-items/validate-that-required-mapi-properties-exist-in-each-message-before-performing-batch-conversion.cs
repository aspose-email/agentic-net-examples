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
            // Define input and output directories
            string inputFolder = "InputMessages";
            string outputFolder = "ConvertedMessages";

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard against missing input directory
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputFolder}");
                return;
            }

            // Get all MSG files in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            // Define required MAPI property tags (Unicode variants)
            const long PR_SUBJECT_UNICODE = 0x0037001F;               // Subject
            const long PR_SENDER_EMAIL_ADDRESS_UNICODE = 0x0C1F001F; // Sender email address

            foreach (string msgPath in msgFiles)
            {
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
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        // Validate required properties
                        string subject = msg.GetPropertyString(PR_SUBJECT_UNICODE);
                        string senderEmail = msg.GetPropertyString(PR_SENDER_EMAIL_ADDRESS_UNICODE);

                        bool hasSubject = !string.IsNullOrEmpty(subject);
                        bool hasSender = !string.IsNullOrEmpty(senderEmail);

                        if (!hasSubject || !hasSender)
                        {
                            Console.Error.WriteLine($"Required properties missing in: {Path.GetFileName(msgPath)}");
                            continue;
                        }

                        // Convert to MailMessage
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                        {
                            // Save as EML
                            string outputPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(msgPath) + ".eml");
                            try
                            {
                                mail.Save(outputPath);
                                Console.WriteLine($"Converted and saved: {outputPath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save EML for {msgPath}: {saveEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to process {msgPath}: {loadEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
