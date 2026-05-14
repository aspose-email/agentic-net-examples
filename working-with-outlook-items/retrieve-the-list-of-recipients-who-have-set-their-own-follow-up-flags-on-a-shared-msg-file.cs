using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "shared.msg";

            if (!File.Exists(msgPath))
            {
                // Create a placeholder MSG file if it does not exist.
                using (var placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder Subject",
                    "Placeholder body."))
                {
                    placeholder.Save(msgPath);
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiRecipient recipient in msg.Recipients)
                {
                    // Consider only primary (To) recipients as required.
                    if (recipient.RecipientType != MapiRecipientType.MAPI_TO)
                        continue;

                    // Check if the recipient has a follow‑up flag set.
                    // The flag status is stored in the PR_FLAG_STATUS property (0 = not flagged, 1 = flagged).
                    const int PR_FLAG_STATUS = 0x1090;
                    if (recipient.Properties.ContainsKey(PR_FLAG_STATUS))
                    {
                        var flagProp = recipient.Properties[PR_FLAG_STATUS];
                        int flagStatus = Convert.ToInt32(flagProp.GetValue());

                        if (flagStatus != 0) // 0 = not flagged
                        {
                            Console.WriteLine($"Flagged Recipient: {recipient.DisplayName} <{recipient.EmailAddress}>");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
