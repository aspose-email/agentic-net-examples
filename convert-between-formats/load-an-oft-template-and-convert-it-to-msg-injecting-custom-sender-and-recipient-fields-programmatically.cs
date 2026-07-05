using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        // Author note: This example loads an Outlook OFT template, sets custom sender/recipient,
        // and saves the result as an MSG file.

        string oftPath = "template.oft";
        string msgPath = "output.msg";

        // Guard against missing input file
        if (!File.Exists(oftPath))
        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(oftPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"Input OFT file not found: {oftPath}");
            return;
        }

        try
        {
            // Load the OFT template
            MapiMessage oftMessage = MapiMessage.Load(oftPath);

            // Set custom sender details
            oftMessage.SenderName = "John Doe";
            oftMessage.SenderEmailAddress = "john.doe@example.com";

            // Replace recipients with a single TO recipient
            oftMessage.Recipients.Clear();
            oftMessage.Recipients.Add("jane.smith@example.com",
                "Jane Smith",
                MapiRecipientType.MAPI_TO);

            // Save the modified message as MSG
            oftMessage.Save(msgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
