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

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Extract replication related properties using the known property descriptors
                ReplicationConfig config = new ReplicationConfig
                {
                    ReplicationStyle = GetLongProperty(msg, KnownPropertyList.ReplicationStyle),
                    ReplicationMessagePriority = GetLongProperty(msg, KnownPropertyList.ReplicationMessagePriority),
                    ReplicationMsgSize = GetLongProperty(msg, KnownPropertyList.ReplicationMsgSize),
                    ReplicationAlwaysInterval = GetLongProperty(msg, KnownPropertyList.ReplicationAlwaysInterval),
                    ReplicationSchedule = GetStringProperty(msg, KnownPropertyList.ReplicationSchedule),
                    EmsAbReplicationSensitivity = GetLongProperty(msg, KnownPropertyList.EmsAbReplicationSensitivity),
                    EmsAbReplicationMailMsgSize = GetLongProperty(msg, KnownPropertyList.EmsAbReplicationMailMsgSize),
                    EmsAbReplicationStagger = GetLongProperty(msg, KnownPropertyList.EmsAbReplicationStagger)
                };

                // Output the extracted configuration values
                Console.WriteLine("Replication Configuration:");
                Console.WriteLine($"  ReplicationStyle: {config.ReplicationStyle}");
                Console.WriteLine($"  ReplicationMessagePriority: {config.ReplicationMessagePriority}");
                Console.WriteLine($"  ReplicationMsgSize: {config.ReplicationMsgSize}");
                Console.WriteLine($"  ReplicationAlwaysInterval: {config.ReplicationAlwaysInterval}");
                Console.WriteLine($"  ReplicationSchedule: {config.ReplicationSchedule}");
                Console.WriteLine($"  EmsAbReplicationSensitivity: {config.EmsAbReplicationSensitivity}");
                Console.WriteLine($"  EmsAbReplicationMailMsgSize: {config.EmsAbReplicationMailMsgSize}");
                Console.WriteLine($"  EmsAbReplicationStagger: {config.EmsAbReplicationStagger}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper to retrieve a long (Int64) property safely
    private static long GetLongProperty(MapiMessage msg, PidTagPropertyDescriptor descriptor)
    {
        try
        {
            object value = msg.GetProperty(descriptor);
            return value != null ? Convert.ToInt64(value) : 0L;
        }
        catch
        {
            return 0L;
        }
    }

    // Helper to retrieve a string property safely
    private static string GetStringProperty(MapiMessage msg, PidTagPropertyDescriptor descriptor)
    {
        try
        {
            object value = msg.GetProperty(descriptor);
            return value?.ToString() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}

// Simple POCO to hold the replication settings
class ReplicationConfig
{
    public long ReplicationStyle { get; set; }
    public long ReplicationMessagePriority { get; set; }
    public long ReplicationMsgSize { get; set; }
    public long ReplicationAlwaysInterval { get; set; }
    public string ReplicationSchedule { get; set; }
    public long EmsAbReplicationSensitivity { get; set; }
    public long EmsAbReplicationMailMsgSize { get; set; }
    public long EmsAbReplicationStagger { get; set; }
}
