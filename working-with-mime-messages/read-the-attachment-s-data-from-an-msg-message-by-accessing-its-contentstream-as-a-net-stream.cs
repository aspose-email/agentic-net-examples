using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = args.Length > 0 ? args[0] : "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    if (attachment.ContentStream == null)
                    {
                        Console.WriteLine($"Attachment \"{attachment.Name}\" has no content stream.");
                        continue;
                    }

                    using (Stream contentStream = attachment.ContentStream)
                    using (MemoryStream memory = new MemoryStream())
                    {
                        contentStream.CopyTo(memory);
                        byte[] data = memory.ToArray();
                        Console.WriteLine($"Attachment \"{attachment.Name}\" size: {data.Length} bytes");
                        // Example: process data here
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
