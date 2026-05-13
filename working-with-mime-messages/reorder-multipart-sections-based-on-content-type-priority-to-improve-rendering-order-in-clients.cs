using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Create a mail message with subject and basic fields
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Message with Reordered Alternate Views";

                // Add plain text alternate view
                string plainText = "This is the plain text version of the email.";
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    plainText, new ContentType("text/plain"));
                message.AlternateViews.Add(plainView);

                // Add HTML alternate view
                string htmlText = "<html><body><h1>This is the HTML version of the email.</h1></body></html>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlText, new ContentType("text/html"));
                message.AlternateViews.Add(htmlView);

                // Add a calendar (iCalendar) alternate view as an example of lower priority content
                string calendarContent = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                AlternateView calendarView = AlternateView.CreateAlternateViewFromString(
                    calendarContent, new ContentType("text/calendar"));
                message.AlternateViews.Add(calendarView);

                // Reorder alternate views based on content type priority
                // Priority: text/plain > text/html > others
                List<AlternateView> reorderedViews = new List<AlternateView>();
                foreach (AlternateView view in message.AlternateViews)
                {
                    if (view.ContentType.MediaType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
                    {
                        reorderedViews.Insert(0, view);
                    }
                    else if (view.ContentType.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
                    {
                        int index = reorderedViews.FindIndex(v => v.ContentType.MediaType.Equals("text/plain", StringComparison.OrdinalIgnoreCase));
                        if (index >= 0)
                        {
                            reorderedViews.Insert(index + 1, view);
                        }
                        else
                        {
                            reorderedViews.Insert(0, view);
                        }
                    }
                    else
                    {
                        reorderedViews.Add(view);
                    }
                }

                // Clear existing alternate views and add reordered ones
                message.AlternateViews.Clear();
                foreach (AlternateView view in reorderedViews)
                {
                    message.AlternateViews.Add(view);
                }

                // Prepare output path
                string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
                string outputPath = Path.Combine(outputDirectory, "ReorderedMessage.msg");

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Save the message to file with MSG format
                try
                {
                    message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    Console.WriteLine($"Message saved successfully to: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
