using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the input MSG file
            string inputPath = "sample.msg";

            // Verify the input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
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

            // Load the MSG file
            MapiMessage msg = MapiMessage.Load(inputPath);

            // Display basic header information
            Console.WriteLine("Subject: " + msg.Subject);
            Console.WriteLine("From: " + msg.SenderName + " <" + msg.SenderEmailAddress + ">");
            Console.WriteLine("Sent On: " + msg.DeliveryTime);

            // List TO recipients (validation requires MAPI_TO)
            Console.WriteLine("Recipients (To):");
            foreach (MapiRecipient recipient in msg.Recipients)
            {
                if (recipient.RecipientType == MapiRecipientType.MAPI_TO)
                {
                    Console.WriteLine($"  {recipient.DisplayName} <{recipient.EmailAddress}>");
                }
            }

            // Add a new TO recipient
            msg.Recipients.Add("new.recipient@example.com",
                "New Recipient",
                MapiRecipientType.MAPI_TO);

            // Save the modified MSG to a new file
            string outputPath = "updated.msg";
            msg.Save(outputPath);
            Console.WriteLine($"Updated MSG saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
