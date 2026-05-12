using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string filePath = "sample.msg";

            if (!File.Exists(filePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            using (Aspose.Email.Mapi.MapiMessage msg = Aspose.Email.Mapi.MapiMessage.Load(filePath))
            {
                string subject = msg.Subject;
                Console.WriteLine("Subject: " + subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
