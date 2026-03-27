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

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Example processing of the calendar entry
                    Console.WriteLine($"Subject: {calendar.Subject}");
                    Console.WriteLine($"Start: {calendar.StartDate}");
                    Console.WriteLine($"End: {calendar.EndDate}");
                    Console.WriteLine($"Location: {calendar.Location}");
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
