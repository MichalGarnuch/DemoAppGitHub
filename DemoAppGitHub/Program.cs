using System.Globalization;

namespace DemoAppGitHub;
internal static class Program
{
    public static int Main(string[] args)
    {
        if (args.Contains("--help"))
        {
            Console.WriteLine("Usage: DemoAppGitHub [--utc]");
            return 0;
        }
        if (args.Any(arg => arg != "--utc"))
        {
            Console.Error.WriteLine("Unknown argument. Use --help.");
            return 2;
        }
        var now = args.Contains("--utc") ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
        Console.WriteLine("Hello, GitHub!");
        Console.WriteLine(now.ToString("O", CultureInfo.InvariantCulture));
        return 0; // No ReadLine: safe in CI and redirected processes.
    }
}
