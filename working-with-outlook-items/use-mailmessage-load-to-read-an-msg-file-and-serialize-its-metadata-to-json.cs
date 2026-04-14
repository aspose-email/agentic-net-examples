using System;
using System.IO;
using Aspose.Email;
using System.Text.Json;
using System.Collections.Generic;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgPath = "sample.msg";

                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file '{msgPath}' does not exist.");
                    return;
                }

                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    MessageMetadata metadata = new MessageMetadata();
                    metadata.Subject = mailMessage.Subject;
                    metadata.From = mailMessage.From?.Address;

                    metadata.To = new List<string>();
                    foreach (MailAddress address in mailMessage.To)
                    {
                        metadata.To.Add(address.Address);
                    }

                    metadata.Cc = new List<string>();
                    foreach (MailAddress address in mailMessage.CC)
                    {
                        metadata.Cc.Add(address.Address);
                    }

                    metadata.Bcc = new List<string>();
                    foreach (MailAddress address in mailMessage.Bcc)
                    {
                        metadata.Bcc.Add(address.Address);
                    }

                    metadata.Date = mailMessage.Date;
                    metadata.MessageId = mailMessage.MessageId;

                    string json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private class MessageMetadata
        {
            public string Subject { get; set; }
            public string From { get; set; }
            public List<string> To { get; set; }
            public List<string> Cc { get; set; }
            public List<string> Bcc { get; set; }
            public DateTime Date { get; set; }
            public string MessageId { get; set; }
        }
    }
}
