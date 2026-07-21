using System;
using System.IO;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Mapi;


namespace ValidateMsgSignature
{
    class Program
    {
        static void Main()
        {
            // Path to the MSG file to be validated
            string msgFilePath = "signed_message.msg";

            // Verify that the input file exists before proceeding
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            try
            {
                // Load the MSG file as a MapiMessage
                MapiMessage mapiMessage = MapiMessage.Load(msgFilePath);

                // Convert the MapiMessage to a MailMessage for signature checking
                MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                // Ensure the MailMessage is disposed after use
                using (mailMessage)
                {
                    // Use reflection to create SecureEmailManager and invoke CheckSignature
                    // This avoids compile‑time dependency on the Aspose.Email.Security.Cryptography.Pkcs namespace
                    Type secureManagerType = Type.GetType("Aspose.Email.Security.Cryptography.Pkcs.SecureEmailManager, Aspose.Email");
                    if (secureManagerType == null)
                    {
                        Console.Error.WriteLine("SecureEmailManager type not found. Signature validation cannot be performed.");
                        return;
                    }

                    object secureManager = Activator.CreateInstance(secureManagerType);
                    MethodInfo checkSignatureMethod = secureManagerType.GetMethod("CheckSignature", new[] { typeof(MailMessage) });
                    if (checkSignatureMethod == null)
                    {
                        Console.Error.WriteLine("CheckSignature method not found on SecureEmailManager.");
                        return;
                    }

                    object signatureResult = checkSignatureMethod.Invoke(secureManager, new object[] { mailMessage });

                    Console.WriteLine($"Signature validation result: {signatureResult}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
