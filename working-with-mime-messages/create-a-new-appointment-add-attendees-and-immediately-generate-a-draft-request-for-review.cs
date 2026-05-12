using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                "Team Sync",
                "Discuss project updates",
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Generate a draft request (iCalendar invitation)
            AlternateView requestView = appointment.RequestApointment();

            // Build a draft email message containing the request
            using (MailMessage draft = new MailMessage())
            {
                draft.From = new MailAddress("organizer@domain.com");
                draft.To.Add(new MailAddress("reviewer@domain.com"));
                draft.Subject = "Draft: " + appointment.Summary;
                draft.AlternateViews.Add(requestView);

                // Save the draft to a file
                string outputPath = "DraftRequest.eml";
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        draft.Save(fs, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode));
                    }

                    Console.WriteLine($"Draft request saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving draft: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
