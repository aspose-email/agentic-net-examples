using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgCapabilities
{
    class Program
    {
        static void Main(string[] args)
        {
        List<MailMessage> messages = new List<MailMessage>();

            try
            {
                List<string> capabilities = new List<string>();

                // Loading and saving MSG files
                capabilities.Add("Load MSG from file path using MapiMessage.Load(string)");
                capabilities.Add("Load MSG from stream using MapiMessage.Load(Stream)");
                capabilities.Add("Save MSG to file path using MapiMessage.Save(string)");
                capabilities.Add("Save MSG to stream using MapiMessage.Save(Stream)");

                // Conversion between MailMessage and MSG
                capabilities.Add("Create MSG from MailMessage using MapiMessage.FromMailMessage(MailMessage)");
                capabilities.Add("Create MSG from MailMessage with conversion options using MapiMessage.FromMailMessage(MailMessage, MapiConversionOptions)");
                capabilities.Add("Create MSG from EML stream using MapiMessage.FromMailMessage(Stream)");
                capabilities.Add("Create MSG from EML file using MapiMessage.FromMailMessage(string)");
                capabilities.Add("Convert MSG to MailMessage using MapiMessage.ToMailMessage(MailConversionOptions)");

                // Body handling
                capabilities.Add("Access plain text body via MapiMessage.Body");
                capabilities.Add("Access RTF body via MapiMessage.BodyRtf");
                capabilities.Add("Access HTML body via MapiMessage.BodyHtml");
                capabilities.Add("Set body content with SetBodyContent(string, BodyContentType)");
                capabilities.Add("Set RTF body with SetBodyRtf(string, bool)");

                // Attachments
                capabilities.Add("Enumerate attachments via MapiMessage.Attachments");
                capabilities.Add("Save individual attachment using MapiAttachment.Save(string)");
                capabilities.Add("Destroy all attachments in MSG file using MapiMessage.DestroyAttachments(string)");
                capabilities.Add("Remove all attachments from MSG file using MapiMessage.RemoveAttachments(string)");

                // Recipients and sender information
                capabilities.Add("Get sender name via MapiMessage.SenderName");
                capabilities.Add("Get sender email via MapiMessage.SenderEmailAddress");
                capabilities.Add("Get recipients collection via MapiMessage.Recipients");
                capabilities.Add("Get display names for To, Cc, Bcc via DisplayTo, DisplayCc, DisplayBcc");

                // Message metadata
                capabilities.Add("Access subject via MapiMessage.Subject");
                capabilities.Add("Access normalized subject via MapiMessage.NormalizedSubject");
                capabilities.Add("Access message class via MapiMessage.MessageClass");
                capabilities.Add("Access message flags via MapiMessage.Flags");
                capabilities.Add("Set message flags using SetMessageFlags(MapiMessageFlags)");
                capabilities.Add("Access categories via MapiMessage.Categories");
                capabilities.Add("Access billing information via MapiMessage.Billing");
                capabilities.Add("Access mileage via MapiMessage.Mileage");
                capabilities.Add("Access sensitivity via MapiMessage.Sensitivity");
                capabilities.Add("Access read receipt request via MapiMessage.ReadReceiptRequested");
                capabilities.Add("Access delivery and client submit times via DeliveryTime and ClientSubmitTime");

                // Custom properties
                capabilities.Add("Add custom property using AddCustomProperty(MapiProperty, string)");
                capabilities.Add("Add custom property using AddCustomProperty(MapiPropertyType, byte[], string)");
                capabilities.Add("Retrieve custom properties via GetCustomProperties()");
                capabilities.Add("Get, set, and remove arbitrary MAPI properties using GetProperty*, SetProperty, and RemoveProperty");

                // Encryption and signing
                capabilities.Add("Check if message is encrypted via IsEncrypted");
                capabilities.Add("Decrypt message using Decrypt()");
                capabilities.Add("Decrypt message with certificate using Decrypt(X509Certificate2)");
                capabilities.Add("Check if message is signed via IsSigned");
                capabilities.Add("Remove signature using RemoveSignature()");
                capabilities.Add("Attach signature using AttachSignature methods");

                // TNEF handling
                capabilities.Add("Load MSG from TNEF stream using LoadFromTnef(Stream)");
                capabilities.Add("Load MSG from TNEF file using LoadFromTnef(string)");
                capabilities.Add("Save MSG as TNEF using SaveAsTnef(Stream) and SaveAsTnef(string)");

                // Miscellaneous utilities
                capabilities.Add("Determine if a file/stream is MSG format using IsMsgFormat(string) and IsMsgFormat(Stream)");
                capabilities.Add("Clone a message using Clone()");
                capabilities.Add("Check for bounce messages using CheckBounced()");
                capabilities.Add("Check digital signature using CheckSignature()");
                capabilities.Add("Access named properties via NamedProperties and NamedPropertyMapping");
                capabilities.Add("Access transport headers via Headers and TransportMessageHeaders");
                capabilities.Add("Access code page via CodePage");
                capabilities.Add("Access item ID via ItemId (used with server operations)");
                capabilities.Add("Access sub storages via SubStorages");
                capabilities.Add("Access supported item type via SupportedType");

                // Output the capabilities
                Console.WriteLine("Supported MSG format capabilities in Aspose.Email:");
                foreach (string capability in capabilities)
                {
                    Console.WriteLine("- " + capability);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}