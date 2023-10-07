namespace SortedList.Test;

public class CopyToTests
{
    public static IEnumerable<object[]> CopyToData_ValidArgs()
    {
        yield return new object[]
        {
            new SortedList<int>() { 1, 2, 3, 4 },
            1,
            new int[5],
            new int[] { 0, 1, 2, 3, 4 }
        };
    }

    public static IEnumerable<object[]> CopyToData_InvalidStartingIndex()
    {
        yield return new object[]
        {
            new SortedList<int>() { 1, 2, 3, 4 },
            7,
            new int[10],
        };
        yield return new object[]
        {
            new SortedList<int>() { 1, 2, 3, 4 },
            0,
            new int[3],
        };
        yield return new object[]
        {
            new SortedList<int>() {},
            -1,
            new int[] { 1, 2, 3, 4 },
        };
    }

    [Theory]
    [MemberData(nameof(CopyToData_ValidArgs))]
    public void CopyTo_ValidArgs_ExpectedArray<T>(SortedList<T> list, int index, T[] arr, T[] expectedArr) where T : IComparable<T>
    {
        list.CopyTo(arr, index);

        MyAssert.Equal(arr, expectedArr);
    }

    [Theory]
    [MemberData(nameof(CopyToData_InvalidStartingIndex))]
    public void CopyTo_InvalidStartingIndex_ArgumentException<T>(SortedList<T> list, int index, T[] arr) where T : IComparable<T>
    {
        Action action = () => list.CopyTo(arr, index);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void CopyTo_ArrayLengthListLengthAreZero_ReturnEmptyArray()
    {
        var list = new SortedList<int>();
        var arr = new int[] { };

        list.CopyTo(arr, 0);

        Assert.Empty(arr);
    }

    [Fact]
    public void CopyTo_ListIsEmpty_ArrayIsTheSame()
    {
        var list = new SortedList<int>();
        var arr = new int[] { 1, 2, 3, 4 };

        var cloneArr = (int[])arr.Clone();
        list.CopyTo(arr, 0);

        MyAssert.Equal(cloneArr, arr);
    }
}