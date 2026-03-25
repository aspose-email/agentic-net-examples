using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace EmailArchiveReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.Error.WriteLine("Usage: EmailArchiveReader <path_to_zimbra_tgz>");
                    return;
                }

                string tgzPath = args[0];

                if (!File.Exists(tgzPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                    return;
                }

                using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
                {
                    while (tgzReader.ReadNextMessage())
                    {
                        using (Aspose.Email.MailMessage mailMessage = tgzReader.CurrentMessage)
                        {
                            string subject = mailMessage.Subject ?? "<no subject>";
                            string from = mailMessage.From != null ? mailMessage.From.Address : "<unknown sender>";
                            Console.WriteLine($"Subject: {subject}");
                            Console.WriteLine($"From: {from}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}