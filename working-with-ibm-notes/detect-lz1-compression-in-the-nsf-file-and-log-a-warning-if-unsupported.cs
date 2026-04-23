using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the NSF file
            string nsfPath = "sample.nsf";

            // Ensure the NSF file exists; create a minimal placeholder if missing
            if (!File.Exists(nsfPath))
            {
                try
                {
                    using (FileStream placeholderStream = File.Create(nsfPath))
                    {
                        // Write a minimal NSF header (placeholder content)
                        byte[] header = new byte[] { 0x4E, 0x53, 0x46, 0x00 }; // "NSF\0"
                        placeholderStream.Write(header, 0, header.Length);
                    }
                    Console.WriteLine("Placeholder NSF file created at: " + nsfPath);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine("Failed to create placeholder NSF file: " + ioEx.Message);
                    return;
                }
            }

            // Attempt to load the NSF file and enumerate its messages
            try
            {
                NsfLoadOptions loadOptions = new NsfLoadOptions();
                using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath, loadOptions))
                {
                    foreach (var messageInfo in nsf.EnumerateMessages())
                    {
                        // Accessing messageInfo forces internal parsing, which will raise an exception
                        // if LZ1 compression is encountered.
                        // No further processing needed for detection.
                    }
                    Console.WriteLine("NSF file loaded successfully without LZ1 compression.");
                }
            }
            catch (FormatNotSupportedException formatEx)
            {
                // Detect if the exception is related to LZ1 compression
                if (formatEx.Message != null && formatEx.Message.IndexOf("LZ1", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.Error.WriteLine("Warning: LZ1 compression in the NSF file is not supported.");
                }
                else
                {
                    Console.Error.WriteLine("Failed to load NSF file: " + formatEx.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error while processing NSF file: " + ex.Message);
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine("Unhandled exception: " + outerEx.Message);
        }
    }
}
