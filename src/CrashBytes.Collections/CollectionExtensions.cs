namespace CrashBytes.Collections;

/// <summary>
/// Extension methods for collections, dictionaries, and arrays.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns <c>true</c> if the collection is <c>null</c> or contains no elements.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this ICollection<T>? source) =>
        source is null || source.Count == 0;

    /// <summary>
    /// Returns <c>true</c> if the read-only collection is <c>null</c> or contains no elements.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T>? source) =>
        source is null || source.Count == 0;

    /// <summary>
    /// Returns the source collection, or an empty array if the source is <c>null</c>.
    /// </summary>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source) =>
        source ?? Enumerable.Empty<T>();

    /// <summary>
    /// Adds all elements from <paramref name="items"/> to the collection.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="items"/> is <c>null</c>.</exception>
    public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (items is null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
            source.Add(item);
    }

    /// <summary>
    /// Gets the value for <paramref name="key"/>, or adds and returns <paramref name="defaultValue"/> if the key does not exist.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));

        if (dictionary.TryGetValue(key, out var value))
            return value;

        dictionary[key] = defaultValue;
        return defaultValue;
    }

    /// <summary>
    /// Gets the value for <paramref name="key"/>, or adds and returns the result of <paramref name="valueFactory"/> if the key does not exist.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="valueFactory"/> is <c>null</c>.</exception>
    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueFactory)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (valueFactory is null) throw new ArgumentNullException(nameof(valueFactory));

        if (dictionary.TryGetValue(key, out var value))
            return value;

        value = valueFactory(key);
        dictionary[key] = value;
        return value;
    }

    /// <summary>
    /// Gets the value for <paramref name="key"/>, or returns <paramref name="defaultValue"/> without modifying the dictionary.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue = default!)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));

        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Removes all elements matching <paramref name="predicate"/> from the list.
    /// Returns the number of elements removed.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
    public static int RemoveWhere<T>(this IList<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        int removed = 0;
        for (int i = source.Count - 1; i >= 0; i--)
        {
            if (predicate(source[i]))
            {
                source.RemoveAt(i);
                removed++;
            }
        }

        return removed;
    }

    /// <summary>
    /// Shuffles the list in-place using the Fisher-Yates algorithm.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
    public static void Shuffle<T>(this IList<T> source, Random? random = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var rng = random ?? new Random();
        for (int i = source.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (source[i], source[j]) = (source[j], source[i]);
        }
    }

    /// <summary>
    /// Adds the element to the collection and returns the collection for fluent chaining.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
    public static ICollection<T> With<T>(this ICollection<T> source, T item)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        source.Add(item);
        return source;
    }

    /// <summary>
    /// Adds the key-value pair only if the key does not already exist in the dictionary.
    /// Returns <c>true</c> if the value was added; <c>false</c> if the key already existed.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
    public static bool TryAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));

        if (dictionary.ContainsKey(key))
            return false;

        dictionary[key] = value;
        return true;
    }

    /// <summary>
    /// Creates a read-only wrapper around the dictionary.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
    public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        return new Dictionary<TKey, TValue>(dictionary);
    }
}
