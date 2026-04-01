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
            // Input and output MSG file paths
            string inputMsgPath = "sample.msg";
            string outputMsgPath = "sample_seen.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file, set the Seen (Read) flag, and save the result
            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                {
                    // Set the message as read (Seen)
                    msg.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);

                    // Save the modified message
                    msg.Save(outputMsgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
