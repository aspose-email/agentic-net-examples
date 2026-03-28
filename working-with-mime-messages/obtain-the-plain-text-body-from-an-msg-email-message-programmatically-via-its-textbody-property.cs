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
            string msgFilePath = "message.msg";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                string plainTextBody = msg.Body;
                Console.WriteLine("Plain text body:");
                Console.WriteLine(plainTextBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
