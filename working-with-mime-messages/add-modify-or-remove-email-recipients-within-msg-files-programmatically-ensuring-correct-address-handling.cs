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
            string inputPath = "sample.msg";
            string outputPath = "modified.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder Body"))
                {
                    placeholder.Save(inputPath);
                }
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Add a new TO recipient.
                msg.Recipients.Add("newto@example.com", "New To Recipient", MapiRecipientType.MAPI_TO);

                // Add a new CC recipient.
                msg.Recipients.Add("newcc@example.com", "New CC Recipient", MapiRecipientType.MAPI_CC);

                // Add a new BCC recipient.
                msg.Recipients.Add("newbcc@example.com", "New BCC Recipient", MapiRecipientType.MAPI_BCC);

                // Modify the first recipient's email address if any exist.
                if (msg.Recipients.Count > 0)
                {
                    MapiRecipient firstRecipient = msg.Recipients[0];
                    firstRecipient.EmailAddress = "modified@example.com";
                }

                // Remove the second recipient if there are at least two.
                if (msg.Recipients.Count > 1)
                {
                    msg.Recipients.RemoveAt(1);
                }

                // Save the modified MSG file.
                msg.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
