using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi.Msg;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            List<string> msgFormats = new List<string>();
            // MailMessageSaveType formats for MSG
            msgFormats.Add(nameof(MailMessageSaveType.OutlookMessageFormat));
            msgFormats.Add(nameof(MailMessageSaveType.OutlookMessageFormatUnicode));
            msgFormats.Add(nameof(MailMessageSaveType.OutlookTemplateFormat));
            // MessageFormat format for MSG
            msgFormats.Add(nameof(MessageFormat.Msg));
            // FileFormatType format for MSG
            msgFormats.Add(nameof(FileFormatType.Msg));
            // MessageObjectSaveFormat format for MSG
            msgFormats.Add(nameof(MessageObjectSaveFormat.Msg));

            Console.WriteLine("File formats that can be used when persisting email messages in MSG format:");
            foreach (string formatName in msgFormats)
            {
                Console.WriteLine("- " + formatName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
