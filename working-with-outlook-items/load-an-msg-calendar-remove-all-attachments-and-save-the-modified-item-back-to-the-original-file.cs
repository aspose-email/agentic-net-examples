using Aspose.Email.Calendar;
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
            string msgPath = "calendar.msg";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
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

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Check if the MSG file is a calendar item
                if (string.IsNullOrEmpty(message.MessageClass) ||
                    !message.MessageClass.StartsWith("IPM.Appointment", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("The MSG file is not a calendar item.");
                    return;
                }

                // Remove all attachments
                message.Attachments?.Clear();

                // Save the modified message back to the original file
                message.Save(msgPath);
                Console.WriteLine("Attachments removed and file saved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
