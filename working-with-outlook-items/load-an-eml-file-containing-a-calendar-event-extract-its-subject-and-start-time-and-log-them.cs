using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the EML file containing the calendar event
            string emlPath = "calendar.eml";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the EML file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(emlPath))
            {
                // Ensure the message contains a calendar item
                if (mapiMessage.SupportedType == MapiItemType.Calendar)
                {
                    // Convert the MapiMessage to a MapiCalendar object
                    MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                    // Log the subject and start time of the calendar event
                    Console.WriteLine($"Subject: {calendar.Subject}");
                    Console.WriteLine($"Start Time: {calendar.StartDate}");
                }
                else
                {
                    Console.WriteLine("The provided EML file does not contain a calendar event.");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
