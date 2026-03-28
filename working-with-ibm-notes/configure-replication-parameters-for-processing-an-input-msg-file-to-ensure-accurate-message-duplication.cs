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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(inputPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file into a MapiMessage object
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Example: configure a replication property (optional)
                // int replicationSize = (int)msg.Size;
                // MapiProperty replicationProperty = new MapiProperty(KnownPropertyList.EmsAbReplicationMailMsgSize, replicationSize);
                // msg.AddCustomProperty(replicationProperty, "ReplicationSize");

                // Save the message to a new file to ensure accurate duplication
                msg.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
