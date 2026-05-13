using Aspose.Email.Mime;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;

namespace AsposeEmailMimeToJson
{
    class Program
    {
        static void Main()
        {
            try
            {
                string emlPath = "sample.eml";
                // Ensure the input file exists; create a minimal placeholder if missing
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
                        using (StreamWriter writer = new StreamWriter(emlPath))
                        {
                            writer.WriteLine("From: sender@example.com");
                            writer.WriteLine("To: recipient@example.com");
                            writer.WriteLine("Subject: Placeholder");
                            writer.WriteLine();
                            writer.WriteLine("This is a placeholder email body.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                        return;
                    }
                }

                // Load the MIME message
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    // Prepare a serializable representation
                    MessageLog log = new MessageLog();
                    log.From = mailMessage.From?.ToString();
                    log.To = ConvertAddressCollectionToList(mailMessage.To);
                    log.CC = ConvertAddressCollectionToList(mailMessage.CC);
                    log.Bcc = ConvertAddressCollectionToList(mailMessage.Bcc);
                    log.Subject = mailMessage.Subject;
                    log.Body = mailMessage.Body;
                    log.Headers = ConvertHeadersToDictionary(mailMessage.Headers);
                    log.Attachments = ConvertAttachmentsToList(mailMessage.Attachments);

                    // Serialize to JSON
                    string json = JsonSerializer.Serialize(log, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper to convert address collections to List<string>
        private static List<string> ConvertAddressCollectionToList(MailAddressCollection collection)
        {
            List<string> list = new List<string>();
            if (collection != null)
            {
                foreach (MailAddress address in collection)
                {
                    list.Add(address.ToString());
                }
            }
            return list;
        }

        // Helper to convert headers to Dictionary<string,string>
        private static Dictionary<string, string> ConvertHeadersToDictionary(HeaderCollection headers)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    dict[key] = headers[key];
                }
            }
            return dict;
        }

        // Helper to convert attachments to List<string> (file names)
        private static List<string> ConvertAttachmentsToList(AttachmentCollection attachments)
        {
            List<string> list = new List<string>();
            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    list.Add(attachment.Name);
                }
            }
            return list;
        }
    }

    // Serializable representation of a mail message
    public class MessageLog
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public List<string> Attachments { get; set; }
    }
}
