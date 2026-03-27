using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgCapabilities
{
    class Program
    {
        static void Main()
        {
            try
            {
                // List of MSG format capabilities provided by Aspose.Email
                List<string> capabilities = new List<string>
                {
                    "Load MSG from file path using MapiMessage.Load(string)",
                    "Load MSG from stream using MapiMessage.Load(System.IO.Stream)",
                    "Check if a stream or file is MSG format using MapiMessage.IsMsgFormat",
                    "Save MSG to file path using MapiMessage.Save(string)",
                    "Save MSG to stream using MapiMessage.Save(System.IO.Stream)",
                    "Create a new empty MSG using MapiMessage() constructor",
                    "Create MSG with specific OutlookMessageFormat using MapiMessage(OutlookMessageFormat)",
                    "Create MSG with sender, recipient, subject, body using MapiMessage(string, string, string, string)",
                    "Create MSG with OutlookMessageFormat using MapiMessage(string, string, string, string, OutlookMessageFormat)",
                    "Convert MailMessage to MSG using MapiMessage.FromMailMessage(MailMessage)",
                    "Convert MailMessage to MSG with conversion options using MapiMessage.FromMailMessage(MailMessage, MapiConversionOptions)",
                    "Convert EML stream to MSG using MapiMessage.FromMailMessage(System.IO.Stream)",
                    "Convert EML file to MSG using MapiMessage.FromMailMessage(string)",
                    "Access message properties such as Subject, Body, SenderName, SenderEmailAddress, Recipients, Attachments",
                    "Modify message properties like Subject, Body, SenderName, etc.",
                    "Add custom MAPI properties using AddCustomProperty",
                    "Retrieve custom MAPI properties using GetCustomProperties",
                    "Check if the message is encrypted using IsEncrypted",
                    "Check if the message is signed using IsSigned",
                    "Decrypt encrypted messages using Decrypt() or Decrypt(X509Certificate2)",
                    "Remove digital signature using RemoveSignature()",
                    "Check for bounce messages using CheckBounced()",
                    "Access and manipulate attachments via the Attachments collection",
                    "Save individual attachments to disk using attachment.Save(string)",
                    "Remove all attachments from a MSG file using MapiMessage.RemoveAttachments(string)",
                    "Destroy all attachments without parsing using MapiMessage.DestroyAttachments(string)",
                    "Convert MSG back to MailMessage using ToMailMessage(MailConversionOptions)",
                    "Convert MSG to generic MAPI item using ToMapiMessageItem()",
                    "Retrieve named properties via NamedProperties collection",
                    "Retrieve property stream via PropertyStream",
                    "Check message flags via Flags property",
                    "Set message flags using SetMessageFlags",
                    "Determine code page via CodePage property",
                    "Check if the message is a template (OFT) using IsTemplate",
                    "Access message class via MessageClass property",
                    "Read transport headers via TransportMessageHeaders",
                    "Read and write message headers via Headers property",
                    "Access categories, billing, companies, mileage, sensitivity, etc."
                };

                foreach (string capability in capabilities)
                {
                    Console.WriteLine("- " + capability);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
