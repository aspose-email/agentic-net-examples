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
            // Define output file paths
            string outputPathKeepTrue = "calendar_keeptrue.ics";
            string outputPathKeepFalse = "calendar_keepfalse.ics";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPathKeepTrue));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a sample MapiCalendar
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.StartDate = new DateTime(2024, 12, 25, 10, 0, 0);
                calendar.EndDate = new DateTime(2024, 12, 25, 11, 0, 0);
                calendar.Subject = "Sample Meeting";
                calendar.Location = "Conference Room";

                // Save with KeepOriginalDateTimeStamp = true
                MapiCalendarIcsSaveOptions optionsTrue = new MapiCalendarIcsSaveOptions
                {
                    KeepOriginalDateTimeStamp = true
                };
                calendar.Save(outputPathKeepTrue, optionsTrue);

                // Save with KeepOriginalDateTimeStamp = false
                MapiCalendarIcsSaveOptions optionsFalse = new MapiCalendarIcsSaveOptions
                {
                    KeepOriginalDateTimeStamp = false
                };
                calendar.Save(outputPathKeepFalse, optionsFalse);
            }

            // Verify that both files were created
            if (!File.Exists(outputPathKeepTrue) || !File.Exists(outputPathKeepFalse))
            {
                Console.Error.WriteLine("One or both output files were not created.");
                return;
            }

            // Load the saved .ics files as Appointment objects
            Appointment appointmentTrue = Appointment.Load(outputPathKeepTrue);
            Appointment appointmentFalse = Appointment.Load(outputPathKeepFalse);

            // Compare time zone properties (they should be equal)
            bool startTimeZoneEqual = string.Equals(appointmentTrue.StartTimeZone, appointmentFalse.StartTimeZone, StringComparison.Ordinal);
            bool endTimeZoneEqual = string.Equals(appointmentTrue.EndTimeZone, appointmentFalse.EndTimeZone, StringComparison.Ordinal);

            Console.WriteLine($"StartTimeZone equal in both files: {startTimeZoneEqual}");
            Console.WriteLine($"EndTimeZone equal in both files: {endTimeZoneEqual}");

            // Display DateTimeStamp values for inspection
            Console.WriteLine($"DateTimeStamp (KeepOriginalDateTimeStamp=true): {appointmentTrue.DateTimeStamp}");
            Console.WriteLine($"DateTimeStamp (KeepOriginalDateTimeStamp=false): {appointmentFalse.DateTimeStamp}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
