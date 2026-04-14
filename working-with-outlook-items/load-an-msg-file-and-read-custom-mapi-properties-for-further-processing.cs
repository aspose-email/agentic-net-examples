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

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder",
                        "This is a placeholder MSG file."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderName}");
                Console.WriteLine($"Body: {msg.Body}");

                // Retrieve custom MAPI properties.
                MapiPropertyCollection customProps = msg.GetCustomProperties();

                if (customProps != null && customProps.Count > 0)
                {
                    Console.WriteLine("Custom MAPI Properties:");
                    // Iterate using the Values collection as required.
                    foreach (var prop in customProps.Values)
                    {
                        try
                        {
                            // Attempt to get the property value as a string; fallback to raw bytes if needed.
                            string value = prop.GetString();
                            if (string.IsNullOrEmpty(value))
                            {
                                // For non-string types, display raw data length.
                                byte[] data = prop.Data;
                                value = data != null ? $"[Binary Data, {data.Length} bytes]" : "[No Data]";
                            }
                            Console.WriteLine($"  Tag: 0x{prop.Tag:X}, Value: {value}");
                        }
                        catch (Exception propEx)
                        {
                            Console.Error.WriteLine($"  Error reading property 0x{prop.Tag:X}: {propEx.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No custom MAPI properties found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
