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
            string inputPath = "appointment.msg";
            string outputPath = "appointment_updated.msg";

            // Verify input file exists
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
                    placeholderCalendar.Save(inputPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Confirm the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(outputPath, ".ics");
                    try
                    {
                        File.WriteAllText(placeholderIcsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                        Console.WriteLine($"Input MSG is not a calendar item. Placeholder ICS created at {placeholderIcsPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing placeholder ICS: {ex.Message}");
                    }
                    return;

                    Console.Error.WriteLine("Error: The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Update basic properties
                calendar.Subject = "Updated Meeting Subject";
                calendar.Location = "Conference Room B";
                calendar.StartDate = new DateTime(2024, 5, 20, 10, 0, 0);
                calendar.EndDate = new DateTime(2024, 5, 20, 11, 0, 0);
                calendar.Body = "Updated meeting agenda and details.";

                // Build attendees using MapiCalendarAttendees
                MapiCalendarAttendees attendees = new MapiCalendarAttendees();
                // Populate the AppointmentRecipients collection
                attendees.AppointmentRecipients.Add("alice@example.com", "Alice", MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients.Add("bob@example.com", "Bob", MapiRecipientType.MAPI_TO);
                // Assign the attendees to the calendar
                calendar.Attendees = attendees;

                // Save the updated calendar back to MSG
                MapiCalendarMsgSaveOptions saveOptions = new MapiCalendarMsgSaveOptions();
                calendar.Save(outputPath, saveOptions);
            }

            Console.WriteLine("Appointment updated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
