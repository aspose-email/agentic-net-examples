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
            string msgPath = "calendar.msg";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Ensure the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Display basic calendar information
                Console.WriteLine($"Subject: {calendar.Subject}");
                Console.WriteLine($"Start: {calendar.StartDate}");
                Console.WriteLine($"End: {calendar.EndDate}");

                // Save the calendar as an iCalendar file
                string icsPath = "output.ics";
                calendar.Save(icsPath);
                Console.WriteLine($"Calendar saved to {icsPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
