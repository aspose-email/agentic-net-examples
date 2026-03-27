using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace EmailToMsgExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output MSG file path
                string outputFilePath = Path.Combine(Environment.CurrentDirectory, "sample_message.msg");
                string outputDirectory = Path.GetDirectoryName(outputFilePath);

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Error: Unable to create directory – {outputDirectory}. {dirEx.Message}");
                        return;
                    }
                }

                // Create a MailMessage with basic fields
                using (MailMessage mailMessage = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the email."))
                {
                    // Add a custom header
                    mailMessage.Headers.Add("X-Custom-Header", "CustomValue");

                    // Create an attachment from a memory stream
                    byte[] attachmentData = Encoding.UTF8.GetBytes("This is the content of the attachment.");
                    using (MemoryStream attachmentStream = new MemoryStream(attachmentData))
                    {
                        using (Attachment attachment = new Attachment(attachmentStream, "sample.txt", "text/plain"))
                        {
                            mailMessage.Attachments.Add(attachment);

                            // Convert MailMessage to MapiMessage
                            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                            {
                                // Add a custom MAPI property (Unicode string)
                                mapiMessage.AddCustomProperty(
                                    Aspose.Email.Mapi.MapiPropertyType.PT_UNICODE,
                                    Encoding.UTF8.GetBytes("CustomPropertyValue"),
                                    "CustomPropertyName");

                                // Save the MapiMessage as an MSG file
                                try
                                {
                                    mapiMessage.Save(outputFilePath);
                                    Console.WriteLine($"Message successfully saved to: {outputFilePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Error: Unable to save MSG file – {saveEx.Message}");
                                }
                            }
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
}