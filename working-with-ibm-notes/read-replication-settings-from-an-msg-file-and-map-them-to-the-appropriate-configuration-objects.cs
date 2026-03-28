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
            // Path to the MSG file containing replication settings
            string msgPath = "replication_settings.msg";

            // Verify the file exists before attempting to load
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file inside a using block to ensure disposal
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Create a configuration object to hold the replication settings
                ReplicationConfig config = new ReplicationConfig();

                // Retrieve replication message size (numeric value)
                object sizeObj = msg.GetProperty(KnownPropertyList.ReplicationMsgSize);
                if (sizeObj is long size)
                {
                    config.ReplicationMessageSize = size;
                }

                // Retrieve replication message priority (numeric value)
                object priorityObj = msg.GetProperty(KnownPropertyList.ReplicationMessagePriority);
                if (priorityObj is long priority)
                {
                    config.ReplicationMessagePriority = priority;
                }

                // Retrieve replication style (string value)
                object styleObj = msg.GetProperty(KnownPropertyList.ReplicationStyle);
                if (styleObj != null)
                {
                    config.ReplicationStyle = styleObj.ToString();
                }

                // Retrieve replication schedule (string value)
                object scheduleObj = msg.GetProperty(KnownPropertyList.ReplicationSchedule);
                if (scheduleObj != null)
                {
                    config.ReplicationSchedule = scheduleObj.ToString();
                }

                // Retrieve receive folder settings (string value)
                object receiveFolderObj = msg.GetProperty(KnownPropertyList.ReceiveFolderSettings);
                if (receiveFolderObj != null)
                {
                    config.ReceiveFolderSettings = receiveFolderObj.ToString();
                }

                // Output the extracted configuration
                Console.WriteLine("Replication Configuration:");
                Console.WriteLine($"Message Size: {config.ReplicationMessageSize}");
                Console.WriteLine($"Message Priority: {config.ReplicationMessagePriority}");
                Console.WriteLine($"Style: {config.ReplicationStyle}");
                Console.WriteLine($"Schedule: {config.ReplicationSchedule}");
                Console.WriteLine($"Receive Folder Settings: {config.ReceiveFolderSettings}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Simple POCO to hold replication settings
class ReplicationConfig
{
    public long ReplicationMessageSize { get; set; }
    public long ReplicationMessagePriority { get; set; }
    public string ReplicationStyle { get; set; }
    public string ReplicationSchedule { get; set; }
    public string ReceiveFolderSettings { get; set; }
}
