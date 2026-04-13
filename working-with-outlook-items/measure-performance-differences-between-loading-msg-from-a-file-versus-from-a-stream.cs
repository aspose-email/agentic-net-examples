using Aspose.Email;
using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Measure loading from file path
            Stopwatch fileStopwatch = new Stopwatch();
            try
            {
                fileStopwatch.Start();
                MapiMessage messageFromFile = MapiMessage.Load(msgPath);
                fileStopwatch.Stop();
                Console.WriteLine($"Loading from file took: {fileStopwatch.ElapsedMilliseconds} ms");
                // Use the message to avoid compiler warnings
                Console.WriteLine($"Subject (file): {messageFromFile.Subject}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG from file: {ex.Message}");
                return;
            }

            // Measure loading from stream
            Stopwatch streamStopwatch = new Stopwatch();
            try
            {
                using (FileStream stream = File.OpenRead(msgPath))
                {
                    streamStopwatch.Start();
                    MapiMessage messageFromStream = MapiMessage.Load(stream);
                    streamStopwatch.Stop();
                    Console.WriteLine($"Loading from stream took: {streamStopwatch.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Subject (stream): {messageFromStream.Subject}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG from stream: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
