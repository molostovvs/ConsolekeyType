namespace ConsolekeyType.UI;

public static class ConsoleKeyExtensions
{
    public static bool IsLetter(this ConsoleKeyInfo input)
        => char.IsLetter(input.KeyChar);
}