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
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string replacementImagePath = "newImage.jpg";

            // Verify input MSG file exists; create placeholder if missing
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

                Console.Error.WriteLine($"Input file '{inputMsgPath}' not found.");
                return;
            }

            // Load the MSG message
            MapiMessage msg = MapiMessage.Load(inputMsgPath);

            // Analyze embedded images
            Console.WriteLine("Embedded images found in the message:");
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                string ext = Path.GetExtension(attachment.FileName)?.ToLowerInvariant();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    Console.WriteLine($"- {attachment.FileName}");
                }
            }

            // Replace the first JPEG embedded image with a new one (if both exist)
            if (File.Exists(replacementImagePath))
            {
                byte[] newImageData = File.ReadAllBytes(replacementImagePath);
                for (int i = 0; i < msg.Attachments.Count; i++)
                {
                    MapiAttachment att = msg.Attachments[i];
                    string ext = Path.GetExtension(att.FileName)?.ToLowerInvariant();
                    if (ext == ".jpg" || ext == ".jpeg")
                    {
                        // Replace the binary data of the existing attachment
                        att.BinaryData = newImageData;
                        Console.WriteLine($"Replaced image '{att.FileName}' with '{replacementImagePath}'.");
                        break; // Replace only the first matching image
                    }
                }
            }
            else
            {
                Console.Error.WriteLine($"Replacement image '{replacementImagePath}' not found. Skipping replacement.");
            }

            // Save the modified MSG message
            msg.Save(outputMsgPath);
            Console.WriteLine($"Modified message saved to '{outputMsgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
