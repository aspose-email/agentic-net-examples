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
            string msgPath = "input.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body",
                        OutlookMessageFormat.Unicode))
                    {
                        placeholder.Save(msgPath);
                    }
                    Console.WriteLine($"Created placeholder MSG file at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Read replication settings.
                long replicationMsgSize = 0;
                int replicationMessagePriority = 0;
                bool hasSize = false;
                bool hasPriority = false;

                // ReplicationMsgSize property (long)
                try
                {
                    hasSize = msg.TryGetPropertyLong(KnownPropertyList.ReplicationMsgSize.Tag, ref replicationMsgSize);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading ReplicationMsgSize: {ex.Message}");
                }

                // ReplicationMessagePriority property (int)
                try
                {
                    hasPriority = msg.TryGetPropertyInt32(KnownPropertyList.ReplicationMessagePriority.Tag, ref replicationMessagePriority);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading ReplicationMessagePriority: {ex.Message}");
                }

                // Map to a simple configuration object.
                ReplicationConfig config = new ReplicationConfig
                {
                    MessageSize = hasSize ? replicationMsgSize : (long?)null,
                    MessagePriority = hasPriority ? replicationMessagePriority : (int?)null
                };

                // Output the configuration.
                Console.WriteLine("Replication Settings:");
                Console.WriteLine($"  Message Size: {(config.MessageSize.HasValue ? config.MessageSize.Value.ToString() : "Not available")}");
                Console.WriteLine($"  Message Priority: {(config.MessagePriority.HasValue ? config.MessagePriority.Value.ToString() : "Not available")}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

// Simple configuration holder for replication settings.
class ReplicationConfig
{
    public long? MessageSize { get; set; }
    public int? MessagePriority { get; set; }
}
