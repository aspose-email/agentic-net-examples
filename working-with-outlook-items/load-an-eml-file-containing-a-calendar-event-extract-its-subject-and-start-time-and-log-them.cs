using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            const string emlPath = "calendar.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple mail message with a calendar appointment attached.
                    MailMessage placeholderMessage = new MailMessage(
                        "organizer@example.com",
                        "attendee@example.com",
                        "Meeting Invitation",
                        "Please find the meeting details attached.");

                    // Create a basic appointment (subject, start, end, organizer, attendees).
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee@example.com"));
                    Appointment appointment = new Appointment(
                        "Meeting Invitation",
                        DateTime.Now.AddHours(1),
                        DateTime.Now.AddHours(2),
                        new MailAddress("organizer@example.com"),
                        attendees);
                    appointment.Summary = "Meeting Invitation";

                    // Add the appointment as an alternate view (iCalendar) to the message.
                    placeholderMessage.AlternateViews.Add(appointment.RequestApointment());

                    // Save the placeholder EML file.
                    placeholderMessage.Save(emlPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load the EML message.
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                bool calendarFound = false;

                // Search for a calendar attachment (Content-Type: text/calendar).
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    if (attachment.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase))
                    {
                        using (Stream calendarStream = attachment.ContentStream)
                        {
                            // Load the appointment from the calendar stream.
                            Appointment appointment = Appointment.Load(calendarStream);

                            // Log subject (use appointment summary if available) and start time.
                            string subject = !string.IsNullOrEmpty(appointment.Summary) ? appointment.Summary : mailMessage.Subject;
                            Console.WriteLine($"Subject: {subject}");
                            Console.WriteLine($"Start Time: {appointment.StartDate}");

                            calendarFound = true;
                            break;
                        }
                    }
                }

                if (!calendarFound)
                {
                    Console.Error.WriteLine("No calendar attachment found in the EML file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
