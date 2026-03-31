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

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
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

            // Load the MSG file to be attached.
            MapiMessage attachedMessage;
            try
            {
                attachedMessage = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file '{inputMsgPath}': {ex.Message}");
                return;
            }

            // Create a new message that will hold the Notes attachment.
            using (MapiMessage targetMessage = new MapiMessage())
            {
                // Add the loaded MSG as an embedded message attachment.
                try
                {
                    targetMessage.Attachments.Add("AttachedMessage.msg", attachedMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Ensure the output directory exists.
                try
                {
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Save the message with the attached MSG.
                try
                {
                    targetMessage.Save(outputMsgPath);
                    Console.WriteLine($"Message saved with Notes attachment to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }

            // Dispose the attached message if it implements IDisposable.
            if (attachedMessage is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
