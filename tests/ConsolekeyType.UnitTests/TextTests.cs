namespace ConsolekeyType.UnitTests;

[TestFixture]
public class TextTests
{
    [Test]
    public void Creating_text_with_zero_words_fails()
    {
        var result = Text.Create(Enumerable.Empty<Word>(), Language.English);

        result.Should().Fail();
    }

    [Test]
    public void Text_with_null_words_throws_exception()
    {
        Action createMethod = () => Text.Create((IEnumerable<Word>)null, Language.English);

        createMethod.Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'words')");
    }

    [Test]
    public void Text_with_null_language_throws_exception()
    {
        var words = CreateDefaultWords();

        Action createMethod = () => Text.Create(words, null);

        createMethod.Should()
                    .ThrowExactly<ArgumentNullException>()
                    .WithMessage("Value cannot be null. (Parameter 'language')");
    }

    [Test]
    public void Text_with_words()
    {
        var words = CreateDefaultWords();

        var createMethod = () => Text.Create(words, Language.English);

        createMethod.Should().NotThrow();
    }

    [Test]
    public void Compare_fully_identical()
    {
        var words = CreateDefaultWords();

        var text1 = Text.Create(words, Language.English).Value;
        var text2 = Text.Create(words, Language.English).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeTrue();
    }

    [Test]
    public void Compare_with_identical_words_and_different_languages()
    {
        var words = CreateDefaultWords();
        var text1 = Text.Create(words, Language.English).Value;
        var text2 = Text.Create(words, Language.Russian).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Compare_with_different_words_and_identical_languages()
    {
        var words1 = CreateWords("a", "b", "c");
        var words2 = CreateWords("a", "b", "x");

        var text1 = Text.Create(words1, Language.English).Value;
        var text2 = Text.Create(words2, Language.English).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Compare_completely_different()
    {
        var words1 = CreateWords("a", "b", "c");
        var words2 = CreateWords("a", "b", "x");

        var text1 = Text.Create(words1, Language.English).Value;
        var text2 = Text.Create(words2, Language.Russian).Value;

        var equality = text1.Equals(text2);

        equality.Should().BeFalse();
    }

    [Test]
    public void Converting_text_to_string()
    {
        var words = CreateDefaultWords();

        var text = Text.Create(words, Language.English).Value;

        string stringText = text;

        stringText.Should().Be(DefaultWords);
    }

    /*//converting to text
    [Test]
    public void Converting_from_string_to_text()
    {
        const string str = "a b c";
        var expected = Text.Create(CreateWords("a", "b", "c"), Language.English).Value;

        var actual = (Text)str;

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Converting_from_empty_string_to_text_throws()
    {
        const string str = "       ";

        var act = () => (Text)str;

        act.Should().ThrowExactly<DomainException>("The number of words must be greater than 0");
    }

    [Test]
    public void Converting_from_null_string_to_text_throws()
    {
        const string str = null;

        var act = () => (Text)str;

        act.Should().ThrowExactly<DomainException>("The number of words must be greater than 0");
    }*/

    private IReadOnlyList<Word> CreateWords(params string[] words)
        => words.Select(word => Word.Create(word).Value).ToList();

    private const string DefaultWords = "a b c";

    private IReadOnlyList<Word> CreateDefaultWords()
        => CreateWords(DefaultWords.Split(" "));
}