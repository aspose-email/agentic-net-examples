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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body.");
                placeholder.Save(msgPath);
                placeholder.Dispose();
            }

            // Load the MSG file, add a custom header, and save it back
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                message.Headers.Add("X-Custom-Header", "CustomValue");
                message.Save(msgPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
