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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    var placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body.");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Display basic message information
            Console.WriteLine("Subject: " + msg.Subject);
            Console.WriteLine("From: " + msg.SenderName);
            Console.WriteLine("Body: " + msg.Body);

            // Attempt to locate a custom property that holds the Notes UNID
            foreach (MapiProperty property in msg.Properties.Values)
            {
                if (string.Equals(property.Name, "NotesUNID", StringComparison.OrdinalIgnoreCase))
                {
                    string unid = property.GetValue()?.ToString() ?? string.Empty;
                    Console.WriteLine("Notes UNID: " + unid);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
