using System;
using Aspose.Email;

namespace AsposeEmailMsgFormats
{
    // Author: Aspose.Email example - enumerates MSG related save formats
    class Program
    {
        static void Main()
        {
            try
            {
                // Enumerate MSG related save formats using MailMessageSaveType static properties
                MailMessageSaveType outlookMsg = MailMessageSaveType.OutlookMessageFormat;
                MailMessageSaveType outlookMsgUnicode = MailMessageSaveType.OutlookMessageFormatUnicode;
                MailMessageSaveType outlookTemplate = MailMessageSaveType.OutlookTemplateFormat;
                MessageFormat msgFormat = MessageFormat.Msg;

                Console.WriteLine("MSG related save formats available in Aspose.Email:");
                Console.WriteLine($"- {nameof(MailMessageSaveType.OutlookMessageFormat)} : {outlookMsg}");
                Console.WriteLine($"- {nameof(MailMessageSaveType.OutlookMessageFormatUnicode)} : {outlookMsgUnicode}");
                Console.WriteLine($"- {nameof(MailMessageSaveType.OutlookTemplateFormat)} : {outlookTemplate}");
                Console.WriteLine($"- {nameof(MessageFormat.Msg)} : {msgFormat}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
