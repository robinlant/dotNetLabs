namespace SortedList.Test;

public class EnumeratorTests
{
    public static IEnumerable<object[]> IteratorData_ListWithExpectedSequence()
    {
        yield return new object[]
        {
            new SortedList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
        };
    }

    [Theory]
    [MemberData(nameof(IteratorData_ListWithExpectedSequence))]
    public void Enumerator_SortedListItems_ReturnsItemsInExpectedOrder<T>(SortedList<T> list, List<T> expected) where T : IComparable<T>
    {
        var expectedEnumerator = expected.GetEnumerator();

        var actualEnumerator = list.GetEnumerator();

        MyAssert.Equal(expectedEnumerator, actualEnumerator);
    }

    [Theory]
    [MemberData(nameof(IteratorData_ListWithExpectedSequence))]
    public void ReversedEnumerator_SortedListItems_ReturnsItemsInExpectedOrder<T>(SortedList<T> list, List<T> expected) where T : IComparable<T>
    {
        expected.Reverse();
        var expectedEnumerator = expected.GetEnumerator();

        var actualEnumerator = list.Reversed();

        MyAssert.Equal(expectedEnumerator, actualEnumerator);
    }

    [Fact]
    public void Enumerator_Reset_NotSupportedException()
    {
        var list = new SortedList<int>();

        var enumerator = list.GetEnumerator();
        var action = () => enumerator.Reset();

        Assert.Throws<NotSupportedException>(action);
    }

    [Fact]
    public void ReversedEnumerator_Reset_NotSupportedException()
    {
        var list = new SortedList<int>();

        var enumerator = list.Reversed();
        var action = () => enumerator.Reset();

        Assert.Throws<NotSupportedException>(action);
    }

    [Fact]
    public void Enumerator_MoveNext_GetEnumeratorWhenCollectionIsEmpty_MoveNextReturnsFalse()
    {
        var list = new SortedList<int>();

        var enumerator = list.GetEnumerator();
        var hasNext = enumerator.MoveNext();

        Assert.False(hasNext);
    }

    [Fact]
    public void ReversedEnumerator_MoveNext_GetEnumeratorWhenCollectionIsEmpty_MoveNextReturnsFalse()
    {
        var list = new SortedList<int>();

        var enumerator = list.Reversed();
        var hasNext = enumerator.MoveNext();

        Assert.False(hasNext);
    }

    [Fact]
    public void Enumerator_Current_EnumerationIsNotStarted_InvalidOperationException()
    {
        var list = new SortedList<int>();

        var enumerator = list.GetEnumerator();
        object Action() => enumerator.Current;

        Assert.Throws<InvalidOperationException>(Action);
    }

    [Fact]
    public void ReversedEnumerator_Current_EnumerationIsNotStarted_ReturnsDefaultValue()
    {
        var list = new SortedList<int>();

        var enumerator = list.Reversed();
        var current = enumerator.Current;

        Assert.Equal(default,current);
    }


    [Fact]
    public void ReversedEnumerator_Current_EnumerationIsAlreadyFinished_ReturnsFirstItemInCollection()
    {
        var list = new SortedList<int>() { 1, 2, 3, 4 };
        var FIRST_ITEM_IN_LIST = 1;

        var enumerator = list.Reversed();
        while (enumerator.MoveNext()) { }

        var current = enumerator.Current;

        Assert.Equal(FIRST_ITEM_IN_LIST,current);
    }

    [Fact]
    public void Enumerator_Current_EnumerationIsAlreadyFinished_InvalidOperationException()
    {
        var list = new SortedList<int>() { 1, 2, 3, 4 };

        var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext()) { }
        object Action() => enumerator.Current;

        Assert.Throws<InvalidOperationException>(Action);
    }
}