namespace SortedList.Test;

public class CoreMethodsTests
{
    public static IEnumerable<object[]> Data_List_NewItem_ExpectedAfterAdd()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3 },
            10,
            new [] { -600, -100, 1, 3, 10, 87, 200 }
        };
    }

    public static IEnumerable<object[]> Data_List_Duplicate()
    {
        yield return new object[]
        {
            new SortedList<int> { 1, -100, 200, 87, -600, 3, 10 },
            10
        };
    }

    public static IEnumerable<object[]>Data_List_Duplicate_ExpectedAfterRemove()
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

    public static IEnumerable<object[]>Data_List_NewItem()
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
    [MemberData(nameof(Data_List_Duplicate))]
    public void OrderRetain_RemoveThenAdd_OrderIsRetained<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        var copy = list.ToArray();

        list.Remove(item);
        list.Add(item);

        MyAssert.Equal(list, copy);
    }

    [Theory]
    [MemberData(nameof(Data_List_NewItem_ExpectedAfterAdd))]
    public void Add_NoDuplicates_SortedList<T>(SortedList<T> list, T item, T[] expOutput) where T : IComparable<T>
    {
        list.Add(item);

        MyAssert.Equal(list,expOutput);
    }

    [Theory]
    [MemberData(nameof(Data_List_Duplicate))]
    public void Add_Duplicates_ArgumentException<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        Action action = () => list.Add(item);

        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [MemberData(nameof(Data_List_Duplicate_ExpectedAfterRemove))]
    public void Remove_ItemInTheList_True<T>(SortedList<T> list, T item, IEnumerable<T> expected) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.True(isDeletionSuccessful);
        MyAssert.Equal(list, expected);
    }

    [Theory]
    [MemberData(nameof(Data_List_NewItem))]
    public void Remove_ItemNotInTheList_False<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        bool isDeletionSuccessful = list.Remove(item);

        Assert.False(isDeletionSuccessful);
        MyAssert.DoesNotContain(item, list);
    }

    [Fact]
    public void Add_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};

        string nullString = null;
        Action action = () => list.Add(nullString);

        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Remove_Null_ArgumentNullException()
    {
        var list = new SortedList<string>();

        string nullString = null;
        Action action = () => list.Remove(nullString);

        Assert.Throws<ArgumentNullException>(action);
    }

    [Theory]
    [MemberData(nameof(Data_List_Duplicate))]
    public void Contains_ItemInTheList_True<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        bool contains = list.Contains(item);

        Assert.True(contains);
        MyAssert.Contains(item, list);
    }

    [Theory]
    [MemberData(nameof(Data_List_NewItem))]
    public void Contains_ItemNotInTheList_False<T>(SortedList<T> list, T item) where T : IComparable<T>
    {
        bool contains = list.Contains(item);

        Assert.False(contains);
        MyAssert.DoesNotContain(item, list);
    }

    [Fact]
    public void Contains_Null_ArgumentNullException()
    {
        var list = new SortedList<string> {};

        string nullString = null;
        Action action = () => list.Contains(nullString);

        Assert.Throws<ArgumentNullException>(action);
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