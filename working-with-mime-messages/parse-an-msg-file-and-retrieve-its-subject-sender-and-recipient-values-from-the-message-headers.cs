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
            const string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (var placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file.
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Retrieve and display basic header information.
            Console.WriteLine("Subject: " + msg.Subject);
            Console.WriteLine("From: " + msg.SenderEmailAddress);

            // Retrieve and display "To" recipients (only MAPI_TO type).
            if (msg.Recipients != null && msg.Recipients.Count > 0)
            {
                Console.Write("To: ");
                bool first = true;
                foreach (MapiRecipient recipient in msg.Recipients)
                {
                    if (recipient.RecipientType == MapiRecipientType.MAPI_TO)
                    {
                        if (!first) Console.Write("; ");
                        Console.Write(recipient.EmailAddress);
                        first = false;
                    }
                }

                if (first) // No MAPI_TO recipients found.
                {
                    Console.Write("(none)");
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("To: (none)");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
