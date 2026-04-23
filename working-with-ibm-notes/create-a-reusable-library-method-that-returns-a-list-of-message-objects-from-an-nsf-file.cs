using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            string nsfPath = "sample.nsf";
            List<MailMessage> messages = NsfHelper.GetMessagesFromNsf(nsfPath);
            Console.WriteLine($"Retrieved {messages.Count} messages from NSF.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}

static class NsfHelper
{
    public static List<MailMessage> GetMessagesFromNsf(string nsfPath)
    {
        List<MailMessage> result = new List<MailMessage>();

        if (!File.Exists(nsfPath))
        {
            Console.Error.WriteLine($"NSF file not found: {nsfPath}");
            return result;
        }

        try
        {
            using (NotesStorageFacility notesStorage = new NotesStorageFacility(nsfPath))
            {
                foreach (MailMessage message in notesStorage.EnumerateMessages())
                {
                    result.Add(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading NSF file: {ex.Message}");
        }

        return result;
    }
}
