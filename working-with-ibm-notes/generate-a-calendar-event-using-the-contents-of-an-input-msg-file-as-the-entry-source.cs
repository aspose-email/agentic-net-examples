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
            string msgPath = "input.msg";
            string icsPath = "output.ics";

            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiCalendar placeholder = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Subject",
                        "Placeholder Body",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    placeholder.Save(msgPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            string outputDir = Path.GetDirectoryName(icsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    try
                    {
                        File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                        Console.WriteLine("Input MSG is not a calendar item. Placeholder .ics created.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing placeholder .ics: {ex.Message}");
                    }
                    return;
                }

                MapiCalendar mapiCal = (MapiCalendar)msg.ToMapiMessageItem();

                string organizerAddress = !string.IsNullOrEmpty(mapiCal.Organizer?.EmailAddress)
                    ? mapiCal.Organizer.EmailAddress
                    : "organizer@example.com";

                MailAddressCollection attendees = new MailAddressCollection();
                if (mapiCal.Attendees != null && mapiCal.Attendees.AppointmentRecipients != null)
                {
                    foreach (MapiRecipient recipient in mapiCal.Attendees.AppointmentRecipients)
                    {
                        if (!string.IsNullOrEmpty(recipient.EmailAddress))
                        {
                            attendees.Add(new MailAddress(recipient.EmailAddress));
                        }
                    }
                }

                Appointment appointment = new Appointment(
                    mapiCal.Location,
                    mapiCal.StartDate,
                    mapiCal.EndDate,
                    new MailAddress(organizerAddress),
                    attendees);

                appointment.Summary = mapiCal.Subject;
                appointment.Description = mapiCal.Body;

                appointment.Save(icsPath);
                Console.WriteLine($"Calendar event saved to {icsPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
