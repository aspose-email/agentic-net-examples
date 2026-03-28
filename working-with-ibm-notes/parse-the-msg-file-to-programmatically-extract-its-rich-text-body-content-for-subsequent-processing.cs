using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "sample.msg";

                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                    return;
                }

                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    string richTextBody = msg.BodyRtf ?? string.Empty;
                    Console.WriteLine("Rich‑text body content:");
                    Console.WriteLine(richTextBody);
                    // TODO: Add further processing of richTextBody here.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
