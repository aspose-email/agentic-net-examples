using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // Empty placeholder
                    }
                    Console.WriteLine($"Placeholder MBOX file created at {mboxPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Conversion completed; PersonalStorage will be disposed automatically
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            // Verify PST file exists before computing checksum
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found at {pstPath}");
                return;
            }

            // Compute SHA256 checksum of the PST file
            try
            {
                using (FileStream pstStream = File.OpenRead(pstPath))
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(pstStream);
                    StringBuilder sb = new StringBuilder(hashBytes.Length * 2);
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    Console.WriteLine($"SHA256 checksum of PST: {sb}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Checksum calculation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
