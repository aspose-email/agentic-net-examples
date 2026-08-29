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
            // Input EML file containing embedded images
            string inputEmlPath = "input.eml";
            // Output MSG file
            string outputMsgPath = "output.msg";
            // Path to save the extracted image
            string extractedImagePath = "extracted_image.jpg";

            // Guard input file existence
            if (!File.Exists(inputEmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputEmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputEmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputEmlPath))
            {
                // Extract the first linked resource (embedded image) if any
                if (mailMessage.LinkedResources.Count > 0)
                {
                    // Assuming the first linked resource is the image we want
                    LinkedResource linkedResource = mailMessage.LinkedResources[0];
                    using (Stream contentStream = linkedResource.ContentStream)
                    {
                        if (contentStream != null)
                        {
                            try
                            {
                                using (FileStream fileStream = new FileStream(extractedImagePath, FileMode.Create, FileAccess.Write))
                                {
                                    contentStream.CopyTo(fileStream);
                                }
                                Console.WriteLine($"Extracted image saved to: {extractedImagePath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to write extracted image: {ioEx.Message}");
                                // Continue without aborting; the MSG will still be saved
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No linked resources (embedded images) found in the email.");
                }

                // Convert the MailMessage to a MapiMessage (preserves embedded resources)
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Save as MSG file
                    try
                    {
                        mapiMessage.Save(outputMsgPath);
                        Console.WriteLine($"MSG file saved to: {outputMsgPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
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
