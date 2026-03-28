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
            // Path to the MSG file containing the calendar entry
            string msgFilePath = "calendar.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Ensure the loaded message is a calendar item
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    // Convert the MAPI message to a MapiCalendar object
                    using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                    {
                        // Example processing: display basic calendar information
                        Console.WriteLine($"Subject: {calendar.Subject}");
                        Console.WriteLine($"Location: {calendar.Location}");
                        Console.WriteLine($"Start: {calendar.StartDate}");
                        Console.WriteLine($"End: {calendar.EndDate}");
                        // Additional processing can be performed here
                    }
                }
                else
                {
                    Console.WriteLine("The MSG file does not contain a calendar item.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
