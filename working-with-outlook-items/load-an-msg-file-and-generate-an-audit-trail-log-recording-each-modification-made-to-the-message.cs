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
            string msgPath = "input.msg";
            string logPath = "audit.log";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder message."))
                {
                    placeholder.Save(msgPath);
                }
                Console.Error.WriteLine($"Input MSG not found. Created placeholder at {msgPath}.");
            }

            // Ensure the directory for the audit log exists.
            string logDirectory = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Open the audit log for writing.
            using (StreamWriter logWriter = new StreamWriter(logPath, false))
            {
                // Load the MSG file.
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    logWriter.WriteLine($"Loaded message: Subject=\"{message.Subject}\", From=\"{message.SenderEmailAddress}\"");

                    // Record original subject.
                    string originalSubject = message.Subject;

                    // Modify the subject.
                    message.Subject = "Updated Subject";
                    logWriter.WriteLine($"Modified Subject: \"{originalSubject}\" -> \"{message.Subject}\"");

                    // Add a custom MAPI property to indicate a change.
                    string propertyName = "AuditProperty";
                    string propertyValue = "Modified";
                    byte[] propertyBytes = System.Text.Encoding.Unicode.GetBytes(propertyValue);
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyBytes, propertyName);
                    logWriter.WriteLine($"Added custom property: {propertyName} = \"{propertyValue}\"");

                    // Save the modified message back to the same file.
                    message.Save(msgPath);
                    logWriter.WriteLine($"Saved modified message back to {msgPath}");
                }
            }

            Console.WriteLine("Audit trail generated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
