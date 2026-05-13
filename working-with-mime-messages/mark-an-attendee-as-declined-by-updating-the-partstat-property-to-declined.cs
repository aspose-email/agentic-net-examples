using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "appointment.ics";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(icsPath))
            {
                Appointment placeholder = new Appointment(
                    "Placeholder Meeting",
                    DateTime.Now.AddHours(1),
                    DateTime.Now.AddHours(2),
                    new MailAddress("organizer@example.com"),
                    new MailAddressCollection());

                placeholder.Save(icsPath);
                Console.WriteLine("Created placeholder appointment at " + icsPath);
                return;
            }

            // Load the existing appointment (optional, just to verify it's a valid iCalendar file).
            Appointment appointment = Appointment.Load(icsPath);

            // Email address of the attendee whose participation status should be set to DECLINED.
            string attendeeEmail = "attendee@example.com";

            // Read all lines from the .ics file.
            string[] lines = File.ReadAllLines(icsPath);
            bool attendeeFound = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // Look for ATTENDEE lines that contain the target email.
                if (line.StartsWith("ATTENDEE", StringComparison.OrdinalIgnoreCase) &&
                    line.IndexOf(attendeeEmail, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Replace any existing PARTSTAT value with DECLINED.
                    // Example part: PARTSTAT=ACCEPTED
                    int partStatIndex = line.IndexOf("PARTSTAT=", StringComparison.OrdinalIgnoreCase);
                    if (partStatIndex >= 0)
                    {
                        int start = partStatIndex + "PARTSTAT=".Length;
                        int end = line.IndexOf(';', start);
                        if (end == -1) end = line.IndexOf(':', start);
                        if (end == -1) end = line.Length;
                        string before = line.Substring(0, start);
                        string after = line.Substring(end);
                        line = before + "DECLINED" + after;
                    }
                    else
                    {
                        // If PARTSTAT is missing, insert it before the first semicolon or colon.
                        int insertPos = line.IndexOf(';');
                        if (insertPos == -1) insertPos = line.IndexOf(':');
                        if (insertPos == -1) insertPos = line.Length;
                        line = line.Insert(insertPos, ";PARTSTAT=DECLINED");
                    }

                    lines[i] = line;
                    attendeeFound = true;
                    break;
                }
            }

            // If the attendee was not present, add a new ATTENDEE line with DECLINED status.
            if (!attendeeFound)
            {
                // Build a simple ATTENDEE line. Adjust parameters as needed.
                string newAttendeeLine = $"ATTENDEE;CN=Attendee;ROLE=REQ-PARTICIPANT;PARTSTAT=DECLINED:mailto:{attendeeEmail}";
                // Insert before the END:VEVENT line if present, otherwise append.
                int insertIndex = Array.FindLastIndex(lines, l => l.StartsWith("END:VEVENT", StringComparison.OrdinalIgnoreCase));
                if (insertIndex >= 0)
                {
                    var list = new System.Collections.Generic.List<string>(lines);
                    list.Insert(insertIndex, newAttendeeLine);
                    lines = list.ToArray();
                }
                else
                {
                    var list = new System.Collections.Generic.List<string>(lines) { newAttendeeLine };
                    lines = list.ToArray();
                }
            }

            // Write the updated content back to the file.
            File.WriteAllLines(icsPath, lines);
            Console.WriteLine("Attendee status updated to DECLINED.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
