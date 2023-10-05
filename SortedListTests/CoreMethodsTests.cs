namespace SortedListTests;

public class CoreMethodsTests
{
    public static IEnumerable<object[]> AddData_NoDuplicates()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            10,
            new [] { -600, -100, 1, 3, 10, 87, 200 }
        };
    }

    public static IEnumerable<object[]> AddData_Duplicates()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3, 10 },
            10
        };
    }

    public static IEnumerable<object[]>RemoveData_InTheList()
    {
        yield return new object[]
        {
            new SortedList<int> { -600, -100, 1, 3, 10, 87, 200 },
            10,
            new [] { -600, -100, 1, 3, 87, 200 },
        };
        yield return new object[]
        {
            new SortedList<int> { -100, 1, 3, 10, -600, 87, 200 },
            -600,
            new [] { -100, 1, 3, 10, 87, 200 },
        };
    }

    public static IEnumerable<object[]>RemoveData_NotInTheList()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            10,
        };
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            1992,
        };
    }



    [Theory]
    [MemberData(nameof(AddData_NoDuplicates))]
    public void Add_NoDuplicates_SortedList<T>(SortedList<T> list, T item, T[] expOutput) where T : IComparable<T>
    {
        list.Add(item);

        Helper.AssertEnumerablesEqual(list,expOutput);
    }

    [Theory]
    [MemberData(nameof(AddData_Duplicates))]
    public void Add_Duplicates_ArgumentException<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        Assert.Throws<ArgumentException>(() => list.Add(item));
    }

    [Theory]
    [MemberData(nameof(RemoveData_InTheList))]
    public void Remove_ItemInTheList_True<T>(SortedList<T> list, T item, IEnumerable<T> expected) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.True(isDeletionSuccessful);
        Helper.AssertEnumerablesEqual(list, expected);
    }

    [Theory]
    [MemberData(nameof(RemoveData_NotInTheList))]
    public void Remove_ItemNotInTheList_False<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.False(isDeletionSuccessful);
        Assert.DoesNotContain(item, list);
    }

    [Fact]
    public void Add_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Add(nullString));
    }

    [Fact]
    public void Remove_Null_ArgumentNullException()
    {
        var list = new SortedList<string>();
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Remove(nullString));
    }

    [Fact]
    public void Contains_ItemInTheList_True()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3, k };

        bool contains = list.Contains(k);

        Assert.True(contains);
        Assert.Contains(k, list);
    }

    [Fact]
    public void Contains_ItemNotInTheList_False()
    {
        var k = 10;
        var list = new SortedList<int> { 1, -100, 200, 87, -600, 3 };

        bool contains = list.Contains(k);

        Assert.False(contains);
        Assert.DoesNotContain(k, list);
    }

    [Fact]
    public void Contains_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};
        string nullString = null;

        Assert.Throws<ArgumentNullException>(() => list.Contains(nullString));
    }

    [Fact]
    public void Count_AddRemoveClear_CountChanges()
    {
        var list = new SortedList<int>() { 1, 2, 3 };

        list.Add(4);
        var countAfterAdd = list.Count;
        list.Remove(1);
        var countAfterRemove = list.Count;
        list.Clear();
        var countAfterClear = list.Count;

        Assert.Equal(4, countAfterAdd);
        Assert.Equal(3, countAfterRemove);
        Assert.Equal(0, countAfterClear);
    }
}