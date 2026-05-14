using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string taskMsgPath = "task.msg";
            string htmlOutputPath = "task.html";

            
            string outputDir = Path.GetDirectoryName(htmlOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
// Ensure the task MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(taskMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(taskMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiTask placeholderTask = new MapiTask())
                    {
                        placeholderTask.Subject = "Sample Task";
                        placeholderTask.DueDate = DateTime.Now.AddDays(7);
                        placeholderTask.Body = "This is a placeholder task generated for the example.";
                        placeholderTask.Save(taskMsgPath, TaskSaveFormat.Msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the task MSG as a MailMessage.
            MailMessage message;
            try
            {
                message = MailMessage.Load(taskMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load task message: {ex.Message}");
                return;
            }

            // Configure HTML save options to render task fields.
            HtmlSaveOptions htmlOptions = new HtmlSaveOptions
            {
                HtmlFormatOptions = HtmlFormatOptions.RenderTaskFields
            };

            // Save the task as HTML.
            try
            {
                message.Save(htmlOutputPath, htmlOptions);
                Console.WriteLine($"Task saved as HTML to '{htmlOutputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save HTML: {ex.Message}");
            }
            finally
            {
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
