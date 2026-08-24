using Bogus;
using System.Linq;

public static class PasswordGenerator
{
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lower = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string Special = "!@#$%^&*()_-+=";

    public static string Generate(Faker f, int length = 8)
    {
        if (length < 4)
            throw new ArgumentException("Length must be at least 4 to fit all required character types.");

        var chars = new List<char>
        {
            f.Random.ArrayElement(Upper.ToCharArray()),
            f.Random.ArrayElement(Lower.ToCharArray()),
            f.Random.ArrayElement(Digits.ToCharArray()),
            f.Random.ArrayElement(Special.ToCharArray())
        };

        var allChars = Upper + Lower + Digits + Special;
        chars.AddRange(Enumerable.Range(0, length - chars.Count)
            .Select(_ => f.Random.ArrayElement(allChars.ToCharArray())));

        return new string(f.Random.Shuffle(chars).ToArray());
    }
}