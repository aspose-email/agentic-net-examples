using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = args.Length > 0 ? args[0] : "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    string plainBody = msg.Body ?? string.Empty;
                    string htmlBody = msg.BodyHtml ?? string.Empty;

                    Console.WriteLine("Plain Text Body:");
                    Console.WriteLine(plainBody);
                    Console.WriteLine();

                    Console.WriteLine("HTML Body:");
                    Console.WriteLine(htmlBody);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
