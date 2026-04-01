using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists
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
                // Create a minimal placeholder MSG to avoid failure
                using (MapiMessage placeholder = new MapiMessage())
                {
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder MSG.";
                    placeholder.Save(msgPath);
                }
                Console.WriteLine($"Created placeholder MSG at {msgPath}");
                return;
            }

            // Verify the file is an MSG format
            if (!MapiMessage.IsMsgFormat(msgPath))
            {
                Console.Error.WriteLine("The specified file is not a valid MSG format.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderName}");

                // Iterate all standard properties
                Console.WriteLine("Standard Properties:");
                foreach (MapiProperty prop in msg.Properties.Values)
                {
                    Console.WriteLine($"Tag 0x{prop.Tag:X}: {BitConverter.ToString(prop.Data)}");
                }

                // Iterate named properties
                Console.WriteLine("Named Properties:");
                foreach (KeyValuePair<long, MapiProperty> entry in msg.NamedProperties)
                {
                    long tag = entry.Key;
                    MapiProperty namedProp = entry.Value;
                    Console.WriteLine($"Tag 0x{tag:X}: {BitConverter.ToString(namedProp.Data)}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
