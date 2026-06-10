using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            string msgPath = Path.Combine(outputDir, "CustomCalendar.msg");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a simple appointment
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                organizer,
                attendees);
            appointment.Location = "Conference Room";
            appointment.Description = "Discuss project milestones.";

            // Convert the appointment to a MAPI message
            using (MapiMessage msg = appointment.ToMapiMessage())
            {
                // Add a custom X-Property (must encode string value as Unicode bytes)
                const string propName = "X-MyCustomProp";
                const string propValue = "CustomValue";
                byte[] valueBytes = Encoding.Unicode.GetBytes(propValue);
                msg.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, propName);

                // Save the message as MSG
                msg.Save(msgPath);
            }

            // Verify that the custom property appears in the raw MSG data
            if (File.Exists(msgPath))
            {
                byte[] rawData = File.ReadAllBytes(msgPath);
                byte[] nameBytes = Encoding.Unicode.GetBytes("X-MyCustomProp");
                bool found = false;

                // Simple byte pattern search
                for (int i = 0; i <= rawData.Length - nameBytes.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < nameBytes.Length; j++)
                    {
                        if (rawData[i + j] != nameBytes[j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        found = true;
                        break;
                    }
                }

                Console.WriteLine(found
                    ? "Custom X-Property successfully added and verified in MSG file."
                    : "Custom X-Property not found in MSG file.");
            }
            else
            {
                Console.Error.WriteLine("Failed to create MSG file at: " + msgPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
