namespace SortedList.Test;

public static class MyAssert
{
    public static void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual) where T : IComparable<T>
    {
        var expEnum = expected.GetEnumerator();
        var actEnum = actual.GetEnumerator();

        while (actEnum.MoveNext() | expEnum.MoveNext())
        {
            Assert.Equal(0, expEnum.Current.CompareTo(actEnum.Current));
        }

        expEnum.Dispose();
        actEnum.Dispose();
    }

    public static void Equal<T>(IEnumerator<T> expected, IEnumerator<T> actual) where T : IComparable<T>
    {
        while (actual.MoveNext() | expected.MoveNext())
        {
            Assert.Equal(0, expected.Current.CompareTo(actual.Current));
        }

        expected.Dispose();
        actual.Dispose();
    }

    public static void Contains<T>(T item, SortedList<T>list) where T : IComparable<T>
    {
        var contains = list.Any(i => item.CompareTo(i) == 0);

        Assert.True(contains);
    }

    public static void DoesNotContain<T>(T item ,SortedList<T>list) where T : IComparable<T>
    {
        foreach (var i in list)
        {
            Assert.False(item.CompareTo(i) == 0);
        }
    }
}