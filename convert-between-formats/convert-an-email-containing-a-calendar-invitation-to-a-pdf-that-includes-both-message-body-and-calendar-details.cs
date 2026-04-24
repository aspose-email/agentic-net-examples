using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "invitation.eml";
            string outputPdfPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            using (MailMessage email = MailMessage.Load(inputPath))
            {
                Appointment calendar = null;

                foreach (AlternateView view in email.AlternateViews)
                {
                    if (view.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MemoryStream calStream = new MemoryStream())
                        {
                            view.ContentStream.CopyTo(calStream);
                            calStream.Position = 0;
                            calendar = Appointment.Load(calStream);
                        }
                        break;
                    }
                }

                if (calendar != null)
                {
                    string calendarHtml = calendar.GetAppointmentHtml();
                    email.Body += "\n\n" + calendarHtml;
                    email.IsBodyHtml = true;
                }

                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    Document doc = new Document(mhtmlStream);
            {
                        doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                    }
                }
            }

            Console.WriteLine($"PDF saved to '{outputPdfPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
