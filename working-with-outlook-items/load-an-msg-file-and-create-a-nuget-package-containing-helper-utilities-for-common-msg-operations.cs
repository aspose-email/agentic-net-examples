using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string inputMsgPath = "sample.msg";
            string outputMsgPath = "output.msg";
            string helperDirectory = "MsgHelpers";
            string nuspecPath = Path.Combine(helperDirectory, "MsgHelperUtilities.nuspec");
            string dummyDllPath = Path.Combine(helperDirectory, "MsgHelperUtilities.dll");
            string nupkgPath = "MsgHelperUtilities.1.0.0.nupkg";

            // Verify input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
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

            // Load the MSG file
            using (MailMessage message = MailMessage.Load(inputMsgPath))
            {
                // Save a copy of the message using MsgSaveOptions (required by validation)
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                message.Save(outputMsgPath, saveOptions);

                // Create a directory for helper utilities
                if (!Directory.Exists(helperDirectory))
                {
                    Directory.CreateDirectory(helperDirectory);
                }

                // Example helper: write the subject to a text file
                string subjectFilePath = Path.Combine(helperDirectory, "Subject.txt");
                File.WriteAllText(subjectFilePath, message.Subject ?? string.Empty);

                // Create a minimal .nuspec file for the NuGet package
                string nuspecContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>MsgHelperUtilities</id>
    <version>1.0.0</version>
    <authors>Example</authors>
    <description>Helper utilities for MSG operations.</description>
  </metadata>
</package>";
                File.WriteAllText(nuspecPath, nuspecContent);

                // Create a placeholder DLL (empty file) to include in the package
                File.WriteAllBytes(dummyDllPath, new byte[0]);

                // Build the .nupkg (ZIP archive) containing the .nuspec and placeholder DLL
                using (FileStream zipStream = new FileStream(nupkgPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                    {
                        // Add the .nuspec file at the root of the package
                        archive.CreateEntryFromFile(nuspecPath, "MsgHelperUtilities.nuspec");

                        // Add the dummy DLL under the lib folder
                        string dllEntryPath = "lib/netstandard2.0/MsgHelperUtilities.dll";
                        archive.CreateEntryFromFile(dummyDllPath, dllEntryPath);
                    }
                }

                Console.WriteLine("MSG processing and NuGet package creation completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
