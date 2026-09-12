using System;
using System.Collections.Generic;

enum CalendarPermission
{
    None = 0,
    Read = 1,
    Write = 2,
    Share = 3
}

class CalendarResource
{
    private readonly string _name;
    private readonly Dictionary<string, CalendarPermission> _userPermissions = new Dictionary<string, CalendarPermission>(StringComparer.OrdinalIgnoreCase);

    public CalendarResource(string name)
    {
        _name = name;
    }

    public void GrantPermission(string userEmail, CalendarPermission permission)
    {
        _userPermissions[userEmail] = permission;
        Console.WriteLine($"Granted {permission} permission to {userEmail} on calendar '{_name}'.");
    }

    private bool HasPermission(string userEmail, CalendarPermission required)
    {
        if (!_userPermissions.TryGetValue(userEmail, out var userPerm))
            return false;

        // Permission hierarchy: Share > Write > Read
        return userPerm >= required;
    }

    public bool CanRead(string userEmail) => HasPermission(userEmail, CalendarPermission.Read);
    public bool CanWrite(string userEmail) => HasPermission(userEmail, CalendarPermission.Write);
    public bool CanShare(string userEmail) => HasPermission(userEmail, CalendarPermission.Share);

    public void ReadEvent(string userEmail)
    {
        if (CanRead(userEmail))
            Console.WriteLine($"{userEmail} reads events from calendar '{_name}'.");
        else
            Console.WriteLine($"{userEmail} does NOT have read permission on calendar '{_name}'.");
    }

    public void WriteEvent(string userEmail, string eventTitle)
    {
        if (CanWrite(userEmail))
            Console.WriteLine($"{userEmail} writes event '{eventTitle}' to calendar '{_name}'.");
        else
            Console.WriteLine($"{userEmail} does NOT have write permission on calendar '{_name}'.");
    }

    public void ShareCalendar(string userEmail, string targetUserEmail)
    {
        if (CanShare(userEmail))
        {
            // Default shared permission is Read
            GrantPermission(targetUserEmail, CalendarPermission.Read);
            Console.WriteLine($"{userEmail} shared calendar '{_name}' with {targetUserEmail} (Read permission).");
        }
        else
        {
            Console.WriteLine($"{userEmail} does NOT have share permission on calendar '{_name}'.");
        }
    }
}

class Program
{
    static void Main()
    {
        var calendar = new CalendarResource("Team Meetings");

        // Owner gets full permissions
        string owner = "owner@example.com";
        calendar.GrantPermission(owner, CalendarPermission.Share);

        // Other users
        string alice = "alice@example.com";
        string bob = "bob@example.com";

        // Owner shares with Alice (read)
        calendar.ShareCalendar(owner, alice);

        // Owner grants write permission to Bob
        calendar.GrantPermission(bob, CalendarPermission.Write);

        // Test operations
        calendar.ReadEvent(alice); // should succeed
        calendar.WriteEvent(alice, "Sprint Review"); // should fail

        calendar.ReadEvent(bob); // should succeed (write includes read)
        calendar.WriteEvent(bob, "Sprint Planning"); // should succeed

        // Bob tries to share (should fail)
        calendar.ShareCalendar(bob, "charlie@example.com");
    }
}
