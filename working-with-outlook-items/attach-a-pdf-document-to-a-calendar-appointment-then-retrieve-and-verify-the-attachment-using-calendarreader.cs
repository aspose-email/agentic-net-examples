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
            // Paths for the PDF and the generated iCalendar file
            string pdfPath = "sample.pdf";
            string icsPath = "appointment.ics";

            // Ensure the PDF file exists; create a minimal placeholder if missing
            if (!File.Exists(pdfPath))
            {
                try
                {
                    // Simple PDF header to make a valid (though empty) PDF file
                    byte[] pdfBytes = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4\n%âãÏÓ\n1 0 obj\n<<>>\nendobj\ntrailer\n<<>>\n%%EOF");
                    File.WriteAllBytes(pdfPath, pdfBytes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PDF: {ex.Message}");
                    return;
                }
            }

            // Create a calendar appointment
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Project Meeting",
                new DateTime(2023, 1, 1, 10, 0, 0),
                new DateTime(2023, 1, 1, 11, 0, 0),
                organizer,
                attendees);

            appointment.Summary = "Project Kick‑off";
            appointment.Description = "Discuss project goals and timeline.";

            // Attach the PDF document
            using (Attachment pdfAttachment = new Attachment(pdfPath))
            {
                appointment.Attachments.Add(pdfAttachment);
            }

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
                return;
            }

            // Verify the attachment by loading the appointment back
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine("The generated iCalendar file was not found.");
                return;
            }

            Appointment loadedAppointment;
            try
            {
                loadedAppointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Check for attachments
            if (loadedAppointment.Attachments.Count > 0)
            {
                Console.WriteLine("Attachments found in the loaded appointment:");
                foreach (Attachment att in loadedAppointment.Attachments)
                {
                    Console.WriteLine($"- Name: {att.Name}, Size: {att.ContentStream.Length} bytes");
                }
            }
            else
            {
                Console.WriteLine("No attachments were found in the loaded appointment.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
