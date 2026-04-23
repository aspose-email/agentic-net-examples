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
            string inputPath = "meetingRequest.msg";
            string outputPath = "meetingRequest.ics";

            // Guard file existence
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(inputPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Verify that the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    // Create minimal placeholder iCalendar file
                    try
                    {
                        File.WriteAllText(outputPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                        Console.WriteLine("Input MSG is not a calendar. Placeholder iCalendar created.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write placeholder iCalendar: {ex.Message}");
                    }
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar mapiCalendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Ensure attendees are set via MapiCalendarAttendees
                MapiCalendarAttendees attendees = new MapiCalendarAttendees
                {
                    AppointmentRecipients = new MapiRecipientCollection()
                };

                // Example: copy existing recipients if any
                if (mapiCalendar.Attendees != null && mapiCalendar.Attendees.AppointmentRecipients != null)
                {
                    foreach (MapiRecipient recipient in mapiCalendar.Attendees.AppointmentRecipients)
                    {
                        attendees.AppointmentRecipients.Add(recipient);
                    }
                }

                // If there were no attendees, you could add custom ones here:
                // attendees.AppointmentRecipients.Add(new MapiRecipient("person1@example.com"));
                // attendees.AppointmentRecipients.Add(new MapiRecipient("person2@example.com"));

                mapiCalendar.Attendees = attendees;

                // Save as iCalendar preserving time‑zone information
                try
                {
                    mapiCalendar.Save(outputPath, new MapiCalendarIcsSaveOptions());
                    Console.WriteLine($"iCalendar saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save iCalendar: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
