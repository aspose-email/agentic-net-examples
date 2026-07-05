using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "Invitation.eml";
            string outputPath = "Invitation.pdf";

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            MailMessage message;

            // Load existing email if present; otherwise create a minimal one with a calendar invitation
            if (File.Exists(inputPath))
            {
                try
                {
                    message = MailMessage.Load(inputPath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load email '{inputPath}': {loadEx.Message}");
                    return;
                }
            }
            else
            {
                // Create a simple email with a calendar appointment
                message = new MailMessage();
                message.From = new MailAddress("organizer@domain.com");
                message.To.Add(new MailAddress("attendee@domain.com"));
                message.Subject = "Meeting Invitation";
                message.Body = "Please find the meeting details attached.";

                // Attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee@domain.com"));

                // Create appointment (calendar invitation)
                Appointment appointment = new Appointment(
                    "Conference Room",
                    new DateTime(2024, 12, 15, 10, 0, 0),
                    new DateTime(2024, 12, 15, 11, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timelines.";

                // Add the calendar as an alternate view
                message.AddAlternateView(appointment.RequestApointment());

                // Optionally save the generated .eml for future runs
                try
                {
                    message.Save(inputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save placeholder email '{inputPath}': {saveEx.Message}");
                }
            }

            // Convert the email (including calendar) to PDF via MHTML bridge
            try
            {
                // Save email to a temporary MHTML file
                string tempMhtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mhtml");
                message.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);

                // Load the MHTML into Aspose.Words Document
                Document doc = new Document(tempMhtmlPath);

                // Save the document as PDF
                doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);

                // Clean up temporary file
                if (File.Exists(tempMhtmlPath))
                {
                    File.Delete(tempMhtmlPath);
                }

                Console.WriteLine($"Email successfully converted to PDF: {outputPath}");
            }
            catch (Exception pdfEx)
            {
                Console.Error.WriteLine($"Failed to convert email to PDF: {pdfEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
