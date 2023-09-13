using System.Collections;

namespace SortedList;

public class SortedList<T> : ICollection<T> where T : IComparable<T>
{
    private MyNode<T>? _head;
    private MyNode<T>? _tail;
    public bool IsReadOnly { get; } = false;
    public int Count { get; private set; } // length of the list
    public int Version { get; private set; }

    public event EventHandler<ItemEventArgs<T>>? ItemAdded;
    public event EventHandler<ItemEventArgs<T>>? ItemRemoved;
    public event EventHandler? ListCleared;

    public IEnumerator<T> GetEnumerator()
    {
        IEnumerator <T> enumerator = new MyEnumerator(_head, this);
        return enumerator;
    }

    public IEnumerator<T> Reversed()
    {
        var listStarterVersion = Version;
        var current = _tail;
        while (current != null)
        {
            if (listStarterVersion != Version)
            {
                throw new InvalidOperationException("Collection was modified");
            }

            yield return current.Item;
            current = current.Prev;
        }
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

        if (Contains(item))
            throw new ArgumentException("Item is already in the list");

        if (_head == null)
        {
            _head = new MyNode<T>(item);
            _tail = _head;
            AfterItemAdded(item);

            return;
        }

        var nodeToInsertAfter = FindNodeToInsertAfter(item);

        if (nodeToInsertAfter == null)
        {
            _head.Prev = new MyNode<T>(item) {Next = _head};
            _head = _head.Prev;
        } else
        {
           InsertAfter(nodeToInsertAfter, item);
        }
        AfterItemAdded(item);
    }

    public bool Remove(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException($"{typeof(T)} {nameof(item)} is null");
        }

        var searchedNode = FindNodeByItem(item);
        if (searchedNode == null) return false;

        RemoveNode(searchedNode);
        AfterItemRemoved(item);

        return true;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        InvokeListCleared();
        ResetVersion();
        ResetCount();
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

    public bool Contains(T item)
    {
        var current = _head;
        while (current != null)
        {
            var compareInt = current.Item.CompareTo(item);
            if (compareInt > 0) return false;
            if (compareInt == 0) return true;
            current = current.Next;
        }

        return false;
    }

    private void InsertAfter(MyNode<T> node, T item)
    {
        var newNode = new MyNode<T>(item) { Prev = node, Next = node.Next};

        if (_tail == node)
        {
            node.Next = newNode;
            _tail = newNode;

            return;
        }

        node.Next!.Prev = newNode;
        node.Next = newNode;
    }

    private MyNode<T>? FindNodeToInsertAfter(T item)
    {
        if (_head == null || _head.Item.CompareTo(item) > 0)
            return null;

        var current = _head;

        while (current.Next != null)
        {
            if (current.Item.CompareTo(item) == 0)
                throw new ArgumentException("Item is already in the list");
            if (current.Item.CompareTo(item) > 0)
                return current;

            current = current.Next;
        }

        return current;
    }

    private MyNode<T>? FindNodeByItem(T item)
    {
        var current = _head;
        while (current != null)
        {
            var compareInt = current.Item.CompareTo(item);
            if (compareInt > 0) return null;
            if (compareInt == 0) return current;
            current = current.Next;
        }

        return null;
    }

    private void RemoveNode(MyNode<T> node)
    {
        if (node == _head && node == _tail)
        {
            _head = null;
            _tail = null;

            return;
        } if (node == _head)
        {
            _head = _head.Next;
            _head!.Prev = null;

            return;
        } if (node == _tail)
        {
            _tail = _tail.Prev;
            _tail!.Next = null;

            return;
        }

        node.Next!.Prev = node.Prev;
        node.Prev!.Next = node.Next;
    }

    private void AfterItemRemoved(T item)
    {
        InvokeItemRemoved(item);
        DecrementCount();
        if (_head == null)
        {
            InvokeListCleared();
            ResetVersion();
            return;
        }

        UpdateVersion();
    }

    private void AfterItemAdded(T item)
    {
        InvokeItemAdded(item);
        UpdateVersion();
        IncrementCount();
    }

    private void InvokeItemAdded(T item) => ItemAdded?.Invoke(this,new ItemEventArgs<T>(item));

    private void InvokeItemRemoved(T item) => ItemRemoved?.Invoke(this,new ItemEventArgs<T>(item));

    private void InvokeListCleared() => ListCleared?.Invoke(this, EventArgs.Empty);

    private void UpdateVersion() => Version++;

    private void ResetVersion() => Version = 0;

    private void IncrementCount() => Count++;

    private void DecrementCount() => Count--;

    private void ResetCount() => Count = 0;

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
                return _current.Item;
            }
        }

        public bool MoveNext()
        {
            CheckVersion();
            if (_current == null)
            {
                if (_head == null)
                    return false;

                _current = _head;
                return true;
            }

            _current = _current.Next;
            return _current != null;
        }

        public void Reset()
        {
             throw new NotSupportedException("Not implemented due to safety reasons");
            // If changes are made to the collection,
            // such as adding, modifying, or deleting elements,
            // the behavior of Reset is undefined.
            // https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset?view=net-6.0
        }

        object IEnumerator.Current => Current;

        public void Dispose() { }

        private void CheckVersion()
        {
            if (_listStarterVersion != _list.Version)
            {
                throw new InvalidOperationException("Collection was modified");
            }
        }
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