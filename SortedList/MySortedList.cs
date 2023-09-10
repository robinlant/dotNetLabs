using System.Collections;

namespace SortedList;

public class MySortedList<T> : ICollection<T> where T : IComparable<T>
{
    private MyInnerLinkedLIst<T> _myInnerLinkedLIst;

    public MySortedList()
    {
        _myInnerLinkedLIst = new MyInnerLinkedLIst<T>();
    }

    // Interface properties + methods
    public int Count { get; }
    public bool IsReadOnly { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Add(T item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }
}