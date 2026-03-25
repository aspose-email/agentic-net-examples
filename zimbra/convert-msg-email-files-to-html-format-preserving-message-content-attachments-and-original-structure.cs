using System;
using System.IO;
using System.Text;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <input.msg> <output.html>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            using (Aspose.Email.Mapi.MapiMessage mapiMessage = Aspose.Email.Mapi.MapiMessage.Load(inputPath))
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><head><meta charset=\"utf-8\"/></head><body>");

                // Subject
                htmlBuilder.AppendLine($"<h2>{WebUtility.HtmlEncode(mapiMessage.Subject)}</h2>");

                // Sender
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(mapiMessage.SenderName)}</p>");

                // Recipients
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(mapiMessage.DisplayTo)}</p>");

                // Body (HTML if available, otherwise plain text)
                if (!string.IsNullOrEmpty(mapiMessage.BodyHtml))
                {
                    htmlBuilder.AppendLine(mapiMessage.BodyHtml);
                }
                else
                {
                    htmlBuilder.AppendLine($"<pre>{WebUtility.HtmlEncode(mapiMessage.Body)}</pre>");
                }

                // Attachments
                if (mapiMessage.Attachments != null && mapiMessage.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments</h3><ul>");
                    foreach (Aspose.Email.Mapi.MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        string attachmentFileName = Path.GetFileName(attachment.FileName);
                        string attachmentPath = Path.Combine(outputDirectory, attachmentFileName);

                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Warning: Unable to save attachment '{attachmentFileName}'. {attEx.Message}");
                            continue;
                        }

                        string encodedFileName = WebUtility.UrlEncode(attachmentFileName);
                        htmlBuilder.AppendLine($"<li><a href=\"{encodedFileName}\">{WebUtility.HtmlEncode(attachmentFileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                try
                {
                    File.WriteAllText(outputPath, htmlBuilder.ToString(), Encoding.UTF8);
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error: Unable to write HTML file – {outputPath}. {writeEx.Message}");
                    return;
                }

                Console.WriteLine("Conversion completed: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}