using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input email file (EML or MSG) containing an embedded image.
            const string sourceEmailPath = "email.eml";

            // Verify the source file exists.
            if (!File.Exists(sourceEmailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourceEmailPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source email file not found: {sourceEmailPath}");
                return;
            }

            // Load the email message.
            using (MailMessage email = MailMessage.Load(sourceEmailPath))
            {
                // Ensure there is at least one embedded resource.
                if (email.LinkedResources.Count == 0)
                {
                    Console.Error.WriteLine("No embedded resources found in the email.");
                    return;
                }

                // Extract the first linked resource (e.g., an image).
                LinkedResource resource = email.LinkedResources[0];
                const string extractedImagePath = "extracted_image.jpg";

                // Save the content stream of the linked resource to a file.
                try
                {
                    using (FileStream fileStream = new FileStream(extractedImagePath, FileMode.Create, FileAccess.Write))
                    {
                        resource.ContentStream.CopyTo(fileStream);
                    }
                    Console.WriteLine($"Embedded image saved to: {extractedImagePath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write embedded image: {ioEx.Message}");
                    return;
                }

                // Save the original email as a MSG file.
                const string outputMsgPath = "output.msg";
                try
                {
                    email.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Email saved as MSG to: {outputMsgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save email as MSG: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
