using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the input MSG file and the output MSG file
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputMsgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body"))
                {
                    placeholder.Save(inputMsgPath);
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Define the reaction (thumbs up) for a specific user
                string reactingUser = "user@example.com";
                string reactionValue = $"{reactingUser}: 👍";

                // Encode the reaction string as Unicode bytes
                byte[] reactionBytes = Encoding.Unicode.GetBytes(reactionValue);

                // Add a custom MAPI property to store the reaction
                // Property name can be any identifier; here we use "Reaction"
                message.AddCustomProperty(MapiPropertyType.PT_UNICODE, reactionBytes, "Reaction");

                // Save the modified message to a new file
                message.Save(outputMsgPath);
            }

            Console.WriteLine("Reaction added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
