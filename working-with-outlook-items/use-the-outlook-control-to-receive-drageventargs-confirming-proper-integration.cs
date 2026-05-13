using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a sample Outlook message in memory
            using (MapiMessage draggedMessage = CreateSampleMessage())
            {
                // Simulate handling of a drag‑enter event from Outlook
                OutlookDragHandler handler = new OutlookDragHandler();
                handler.OnMessageDragged(draggedMessage);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    static MapiMessage CreateSampleMessage()
    {
        // Initialize a MapiMessage with minimal required fields
        return new MapiMessage(
            "alice@example.com",
            "bob@example.com",
            "Sample Subject",
            "This is a sample body.");
    }
}

// Handler that would be invoked when an Outlook message is dragged onto a control
class OutlookDragHandler
{
    // In a UI scenario this method could be bound to the DragEnter event.
    // Here it simply processes the provided MapiMessage.
    public void OnMessageDragged(MapiMessage message)
    {
        if (message == null)
        {
            Console.WriteLine("No message received.");
            return;
        }

        Console.WriteLine("Dragged message received:");
        Console.WriteLine("Subject: " + message.Subject);
        Console.WriteLine("From: " + message.SenderEmailAddress);
        Console.WriteLine("To: " + message.Recipients);
    }
}
