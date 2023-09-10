using System.Collections;
using System.Diagnostics;

namespace SortedList;

internal class MyInnerLinkedLIst<T> : ICollection<T>
{
    private MyNode<T>? _head;

    public int Count { get; private set; }
    public bool IsReadOnly { get; } = false;
    public int Version { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    // public IEnumerator<T> GetEnumerator()
    // {
    //     MyNode<T>? current = _head;
    //     while (current != null)
    //     {
    //         yield return current.Item;
    //         current = current.Next;
    //     }
    // } 2nd variant


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
            if (current.Item!.Equals(item)) return true;
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

    // Enumerator
    private class MyEnumerator : IEnumerator<T>
    {
        private MyNode<T>? _current;
        private MyNode<T>? _head;

        private readonly MyInnerLinkedLIst<T> _list;
        private int _listStarterVersion;

        public MyEnumerator(MyNode<T>? head, MyInnerLinkedLIst<T> list)
        {
            _current = null;
            _head = head;
            _list = list;
            _listStarterVersion = _list.Version;
        }

        public T Current
        {
            get
            {
                CheckVersion();
                if (_current == null)
                    throw new InvalidOperationException("Enumeration has not been started ot it is already finished");
                return _current.Item;
            }
        }

        private void CheckVersion()
        {
            if (_listStarterVersion != _list.Version)
            {
                throw new InvalidOperationException("Collection was modified");
            }
        }

        public bool MoveNext()
        {
            CheckVersion();
            if (_current == null)
            {
                if (_head == null)
                    return false; // list is empty

                _current = _head;
                return true;
            }

            _current = _current.Next;
            return _current != null;
        }

        public void Reset()
        {
            throw new NotSupportedException("Reset is not implemented due to safety reasons");
            // If changes are made to the collection,
            // such as adding, modifying, or deleting elements,
            // the behavior of Reset is undefined.
            // https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset?view=net-6.0
        }

#nullable disable
        object IEnumerator.Current => Current;
#nullable enable

        public void Dispose() { }
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