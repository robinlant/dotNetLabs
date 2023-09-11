using System.Collections;

namespace SortedList;

internal class SortedList<T> : ICollection<T> where T : IComparable<T>
{
    private MyNode<T>? _head;
    private MyNode<T>? _tail;
    public bool IsReadOnly { get; } = false;
    public int Count { get; private set; } // length of the list
    public int Version { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        IEnumerator <T> enumerator = new MyEnumerator(_head, this);
        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException($"{typeof(T)} {nameof(item)} is null");
        }

        // head is null
        if (_head == null)
        {
            _head = new MyNode<T>(item);
            _tail = _head;
            IncrementCount();
            UpdateVersion();

            return;
        }
        // item < head
        if (_head.Item.CompareTo(item) > 0)
        {
            var node = new MyNode<T>(item) { Next = _head };
            _head.Prev = node;
            _head = node;
            IncrementCount();
            UpdateVersion();

            return;
        }
        // cycles till the end
        var current = _head;
        while (current.Next != null)
        {
            if (current.Next.Item.CompareTo(item) > 0)
            {
                var node = new MyNode<T>(item) { Next = current.Next, Prev = current};
                current.Next.Prev = node;
                current.Next = node;
                IncrementCount();
                UpdateVersion();

                return;
            }

            current = current.Next;
        }

        _tail = new MyNode<T>(item) { Prev = current };
        current.Next = _tail;
        IncrementCount();
        UpdateVersion();
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        ResetVersion();
        ResetCount();
    }

    public bool Contains(T item)
    {
        var current = _head;
        while (current != null)
        {
            var temp = current.Item.CompareTo(item);
            if (temp < 0) return false; // further numbers are bigger
            if (temp == 0) return true;
            current = current.Next;
        }

        return false;
    }

    private MyNode<T>? FindNodeByItem(T item)
    {
        var current = _head;
        while (current != null)
        {
            var temp = current.Item.CompareTo(item);
            if (temp < 0) return null; // further numbers are bigger
            if (temp == 0) return current;
            current = current.Next;
        }

        return null;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException($"Array {nameof(array)} is null");
        }

        if (array.Length - arrayIndex < Count )
        {
            throw new ArgumentException("Not enough space. Count > array length - starting index");
        }

        if (arrayIndex < 0 || arrayIndex >= array.Length)
        {
            throw new ArgumentException($"Invalid Argument. arrayIndex = {arrayIndex}. It has to be greater than zero and smaller than array length");
        }

        var i = 0;
        foreach (var item in this)
        {
            array[arrayIndex + i] = item;
            i++;
        }
    }

    public bool Remove(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException($"{typeof(T)} {nameof(item)} is null");
        }

        var searchedNode = FindNodeByItem(item);
        if (searchedNode == null) return false;

        if (searchedNode == _head && searchedNode == _tail)
        {
            _head = null;
            _tail = null;
            ResetVersion();
            ResetCount();

            return true;
        } if (searchedNode == _head)
        {
            _head = _head.Next;
            _head!.Prev = null;
            DecrementCount();
            UpdateVersion();

            return true;
        } if (searchedNode == _tail)
        {
            _tail = _tail.Prev;
            _tail!.Next = null;
            DecrementCount();
            UpdateVersion();

            return true;
        }

        searchedNode.Next!.Prev = searchedNode.Prev;
        searchedNode.Prev!.Next = searchedNode.Next;
        DecrementCount();
        UpdateVersion();

        return true;
    }

    // version and length control methods
    private void UpdateVersion() => Version++;

    private void ResetVersion() => Version = 0;

    private void IncrementCount() => Count++;

    private void DecrementCount() => Count--;

    private void ResetCount() => Count = 0;

    // Enumerator
    private class MyEnumerator : IEnumerator<T>
    {
        private MyNode<T>? _current;
        private MyNode<T>? _head;

        private readonly SortedList<T> _list;
        private readonly int _listStarterVersion;

        public MyEnumerator(MyNode<T>? head, SortedList<T> list)
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
                return _current.Item; // THIS ENSURES THAT USER WILL WORK WITH T INSTEAD OF THE NODE
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
    private class MyNode<TU>
    {
        public TU Item { get; set; }
        public MyNode<TU>? Next { get; set; }
        public MyNode<TU>? Prev { get; set; }

        public MyNode(TU value)
        {
            Item = value;
        }
    }
}