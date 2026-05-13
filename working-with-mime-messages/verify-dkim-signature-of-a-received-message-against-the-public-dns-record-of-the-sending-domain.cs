using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "message.eml";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            try
            {
                bool isValid = MailMessage.CheckSignature(emlPath);
                Console.WriteLine(isValid
                    ? "DKIM signature is valid."
                    : "DKIM signature is invalid.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error verifying DKIM signature: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
