using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This example loads an MSG file, extracts embedded linked resources,
            // and saves each resource's content stream to a local file.

            string inputMsgPath = "input.msg";

            // Verify the input MSG file exists.
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file.
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Convert the MAPI message to a MailMessage to access LinkedResources.
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions))
            {
                // Ensure the output directory exists.
                string outputDirectory = "output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Iterate over each linked resource in the email.
                foreach (LinkedResource resource in mailMsg.LinkedResources)
                {
                    // Skip if the resource does not have a content stream.
                    if (resource.ContentStream == null)
                    {
                        continue;
                    }

                    // Determine a file name for the resource.
                    string fileName = resource.ContentId;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "resource.bin";
                    }
                    string outputPath = Path.Combine(outputDirectory, fileName);

                    try
                    {
                        // Save the content stream to the file.
                        using (Stream outputFile = File.Create(outputPath))
                        {
                            resource.ContentStream.CopyTo(outputFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save linked resource to '{outputPath}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
