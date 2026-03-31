using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputDirectory = "LinkedResources";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Save(inputMsgPath, SaveOptions.DefaultMsg);
                        Console.WriteLine($"Created placeholder MSG at '{inputMsgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {ex.Message}");
                return;
            }

            // Load the MSG file
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(inputMsgPath))
                {
                    int index = 0;
                    foreach (LinkedResource linkedResource in mailMessage.LinkedResources)
                    {
                        string fileName = linkedResource.ContentId;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = $"resource_{index}.bin";
                        }

                        string outputPath = Path.Combine(outputDirectory, fileName);
                        try
                        {
                            linkedResource.Save(outputPath);
                            Console.WriteLine($"Saved linked resource to '{outputPath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save linked resource '{fileName}': {ex.Message}");
                        }

                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file '{inputMsgPath}': {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
