using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "sample.msg";
            // Output XML summary path
            string outputXmlPath = "summary.xml";

            // Verify input file exists
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

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputXmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Build XML summary
                XDocument xmlDoc = new XDocument(
                    new XElement("EmailSummary",
                        new XElement("Subject", message.Subject ?? string.Empty),
                        new XElement("Sender",
                            new XElement("Name", message.SenderName ?? string.Empty),
                            new XElement("Email", message.SenderEmailAddress ?? string.Empty)
                        ),
                        new XElement("SentRepresenting",
                            new XElement("Name", message.SentRepresentingName ?? string.Empty),
                            new XElement("Email", message.SentRepresentingEmailAddress ?? string.Empty)
                        ),
                        new XElement("DeliveryTime", message.DeliveryTime.ToString("o")),
                        new XElement("ClientSubmitTime", message.ClientSubmitTime.ToString("o")),
                        new XElement("Body", message.Body ?? string.Empty),
                        new XElement("Attachments",
                            from MapiAttachment attachment in message.Attachments
                            select new XElement("Attachment", attachment.FileName ?? string.Empty)
                        )
                    )
                );

                // Save XML to file
                xmlDoc.Save(outputXmlPath);
                Console.WriteLine($"Summary XML saved to {outputXmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
