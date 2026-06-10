using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Argument validation does not require a network connection.
                    TestDeleteMessage(client, 0);
                    TestDeleteMessage(client, -1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void TestDeleteMessage(Pop3Client client, int index)
    {
        try
        {
            client.DeleteMessage(index);
            Console.WriteLine($"DeleteMessage({index}) did NOT throw as expected.");
        }
        catch (Pop3Exception)
        {
            Console.WriteLine($"DeleteMessage({index}) threw Pop3Exception as expected.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"DeleteMessage({index}) threw ArgumentException as expected.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"DeleteMessage({index}) threw unexpected exception: {ex.GetType().Name}");
        }
    }
}
