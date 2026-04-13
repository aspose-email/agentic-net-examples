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

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Test Subject",
                        "Test Body"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and verify subject and body
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    string actualSubject = msg.Subject;
                    string actualBody = msg.Body;

                    bool subjectMatches = string.Equals(actualSubject, "Test Subject", StringComparison.Ordinal);
                    bool bodyMatches = string.Equals(actualBody, "Test Body", StringComparison.Ordinal);

                    if (subjectMatches && bodyMatches)
                    {
                        Console.WriteLine("Subject and body extraction passed.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Extraction failed.");
                        Console.Error.WriteLine($"Expected Subject: Test Subject, Actual: {actualSubject}");
                        Console.Error.WriteLine($"Expected Body: Test Body, Actual: {actualBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
