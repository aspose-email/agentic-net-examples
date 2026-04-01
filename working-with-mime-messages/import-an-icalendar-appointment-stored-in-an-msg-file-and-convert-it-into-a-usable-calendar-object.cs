using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "appointment.msg";
            string outputIcsPath = "output.ics";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    placeholderCalendar.Save(inputMsgPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputMsgPath}' not found.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputIcsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Check if the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                    // Create a minimal placeholder iCalendar file
                    string placeholder = "BEGIN:VCALENDAR\r\nEND:VCALENDAR";
                    File.WriteAllText(outputIcsPath, placeholder);
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar mapiCalendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Build an Appointment object from the MapiCalendar data
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();

                Appointment appointment = new Appointment(
                    mapiCalendar.Subject,
                    mapiCalendar.StartDate,
                    mapiCalendar.EndDate,
                    organizer,
                    attendees);

                appointment.Location = mapiCalendar.Location;
                appointment.Description = mapiCalendar.Body;

                // Save the appointment as an iCalendar file
                appointment.Save(outputIcsPath);
                Console.WriteLine($"Appointment saved to {outputIcsPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
