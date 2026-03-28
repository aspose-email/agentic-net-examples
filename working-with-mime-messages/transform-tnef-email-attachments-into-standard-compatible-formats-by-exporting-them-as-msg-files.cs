using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string tnefFilePath = "winmail.dat";
            string msgOutputPath = "output.msg";

            if (!File.Exists(tnefFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {tnefFilePath}");
                return;
            }

            using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tnefFilePath))
            {
                tnefMessage.Save(msgOutputPath);
                Console.WriteLine($"TNEF attachment exported to MSG: {msgOutputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
