using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgLogging
{
    class SimpleLogger
    {
        public void LogInformation(string message, params object[] args)
        {
            Console.WriteLine("[Info] " + string.Format(message, args));
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            Console.WriteLine("[Error] " + string.Format(message, args));
            Console.WriteLine("Exception: " + ex);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var logger = new SimpleLogger();

            // Define the path to the MSG file
            string msgFilePath = "sample.msg";

            // Ensure the directory for the MSG file exists
            try
            {
                string directory = Path.GetDirectoryName(msgFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                logger.LogError(dirEx, "Failed to ensure output directory exists for path: {0}", msgFilePath);
                return;
            }

            // Verify that the file exists; if not, create a placeholder MSG
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

                    logger.LogInformation("Created placeholder MSG file at {0}", msgFilePath);
                }
                catch (Exception placeholderEx)
                {
                    logger.LogError(placeholderEx, "Error creating placeholder MSG file at {0}", msgFilePath);
                    return;
                }
            }

            // Load the MSG file
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    logger.LogInformation("Successfully loaded MSG file: {0}", msgFilePath);
                    logger.LogInformation("Subject: {0}", msg.Subject);
                    logger.LogInformation("From: {0}", msg.SenderEmailAddress);
                    logger.LogInformation("Body length: {0}", msg.Body?.Length ?? 0);

                    // Log attachment details
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        logger.LogInformation("Attachment found: {0}", attachment.FileName);
                    }
                }
            }
            catch (Exception loadEx)
            {
                logger.LogError(loadEx, "Failed to load MSG file: {0}", msgFilePath);
            }
        }
    }
}
