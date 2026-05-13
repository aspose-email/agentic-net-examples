using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "sample.ics";

            // Guard file existence
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
            }

            Appointment appointment = null;
            try
            {
                // Load the iCalendar file
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                return;
            }

            // Validate required properties
            List<string> missingProperties = new List<string>();

            if (string.IsNullOrEmpty(appointment.UniqueId))
                missingProperties.Add("UID");

            if (appointment.DateTimeStamp == default(DateTime))
                missingProperties.Add("DTSTAMP");

            if (appointment.StartDate == default(DateTime))
                missingProperties.Add("DTSTART");

            if (appointment.EndDate == default(DateTime))
                missingProperties.Add("DTEND");

            if (string.IsNullOrEmpty(appointment.Summary))
                missingProperties.Add("SUMMARY");

            if (string.IsNullOrEmpty(appointment.Location))
                missingProperties.Add("LOCATION");

            // Report results
            if (missingProperties.Count == 0)
            {
                Console.WriteLine("All required iCalendar properties are present.");
            }
            else
            {
                Console.WriteLine("Missing required iCalendar properties:");
                foreach (string prop in missingProperties)
                {
                    Console.WriteLine($"- {prop}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
