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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the Outlook MSG file inside a using block to guarantee disposal.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Access basic metadata.
                string senderName = msg.SenderName;
                string senderEmail = msg.SenderEmailAddress;
                string subject = msg.Subject;
                DateTime? deliveryTime = msg.DeliveryTime;

                // Access raw transport headers (as a single string).
                string transportHeaders = msg.TransportMessageHeaders;

                // Output the retrieved information.
                Console.WriteLine($"Sender Name   : {senderName}");
                Console.WriteLine($"Sender Email  : {senderEmail}");
                Console.WriteLine($"Subject       : {subject}");
                Console.WriteLine($"Delivery Time : {(deliveryTime.HasValue ? deliveryTime.Value.ToString() : "N/A")}");
                Console.WriteLine("Transport Headers:");
                Console.WriteLine(transportHeaders);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
