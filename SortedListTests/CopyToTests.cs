namespace SortedListTests;

public class CopyToTests
{
    public static IEnumerable<object[]> DataCopyTo()
    {
        yield return new object[]
        {
            new SortedList<int>() { 1, 2, 3, 4 },
            1,
            new int[5],
            new int[] { 0, 1, 2, 3, 4 }
        };
    }

    [Theory]
    [MemberData(nameof(DataCopyTo))]
    public void CopyTo_ValidArgs_ExpectedArray<T>(SortedList<T> list, int index, T[] arr, T[] expectedArr) where T : IComparable<T>
    {
        list.CopyTo(arr, index);

        Helper.AssertEnumerablesEqual(arr, expectedArr);
    }

    [Fact]
    public void CopyTo_InvalidStartingIndex_ArgumentException()
    {
        var list = new SortedList<int>() { 1, 2, 3, 4};
        var arrOne = new int[3];
        var arrTwo = new int[10];


        Assert.Throws<ArgumentException>(() => list.CopyTo(arrOne, 0));
        Assert.Throws<ArgumentException>(() => list.CopyTo(arrTwo, 7));
    }

    [Fact]
    public void CopyTo_ArrayLengthListLengthAreZero_ReturnEmptyArray()
    {
        var list = new SortedList<int>() { };
        var arr = new int[] { };

        list.CopyTo(arr, 0);

        Assert.Empty(arr);
    }

    [Fact]
    public void CopyTo_NegativeIndex_ArgumentException()
    {
        var list = new SortedList<int>();
        var arr = new int [5];

        Assert.Throws<ArgumentException>(() => list.CopyTo(arr, -5));
    }

    [Fact]
    public void CopyTo_ListIsEmpty_ArrayIsTheSame()
    {
        var list = new SortedList<int>();
        var arr = new int[] { 1, 2, 3, 4 };

        var cloneArr = (int[])arr.Clone();
        list.CopyTo(arr, 0);

        Helper.AssertEnumerablesEqual(cloneArr, arr);
    }
}