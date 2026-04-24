using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string nsfPath = "sample.nsf";

            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                return;
            }

            using (NotesStorageFacility facility = new NotesStorageFacility(nsfPath))
            {
                await Task.Run(() =>
                {
                    foreach (MailMessage message in facility.EnumerateMessages())
                    {
                        using (MailMessage mail = message)
                        {
                            Console.WriteLine($"Subject: {mail.Subject}");
                        }
                    }
                });
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
