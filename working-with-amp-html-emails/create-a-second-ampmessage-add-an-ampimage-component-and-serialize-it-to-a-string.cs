using System;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // First AMP message (could be used for other purposes)
            using (AmpMessage firstMessage = new AmpMessage())
            {
                firstMessage.Subject = "First AMP Email";
                // Additional setup for the first message can be done here
            }

            // Second AMP message where we add an AmpImage component
            using (AmpMessage secondMessage = new AmpMessage())
            {
                secondMessage.Subject = "Second AMP Email";

                // Create an AmpImage with required width and height
                AmpImage image = new AmpImage(300, 200);
                image.Src = "https://example.com/image.png";
                image.Alt = "Example Image";

                // Add the image component to the message
                secondMessage.AddAmpComponent(image);

                // Serialize the message to a string
                string serializedMessage = secondMessage.ToString();

                // Output the serialized AMP message
                Console.WriteLine(serializedMessage);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
