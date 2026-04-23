using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailAttachmentOrderTest
{
    class Program
    {
        static void Main()
        {
            try
            {
                TestAttachmentOrder();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void TestAttachmentOrder()
        {
            // Create a mail message with two attachments in a specific order
            using (MailMessage originalMessage = new MailMessage())
            {
                originalMessage.From = "sender@example.com";
                originalMessage.To.Add("recipient@example.com");
                originalMessage.Subject = "Attachment Order Test";
                originalMessage.Body = "Testing attachment order preservation.";

                // First attachment
                byte[] data1 = System.Text.Encoding.UTF8.GetBytes("First attachment content");
                using (MemoryStream stream1 = new MemoryStream(data1))
                {
                    Attachment attachment1 = new Attachment(stream1, "first.txt");
                    originalMessage.Attachments.Add(attachment1);
                }

                // Second attachment
                byte[] data2 = System.Text.Encoding.UTF8.GetBytes("Second attachment content");
                using (MemoryStream stream2 = new MemoryStream(data2))
                {
                    Attachment attachment2 = new Attachment(stream2, "second.txt");
                    originalMessage.Attachments.Add(attachment2);
                }

                // Convert to MapiMessage
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(originalMessage);

                // Convert back to MailMessage
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage convertedMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Verify that the attachment order is preserved
                    if (convertedMessage.Attachments.Count != 2)
                    {
                        Console.Error.WriteLine("Attachment count mismatch after conversion.");
                        return;
                    }

                    string firstName = convertedMessage.Attachments[0].Name;
                    string secondName = convertedMessage.Attachments[1].Name;

                    if (firstName == "first.txt" && secondName == "second.txt")
                    {
                        Console.WriteLine("Attachment order preserved successfully.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Attachment order was not preserved.");
                        Console.Error.WriteLine($"Expected: first.txt, second.txt");
                        Console.Error.WriteLine($"Actual: {firstName}, {secondName}");
                    }
                }
            }
        }
    }
}
