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
            string msgPath = "message.msg";
            string directory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (MapiMessage message = new MapiMessage("from@example.com", "to@example.com", "Sample Subject", "Sample body"))
            {
                // Save the initial message without voting buttons
                try
                {
                    message.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save initial message: {ex.Message}");
                    return;
                }

                long sizeBefore = 0;
                try
                {
                    if (File.Exists(msgPath))
                    {
                        sizeBefore = new FileInfo(msgPath).Length;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get file size before adding voting button: {ex.Message}");
                    return;
                }

                // Add a voting button
                try
                {
                    FollowUpManager.AddVotingButton(message, "Approve");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add voting button: {ex.Message}");
                    return;
                }

                // Save the message after adding the voting button
                try
                {
                    message.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message after adding voting button: {ex.Message}");
                    return;
                }

                long sizeAfter = 0;
                try
                {
                    if (File.Exists(msgPath))
                    {
                        sizeAfter = new FileInfo(msgPath).Length;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get file size after adding voting button: {ex.Message}");
                    return;
                }

                Console.WriteLine($"File size before adding voting button: {sizeBefore} bytes");
                Console.WriteLine($"File size after adding voting button: {sizeAfter} bytes");
                if (sizeAfter > sizeBefore)
                {
                    Console.WriteLine("Voting button added successfully, file size increased.");
                }
                else
                {
                    Console.WriteLine("File size did not increase; voting button may not have been added.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
