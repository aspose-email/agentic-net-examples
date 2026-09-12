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
            // Path to the private key file (PEM format)
            string privateKeyPath = "key2.pem";

            // Verify that the private key file exists
            if (!File.Exists(privateKeyPath))
            {
                Console.Error.WriteLine($"Private key file not found: {privateKeyPath}");
                return;
            }

            // Load the RSA private key from the PEM file
            string pem = File.ReadAllText(privateKeyPath);
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(pem.ToCharArray());

            // DKIM parameters
            string selector = "selector";
            string domain = "example.com";

            // Create the email message to be signed
            using (MailMessage mailMessage = new MailMessage("user@example.com", "recipient@example.com"))
            {
                mailMessage.Subject = "Signed DKIM message";
                mailMessage.Body = "This is a DKIM signed email.";

                // Compute body hash (bh)
                byte[] bodyBytes = Encoding.UTF8.GetBytes(mailMessage.Body);
                using SHA256 sha256 = SHA256.Create();
                byte[] bodyHash = sha256.ComputeHash(bodyBytes);
                string bh = Convert.ToBase64String(bodyHash);

                // Prepare header data to be signed (simple canonicalization)
                StringBuilder headerBuilder = new StringBuilder();
                headerBuilder.AppendLine($"from:{mailMessage.From}");
                headerBuilder.AppendLine($"subject:{mailMessage.Subject}");

                byte[] headerBytes = Encoding.UTF8.GetBytes(headerBuilder.ToString());

                // Sign the header data
                byte[] signature = rsa.SignData(headerBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                string b = Convert.ToBase64String(signature);

                // Construct DKIM-Signature header
                string dkimHeaderValue = $"v=1; a=rsa-sha256; d={domain}; s={selector}; bh={bh}; b={b}";
                mailMessage.Headers.Add("DKIM-Signature", dkimHeaderValue);

                // Define output path for the signed message
                string outputPath = "signed.eml";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the signed message to a file
                mailMessage.Save(outputPath);
                Console.WriteLine($"DKIM signed message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
