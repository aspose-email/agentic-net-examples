using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = args.Length > 0 ? args[0] : "sample.msg";
            string outputHtmlPath = args.Length > 1 ? args[1] : "output.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html><head><meta charset=\"utf-8\"/>");
                htmlBuilder.AppendLine($"<title>{WebUtility.HtmlEncode(msg.Subject ?? "No Subject")}</title>");
                htmlBuilder.AppendLine("</head><body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject ?? string.Empty)}</h1>");

                // From
                string from = msg.SenderName ?? msg.SenderEmailAddress ?? string.Empty;
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(from)}</p>");

                // To (recipients)
                if (msg.Recipients != null && msg.Recipients.Count > 0)
                {
                    List<string> toList = new List<string>();
                    foreach (MapiRecipient recipient in msg.Recipients)
                    {
                        string nameOrEmail = recipient.DisplayName ?? recipient.EmailAddress;
                        toList.Add(nameOrEmail);
                    }
                    string toJoined = string.Join(", ", toList);
                    htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(toJoined)}</p>");
                }

                // Body
                if (!string.IsNullOrEmpty(msg.BodyHtml))
                {
                    htmlBuilder.AppendLine(msg.BodyHtml);
                }
                else
                {
                    string bodyText = msg.Body ?? string.Empty;
                    htmlBuilder.AppendLine("<pre>");
                    htmlBuilder.AppendLine(WebUtility.HtmlEncode(bodyText));
                    htmlBuilder.AppendLine("</pre>");
                }

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                    string attachmentDir = Path.Combine(outputDirectory ?? "", "attachments");
                    if (!Directory.Exists(attachmentDir))
                    {
                        Directory.CreateDirectory(attachmentDir);
                    }

                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentPath = Path.Combine(attachmentDir, attachment.FileName);
                        attachment.Save(attachmentPath);
                        string relativePath = Path.Combine("attachments", attachment.FileName);
                        htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(relativePath)}\">{WebUtility.HtmlEncode(attachment.FileName)}</a></li>");
                    }

                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                Console.WriteLine($"HTML file saved to {outputHtmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}