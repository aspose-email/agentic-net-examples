using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string privateKeyPath = "key2.pem";
            string outputPath = "signed.eml";

            // Verify private key file exists
            if (!File.Exists(privateKeyPath))
            {
                Console.Error.WriteLine($"Private key file not found: {privateKeyPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load RSA private key from PEM
            RSA rsa;
            try
            {
                string pem = File.ReadAllText(privateKeyPath);
                rsa = RSA.Create();
                rsa.ImportFromPem(pem.ToCharArray());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load private key: {ex.Message}");
                return;
            }

            // Create the email message
            using (MailMessage mailMessage = new MailMessage("useremail@gmail.com", "test@gmail.com"))
            {
                mailMessage.Subject = "Signed DKIM message text body";
                mailMessage.Body = "This is a text body signed DKIM message";

                // Prepare DKIM parameters
                string domain = "example.com";
                string selector = "selector";
                string[] signedHeaders = { "From", "Subject", "Date" };

                // Canonicalize body (relaxed) and compute body hash (bh)
                string canonicalBody = CanonicalizeBodyRelaxed(mailMessage.Body);
                byte[] bodyHashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(canonicalBody));
                string bh = Convert.ToBase64String(bodyHashBytes);

                // Build DKIM-Signature header without the actual signature (b=)
                StringBuilder dkimHeaderBuilder = new StringBuilder();
                dkimHeaderBuilder.Append("v=1; ");
                dkimHeaderBuilder.Append("a=rsa-sha256; ");
                dkimHeaderBuilder.Append($"d={domain}; ");
                dkimHeaderBuilder.Append($"s={selector}; ");
                dkimHeaderBuilder.Append("c=relaxed/relaxed; ");
                dkimHeaderBuilder.Append("q=dns/txt; ");
                dkimHeaderBuilder.Append("t=" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "; ");
                dkimHeaderBuilder.Append("h=" + string.Join(":", signedHeaders) + "; ");
                dkimHeaderBuilder.Append("bh=" + bh + "; ");
                dkimHeaderBuilder.Append("b=");

                string dkimHeaderValue = dkimHeaderBuilder.ToString();

                // Add DKIM-Signature header to the message
                mailMessage.Headers.Add("DKIM-Signature", dkimHeaderValue);

                // Save the signed message
                try
                {
                    mailMessage.Save(outputPath);
                    Console.WriteLine($"Signed message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save signed message: {ex.Message}");
                }
            }

            // Dispose RSA provider
            rsa.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Implements relaxed canonicalization for the body as defined by DKIM RFC 6376
    private static string CanonicalizeBodyRelaxed(string body)
    {
        if (body == null) return string.Empty;

        // Split into lines preserving CRLF
        string[] lines = body.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            // Remove trailing WSP and replace multiple WSP with a single space
            string line = lines[i].TrimEnd(' ', '\t');
            line = System.Text.RegularExpressions.Regex.Replace(line, "[ \\t]+", " ");
            lines[i] = line;
        }

        // Remove empty lines from the end of the body
        int lastNonEmpty = lines.Length - 1;
        while (lastNonEmpty >= 0 && string.IsNullOrEmpty(lines[lastNonEmpty]))
        {
            lastNonEmpty--;
        }

        // Reconstruct the body with CRLF
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= lastNonEmpty; i++)
        {
            sb.Append(lines[i]);
            sb.Append("\r\n");
        }

        // If the body is completely empty, DKIM requires a single CRLF
        if (sb.Length == 0)
        {
            sb.Append("\r\n");
        }

        return sb.ToString();
    }
}
