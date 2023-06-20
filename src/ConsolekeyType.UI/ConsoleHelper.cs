using System.Collections.ObjectModel;
using System.Text;

namespace ConsolekeyType.UI;

public static class ConsoleHelper
{
    public static void SetCursorMin()
        => Console.SetCursorPosition(0, 0);

    public static (int x, int y) GetCenter()
        => (Console.WindowWidth / 2, Console.WindowHeight / 2);

    public static void SetCursorCenter()
    {
        var center = GetCenter();
        Console.SetCursorPosition(center.x, center.y);
    }

    public static void SetCursorCenterWithOffset(int x, int y)
    {
        var center = GetCenter();
        Console.SetCursorPosition(center.x + x, center.y + y);
    }

    public static void SetCursorMax()
        => Console.SetCursorPosition(Console.WindowWidth, Console.WindowHeight);

    public static void Clear()
    {
        // SetCursorMin();
        var height = Console.WindowHeight;

        for (var i = 0; i < height - 1; i++)
            Console.WriteLine();

        SetCursorMin();
    }

    public static void WriteCentered(string text)
    {
        var original = Console.GetCursorPosition();
        Console.SetCursorPosition(original.Left - text.Length / 2, original.Top);
        Console.Write(text);
        Console.SetCursorPosition(original.Left, original.Top);
    }

    public static void WriteWithColor(string text, Color color)
    {
        var colorTextBuilder = GetColorTextBuilder(color);
        colorTextBuilder.Append(text);
        colorTextBuilder.Append(TextFormatting.Reset);

        Console.Write(colorTextBuilder);
    }

    private static StringBuilder GetColorTextBuilder(Color color)
    {
        var colorTextBuilder = new StringBuilder();
        colorTextBuilder.Append(TextFormatting.Start);
        colorTextBuilder.Append(TextFormatting.ColorRGB);
        colorTextBuilder.Append(color.R);
        colorTextBuilder.Append(';');
        colorTextBuilder.Append(color.G);
        colorTextBuilder.Append(';');
        colorTextBuilder.Append(color.B);
        colorTextBuilder.Append('m');

        return colorTextBuilder;
    }

    public static void PrintScrollableList<T>(IEnumerable<T> items, T defaultSelection,
        int showCount)
    {
        var orig = Console.GetCursorPosition();

        var list = items.ToList();
        var longestValue = list.Max(x => x!.ToString()!.Length);
        var defaultIndex = list.IndexOf(defaultSelection);

        var firstShow = defaultIndex - showCount / 2 < 0
            ? list.Count - showCount / 2 + defaultIndex
            : defaultIndex - showCount / 2;

        Console.SetCursorPosition(orig.Left, orig.Top - showCount / 2);
        var currentPos = Console.GetCursorPosition();

        for (var i = firstShow; i < firstShow + showCount; i++)
        {
            Console.SetCursorPosition(currentPos.Left, currentPos.Top);

            var index = i < list.Count ? i : i % list.Count;
            var item = list[index];

            var itemBuilder = new StringBuilder();

            if (index == defaultIndex)
            {
                itemBuilder.Append(TextFormatting.Start);
                itemBuilder.Append(TextFormatting.Underline);
            }

            itemBuilder.Append(item);
            itemBuilder.Append(TextFormatting.Reset);
            Console.Write(itemBuilder);

            if (item!.ToString()!.Length < longestValue)
                Console.Write(
                    string.Concat(Enumerable.Repeat(" ", longestValue - item.ToString()!.Length))
                );

            currentPos.Top += 1;
        }
    }

    public static void PrintScrollableListWithSelection<T>(IEnumerable<T> items, T selectedItem,
        int showCount, Color selectionColor)
    {
        var orig = Console.GetCursorPosition();

        var list = items.ToList();
        var longestValue = list.Max(x => x!.ToString()!.Length);
        var selectedIndex = list.IndexOf(selectedItem);

        var firstItem = selectedIndex - showCount / 2 < 0
            ? list.Count - showCount / 2 + selectedIndex
            : selectedIndex - showCount / 2;

        Console.SetCursorPosition(orig.Left, orig.Top - showCount / 2);
        var currentPos = Console.GetCursorPosition();

        for (var i = firstItem; i < firstItem + showCount; i++)
        {
            Console.SetCursorPosition(currentPos.Left, currentPos.Top);

            var currentIndex = i < list.Count ? i : i % list.Count;
            var currentItem = list[currentIndex];

            if (currentIndex == selectedIndex)
            {
                var sb = GetColorTextBuilder(selectionColor);
                sb.Append("> ");
                sb.Append(currentItem);
                sb.Append(TextFormatting.Reset);
                Console.SetCursorPosition(currentPos.Left - 2, currentPos.Top);

                if (currentItem.ToString().Length < longestValue)
                    sb.AppendJoin(
                        "",
                        Enumerable.Repeat(
                            " ",
                            longestValue - currentItem.ToString().Length + "> ".Length
                        )
                    );

                Console.Write(sb);

                Console.SetCursorPosition(currentPos.Left, currentPos.Top);
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(currentItem);

                if (currentItem.ToString().Length < longestValue)
                    sb.AppendJoin(
                        "",
                        Enumerable.Repeat(
                            " ",
                            longestValue - currentItem.ToString().Length + "> ".Length
                        )
                    );

                Console.Write(sb);
            }

            currentPos.Top += 1;
        }
    }

    private static class TextFormatting
    {
        public const string Start = "\x1b[";
        public const string ColorRGB = "38;2;";
        public const string Reset = "\x1b[0m";
        public const string Underline = "4m";
    }
}

public class Color
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
}