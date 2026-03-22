namespace CrashBytes.Collections.Tests;

public class IsNullOrEmptyCollectionTests
{
    [Fact]
    public void IsNullOrEmpty_NullCollection_ReturnsTrue()
    {
        ICollection<int>? source = null;
        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyCollection_ReturnsTrue()
    {
        ICollection<int> empty = new List<int>();
        Assert.True(empty.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NonEmpty_ReturnsFalse()
    {
        ICollection<int> list = new List<int> { 1 };
        Assert.False(list.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NullReadOnlyCollection_ReturnsTrue()
    {
        IReadOnlyCollection<int>? source = null;
        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyReadOnly_ReturnsTrue()
    {
        IReadOnlyCollection<int> source = Array.Empty<int>();
        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyReadOnly_ReturnsFalse()
    {
        IReadOnlyCollection<int> source = new[] { 1 };
        Assert.False(source.IsNullOrEmpty());
    }
}

public class EmptyIfNullTests
{
    [Fact]
    public void EmptyIfNull_NullSource_ReturnsEmpty()
    {
        IEnumerable<int>? source = null;
        Assert.Empty(source.EmptyIfNull());
    }

    [Fact]
    public void EmptyIfNull_NonNull_ReturnsSame()
    {
        var source = new[] { 1, 2, 3 };
        Assert.Equal(new[] { 1, 2, 3 }, source.EmptyIfNull());
    }
}

public class AddRangeTests
{
    [Fact]
    public void AddRange_AddsAllElements()
    {
        var list = new List<int> { 1 };
        ICollection<int> collection = list;
        collection.AddRange(new[] { 2, 3, 4 });
        Assert.Equal(new[] { 1, 2, 3, 4 }, list);
    }

    [Fact]
    public void AddRange_EmptyItems_NoChange()
    {
        var list = new List<int> { 1 };
        ICollection<int> collection = list;
        collection.AddRange(Array.Empty<int>());
        Assert.Single(list);
    }

    [Fact]
    public void AddRange_NullSource_ThrowsArgumentNullException()
    {
        ICollection<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.AddRange(new[] { 1 }));
    }

    [Fact]
    public void AddRange_NullItems_ThrowsArgumentNullException()
    {
        var list = new List<int>();
        ICollection<int> collection = list;
        Assert.Throws<ArgumentNullException>(() => collection.AddRange(null!));
    }
}

public class GetOrAddTests
{
    [Fact]
    public void GetOrAdd_KeyExists_ReturnsExistingValue()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        Assert.Equal(1, dict.GetOrAdd("a", 99));
    }

    [Fact]
    public void GetOrAdd_KeyMissing_AddsAndReturnsDefault()
    {
        var dict = new Dictionary<string, int>();
        Assert.Equal(99, dict.GetOrAdd("a", 99));
        Assert.Equal(99, dict["a"]);
    }

    [Fact]
    public void GetOrAdd_WithFactory_KeyExists_DoesNotCallFactory()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        bool called = false;
        dict.GetOrAdd("a", _ => { called = true; return 99; });
        Assert.False(called);
    }

    [Fact]
    public void GetOrAdd_WithFactory_KeyMissing_CallsFactory()
    {
        var dict = new Dictionary<string, int>();
        var result = dict.GetOrAdd("a", key => key.Length);
        Assert.Equal(1, result);
        Assert.Equal(1, dict["a"]);
    }

    [Fact]
    public void GetOrAdd_NullDictionary_ThrowsArgumentNullException()
    {
        IDictionary<string, int> dict = null!;
        Assert.Throws<ArgumentNullException>(() => dict.GetOrAdd("a", 1));
    }

    [Fact]
    public void GetOrAdd_WithFactory_NullFactory_ThrowsArgumentNullException()
    {
        var dict = new Dictionary<string, int>();
        Assert.Throws<ArgumentNullException>(() => dict.GetOrAdd("a", (Func<string, int>)null!));
    }
}

public class GetValueOrDefaultTests
{
    [Fact]
    public void GetValueOrDefault_KeyExists_ReturnsValue()
    {
        var dict = new Dictionary<string, int> { ["a"] = 42 };
        Assert.Equal(42, dict.GetValueOrDefault("a", -1));
    }

    [Fact]
    public void GetValueOrDefault_KeyMissing_ReturnsDefault()
    {
        var dict = new Dictionary<string, int>();
        Assert.Equal(-1, dict.GetValueOrDefault("a", -1));
    }

    [Fact]
    public void GetValueOrDefault_NullDictionary_ThrowsArgumentNullException()
    {
        IDictionary<string, int> dict = null!;
        Assert.Throws<ArgumentNullException>(() => dict.GetValueOrDefault("a"));
    }
}

public class RemoveWhereTests
{
    [Fact]
    public void RemoveWhere_RemovesMatchingElements()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        int removed = list.RemoveWhere(x => x % 2 == 0);
        Assert.Equal(2, removed);
        Assert.Equal(new[] { 1, 3, 5 }, list);
    }

    [Fact]
    public void RemoveWhere_NoneMatch_ReturnsZero()
    {
        var list = new List<int> { 1, 3, 5 };
        Assert.Equal(0, list.RemoveWhere(x => x % 2 == 0));
    }

    [Fact]
    public void RemoveWhere_AllMatch_RemovesAll()
    {
        var list = new List<int> { 2, 4, 6 };
        Assert.Equal(3, list.RemoveWhere(x => x % 2 == 0));
        Assert.Empty(list);
    }

    [Fact]
    public void RemoveWhere_NullSource_ThrowsArgumentNullException()
    {
        IList<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.RemoveWhere(x => true));
    }

    [Fact]
    public void RemoveWhere_NullPredicate_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new List<int>().RemoveWhere(null!));
    }
}

public class ShuffleInPlaceTests
{
    [Fact]
    public void Shuffle_ContainsSameElements()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        list.Shuffle();
        Assert.Equal(5, list.Count);
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, list.OrderBy(x => x));
    }

    [Fact]
    public void Shuffle_WithSeededRandom_IsDeterministic()
    {
        var list1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var list2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        list1.Shuffle(new Random(42));
        list2.Shuffle(new Random(42));
        Assert.Equal(list1, list2);
    }

    [Fact]
    public void Shuffle_NullSource_ThrowsArgumentNullException()
    {
        IList<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.Shuffle());
    }
}

public class WithTests
{
    [Fact]
    public void With_AddsElementAndReturnsCollection()
    {
        var list = new List<int> { 1 };
        ICollection<int> result = list.With(2);
        Assert.Same(list, result);
        Assert.Equal(new[] { 1, 2 }, list);
    }

    [Fact]
    public void With_NullSource_ThrowsArgumentNullException()
    {
        ICollection<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.With(1));
    }
}

public class TryAddTests
{
    [Fact]
    public void TryAdd_NewKey_AddsAndReturnsTrue()
    {
        var dict = new Dictionary<string, int>();
        Assert.True(dict.TryAdd("a", 1));
        Assert.Equal(1, dict["a"]);
    }

    [Fact]
    public void TryAdd_ExistingKey_ReturnsFalseDoesNotOverwrite()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        Assert.False(dict.TryAdd("a", 99));
        Assert.Equal(1, dict["a"]);
    }

    [Fact]
    public void TryAdd_NullDictionary_ThrowsArgumentNullException()
    {
        IDictionary<string, int> dict = null!;
        Assert.Throws<ArgumentNullException>(() => dict.TryAdd("a", 1));
    }
}

public class AsReadOnlyTests
{
    [Fact]
    public void AsReadOnly_ReturnsReadOnlyCopy()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var readOnly = dict.AsReadOnly();
        Assert.Equal(2, readOnly.Count);
        Assert.Equal(1, readOnly["a"]);
    }

    [Fact]
    public void AsReadOnly_NullDictionary_ThrowsArgumentNullException()
    {
        IDictionary<string, int> dict = null!;
        Assert.Throws<ArgumentNullException>(() => dict.AsReadOnly());
    }
}
