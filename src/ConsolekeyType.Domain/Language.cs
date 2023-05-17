using System.Reflection;

namespace ConsolekeyType.Domain;

public class Language : Entity
{
    public static Language English => new(1, "English");
    public static Language Russian => new(2, "Russian");

    private static Dictionary<string, Language> _dictionary = new()
    {
        { nameof(English), English },
        { nameof(Russian), Russian }
    };

    public virtual string Name { get; }

    private Language() {}

    private Language(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Language> Get(Maybe<string> name)
    {
        // var props = new Language().GetType().GetProperties(BindingFlags.Static | BindingFlags.Public);

        if (name.HasNoValue)
            return Result.Failure<Language>("Language name is not specified");

        if (_dictionary.TryGetValue(name.Value, out var language))
            return Result.Success(language);

        return Result.Failure<Language>("Language name is invalid: " + name.Value);
    }
}