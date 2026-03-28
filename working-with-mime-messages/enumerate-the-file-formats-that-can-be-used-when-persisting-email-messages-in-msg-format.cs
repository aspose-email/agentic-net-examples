using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mapi.Msg;

namespace AsposeEmailMsgFormats
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // MailMessageSaveType provides several MSG related options
                MailMessageSaveType saveTypeStandard = MailMessageSaveType.OutlookMessageFormat;
                MailMessageSaveType saveTypeUnicode = MailMessageSaveType.OutlookMessageFormatUnicode;
                MailMessageSaveType saveTypeTemplate = MailMessageSaveType.OutlookTemplateFormat;

                // MAPI MessageObjectSaveFormat enumeration for MSG
                MessageObjectSaveFormat mapiSaveFormat = MessageObjectSaveFormat.Msg;

                // MessageFormat class also defines a MSG format
                MessageFormat messageFormat = MessageFormat.Msg;

                Console.WriteLine("Available MSG persistence formats in Aspose.Email:");
                Console.WriteLine($"- MailMessageSaveType.OutlookMessageFormat");
                Console.WriteLine($"- MailMessageSaveType.OutlookMessageFormatUnicode");
                Console.WriteLine($"- MailMessageSaveType.OutlookTemplateFormat");
                Console.WriteLine($"- MessageObjectSaveFormat.Msg");
                Console.WriteLine($"- MessageFormat.Msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
