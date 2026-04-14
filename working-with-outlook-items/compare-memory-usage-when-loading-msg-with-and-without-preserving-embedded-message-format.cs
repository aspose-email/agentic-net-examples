using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholderMessage = new MailMessage())
                    {
                        placeholderMessage.From = new MailAddress("from@example.com");
                        placeholderMessage.To.Add(new MailAddress("to@example.com"));
                        placeholderMessage.Subject = "Placeholder";
                        placeholderMessage.Body = "This is a placeholder message.";

                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        placeholderMessage.Save(msgPath, saveOptions);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            Process currentProcess = Process.GetCurrentProcess();

            // Load without preserving embedded message format
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryBeforeNoPreserve = currentProcess.PrivateMemorySize64;

            try
            {
                MsgLoadOptions loadOptionsNoPreserve = new MsgLoadOptions();
                loadOptionsNoPreserve.PreserveEmbeddedMessageFormat = false;

                using (MailMessage messageNoPreserve = MailMessage.Load(msgPath, loadOptionsNoPreserve))
                {
                    // No additional processing required
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error loading MSG without preserving format: " + ex.Message);
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfterNoPreserve = currentProcess.PrivateMemorySize64;
            long memoryUsageNoPreserve = memoryAfterNoPreserve - memoryBeforeNoPreserve;

            // Load with preserving embedded message format
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryBeforePreserve = currentProcess.PrivateMemorySize64;

            try
            {
                MsgLoadOptions loadOptionsPreserve = new MsgLoadOptions();
                loadOptionsPreserve.PreserveEmbeddedMessageFormat = true;

                using (MailMessage messagePreserve = MailMessage.Load(msgPath, loadOptionsPreserve))
                {
                    // No additional processing required
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error loading MSG with preserving format: " + ex.Message);
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfterPreserve = currentProcess.PrivateMemorySize64;
            long memoryUsagePreserve = memoryAfterPreserve - memoryBeforePreserve;

            Console.WriteLine("Memory usage without preserving embedded format: {0} bytes", memoryUsageNoPreserve);
            Console.WriteLine("Memory usage with preserving embedded format: {0} bytes", memoryUsagePreserve);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
