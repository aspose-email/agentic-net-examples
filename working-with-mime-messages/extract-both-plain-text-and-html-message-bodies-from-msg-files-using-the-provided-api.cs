using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace ExtractMsgBodies
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
                    Console.Error.WriteLine($"Error: File not found – {msgPath}");
                    return;
                }

                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    string plainBody = message.Body;
                    string htmlBody = message.BodyHtml;

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
