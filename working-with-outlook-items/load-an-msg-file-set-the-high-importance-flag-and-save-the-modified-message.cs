using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output MSG file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";

            // Verify that the input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file, set high importance, and save the modified message
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Set the standard Importance property to High (value 2)
                msg.SetProperty(KnownPropertyList.Importance, (int)MapiImportance.High);

                // Save the modified message
                msg.Save(outputMsgPath);
            }

            Console.WriteLine($"Message saved with high importance to: {outputMsgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
