using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.ics";

            // Guard input file existence
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Ensure the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    // Save as iCalendar (ICS) format
                    calendar.Save(outputPath, AppointmentSaveFormat.Ics);
                }
            }

            Console.WriteLine($"Conversion completed successfully. ICS file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}