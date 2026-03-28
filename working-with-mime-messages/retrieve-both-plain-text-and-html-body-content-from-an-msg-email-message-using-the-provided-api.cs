using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace RetrieveMsgBody
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
                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    string plainBody = msg.Body;
                    string htmlBody = msg.BodyHtml;

                    Console.WriteLine("Plain Text Body:");
                    Console.WriteLine(plainBody);
                    Console.WriteLine();

                    Console.WriteLine("HTML Body:");
                    Console.WriteLine(htmlBody);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
