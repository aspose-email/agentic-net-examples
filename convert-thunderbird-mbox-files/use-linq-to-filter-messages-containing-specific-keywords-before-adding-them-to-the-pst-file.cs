using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "FilteredMessages.pst";

            // Ensure the PST file exists; create if it does not.
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                pst = PersonalStorage.FromFile(pstPath);
            }
            else
            {
                pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            using (pst)
            {
                // Get the standard Inbox folder (creates it if missing).
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Sample messages to be added.
                List<MapiMessage> messages = new List<MapiMessage>();
                MapiMessage msg1 = new MapiMessage("alice@example.com", "bob@example.com", "Meeting agenda", "Please review the agenda.");
                MapiMessage msg2 = new MapiMessage("carol@example.com", "dave@example.com", "Project update", "The project is on schedule.");
                MapiMessage msg3 = new MapiMessage("eve@example.com", "frank@example.com", "Urgent: Server down", "The server is down, need immediate attention.");
                messages.Add(msg1);
                messages.Add(msg2);
                messages.Add(msg3);

                // Keywords to filter messages.
                string[] keywords = new string[] { "Urgent", "agenda" };

                // LINQ filter: keep messages whose subject or body contains any keyword (case‑insensitive).
                IEnumerable<MapiMessage> filtered = messages.Where(m =>
                    keywords.Any(k =>
                        (m.Subject != null && m.Subject.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (m.Body != null && m.Body.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)));

                // Add the filtered messages to the PST Inbox folder.
                foreach (MapiMessage message in filtered)
                {
                    inboxFolder.AddMessage(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
