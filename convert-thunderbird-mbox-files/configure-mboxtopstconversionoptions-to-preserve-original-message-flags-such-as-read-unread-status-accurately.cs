using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                using (FileStream placeholder = File.Create(mboxFilePath))
                {
                    byte[] header = System.Text.Encoding.UTF8.GetBytes("From - \r\n");
                    placeholder.Write(header, 0, header.Length);
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure conversion options to preserve message flags
            MboxToPstConversionOptions options = new MboxToPstConversionOptions
            {
                RemoveSignature = false,
                // The delegate takes a single MailMessage argument in current API
                MessageHandler = (msg) =>
                {
                    // No modifications are required; original flags (read/unread) are preserved.
                }
            };

            // Perform the conversion
            MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, options);
            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
