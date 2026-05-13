using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Guard against missing input file
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
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

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Iterate through all alternate views to find linked resources
                foreach (AlternateView view in message.AlternateViews)
                {
                    // Collect linked resources to remove (e.g., all of them)
                    List<LinkedResource> resourcesToRemove = new List<LinkedResource>();
                    foreach (LinkedResource resource in view.LinkedResources)
                    {
                        // Example condition: remove any linked resource (inline image)
                        // Adjust condition as needed, e.g., check ContentId
                        resourcesToRemove.Add(resource);
                    }

                    // Remove the collected linked resources from the view
                    foreach (LinkedResource resource in resourcesToRemove)
                    {
                        view.LinkedResources.Remove(resource);
                    }
                }

                // Save the modified message
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
