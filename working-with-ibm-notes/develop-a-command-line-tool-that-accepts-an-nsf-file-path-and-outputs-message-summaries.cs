using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: program <nsfFilePath>");
                return;
            }

            string nsfPath = args[0];

            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"File not found: {nsfPath}");
                return;
            }

            // Open the NSF file
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
            {
                try
                {
                    IEnumerable<MailMessage> messages = nsf.EnumerateMessages();

                    foreach (MailMessage message in messages)
                    {
                        string subject = message.Subject ?? "(no subject)";
                        string from = message.From != null ? message.From.ToString() : "(no sender)";
                        DateTime? date = message.Date;
                        string dateStr = date.HasValue ? date.Value.ToString("u") : "(no date)";

                        Console.WriteLine($"Subject: {subject}");
                        Console.WriteLine($"From: {from}");
                        Console.WriteLine($"Date: {dateStr}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
