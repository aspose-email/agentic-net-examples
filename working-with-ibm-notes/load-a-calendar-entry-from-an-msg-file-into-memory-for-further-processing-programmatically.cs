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
            string msgPath = "calendar.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "organizer@example.com",
                    "attendee@example.com",
                    "Meeting",
                    "This is a meeting."))
                {
                    // Mark the message as a calendar item.
                    placeholder.MessageClass = "IPM.Appointment";
                    placeholder.Save(msgPath);
                }

                Console.WriteLine($"Placeholder MSG file created at {msgPath}");
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    // Convert the MAPI message to a MapiCalendar object.
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Access calendar properties.
                    Console.WriteLine($"Subject: {calendar.Subject}");
                    Console.WriteLine($"Location: {calendar.Location}");
                    Console.WriteLine($"Start: {calendar.StartDate}");
                    Console.WriteLine($"End: {calendar.EndDate}");
                    // Use Body for the calendar description.
                    Console.WriteLine($"Body: {calendar.Body}");
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
