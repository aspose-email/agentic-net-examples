using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.Priority = MailPriority.High;
                Console.WriteLine($"Priority set to: {mailMessage.Priority}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
