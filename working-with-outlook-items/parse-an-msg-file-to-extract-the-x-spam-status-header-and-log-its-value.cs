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

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                const string headerName = "X-Spam-Status";
                bool found = false;

                foreach (string key in message.Headers.Keys)
                {
                    if (string.Equals(key, headerName, StringComparison.OrdinalIgnoreCase))
                    {
                        string value = message.Headers[key];
                        Console.WriteLine($"{headerName}: {value}");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"{headerName} header not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
