using System.Collections;
using System.Diagnostics;

namespace SortedList;

internal class MyInnerLinkedLIst<T> : ICollection<T>
{
    private MyNode<T>? _head;

    public int Count { get; private set; }
    public bool IsReadOnly { get; } = false;

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        // head is null
        if (_head == null)
        {
            _head = new MyNode<T>(item);
            IncrementCount();
            return;
        }

        var current = _head;
        while (current.Next != null)
        {
            current = current.Next;
        }

        current.Next = new MyNode<T>(item);

    }

    public void Clear()
    {
        _head = null;
    }

    public bool Contains(T item)
    {
        var current = _head;

        while (current != null)
        {
            Debug.Assert(current.Item != null, "current != null");
            if (current.Item.Equals(item)) return true;
            current = current.Next;
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    private void IncrementCount()
    {
        Count++;
    }

    private void DecrementCount()
    {
        Count--;
    }
}

internal class MyNode<T>
{
    public T Item { get; set; }
    public MyNode<T>? Next { get; set; }

    public MyNode(T value)
    {
        Item = value;
    }
}