using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QueueTask.Test
{
    public struct TestStruct
    {
        public int Value { get; set; }
    }

    public class TestClass
    {
        public int Value { get; set; }
    }

    ///<summary>
    /// ��������� ����������� ������ �������
    ///</summary>
    public interface IQueue<T>: IEnumerable<T>, IEnumerator<T>
    {
        public int Count { get; }
        public void Enqueue(T item);
        public T Dequeue();

    }
    ///<summary>
    /// ����� �������
    ///</summary>
    public class MyQueue<T> : IQueue<T>
    {
        ///<summary>
        /// ������� ��������� ���������
        ///</summary>
        ///<value>
        /// �������� Count ������������ � ���������� �������� � int 
        ///</value>
        public int Count { get; private set; }
        ///<summary>
        /// ������� ��������� ��������� ��� ������������ �������
        ///</summary>
        ///<value>
        /// �������� CountReset ������������ � ���������� �������� � int 
        ///</value>
        private int CountReset { get; set; }
        ///<summary>
        /// ���������� ������ �������
        ///</summary>
        ///<value>
        /// �������� _version ������������ � ���������� �������� � int 
        ///</value>
        private int _version { get; set; }
        ///<summary>
        /// ���������� ������ ������� ������ �������
        ///</summary>
        ///<value>
        /// �������� _versioninside ������������ � ���������� �������� � int 
        ///</value>
        private int _versioninside { get; set; }
        ///<summary>
        /// ���������� ����� ����������� ��������� �������
        ///</summary>
        private class Node<T>
        {
            ///<summary>
            /// �������� ��������
            ///</summary>
            public T item;
            ///<summary>
            /// ��������� �������
            ///</summary>
            public Node<T> next;
        }
        ///<summary>
        /// ����������� �������
        ///</summary>
        public MyQueue()
        {
            first = null;
            last = null;
            Count = 0;
        }
        ///<summary>
        /// �������� ������ �� �������
        ///</summary>
        public bool IsEmpty()
        {
            ThrowIfDisposed();
            return first == null;
        }
        ///<summary>
        /// ���������� �������� � �������
        ///</summary>
        public void Enqueue(T item)
        {
            ThrowIfDisposed();
            if (item == null) throw new ArgumentNullException(nameof(item));
            Node<T> oldlast = last;
            last = new Node<T>();
            last.item = item;
            last.next = null;
            if (IsEmpty())
                first = last;
            else 
                oldlast.next = last;
            Count++;
            _version++;
            _�urrentNote = first;
        }
        ///<summary>
        /// �������� �������� �� �������
        ///</summary>
        public T Dequeue()
        {
            ThrowIfDisposed();
            if (IsEmpty()) throw new InvalidOperationException("Queue underflow");
            T item = first.item;
            first = first.next;
            Count--;
            if (IsEmpty()) 
                last = null;
            _�urrentNote = first;
            _version++;
            return item;
        }
        ///<summary>
        /// ������������ Enumerator
        ///</summary>
        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();
            _versioninside = _version;
            return this;
        }
        ///<summary>
        /// ������������ Enumerator
        ///</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        ///<summary>
        /// ������� �� ��������� �������
        ///</summary>
        public bool MoveNext()
        {
            ThrowIfDisposed();
            if (_versioninside != _version) throw new InvalidOperationException("Queue underflow");
            if (_�urrentNote == last) return false;
            if (CountReset==0)
            {
                CountReset++;
                return true;
            }
            _�urrentNote = _�urrentNote.next;
            return true;
        }
        ///<summary>
        /// �������� �� ���������
        ///</summary>
        public void Reset()
        {
            ThrowIfDisposed();
            CountReset = 0;
            _�urrentNote = first;
        }
        ///<summary>
        /// ������� ��������
        ///</summary>
        public void Dispose()
        {
            _disposed = true;
            first = null;
        }
        ///<summary>
        /// �������� ����������� �� �������
        ///</summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("������� �����������");
            }
        }
        ///<summary>
        /// ������ ������� �������
        ///</summary>
        private Node<T> first;
        ///<summary>
        /// ��������� ������� �������
        ///</summary>
        private Node<T> last;
        ///<summary>
        /// ������� ������� ������� � T
        ///</summary>
        public T Current => _�urrentNote.item;
        ///<summary>
        /// ������� ������� ������� � Node
        ///</summary>
        private Node<T> _�urrentNote;
        ///<summary>
        /// ������� ������� ������� � Object
        ///</summary>
        object IEnumerator.Current => Current;
        ///<summary>
        /// ���������� �� ������
        ///</summary>
        private bool _disposed;
    }


    public class UnitTest1
    {

        private IQueue<T> InitQueue<T>() => new MyQueue<T>(); //Add queue 


        [Fact]
        [Trait("DisplayName", "Struct Dequeue empty")]
        public void StructQueue_Dequeue_Empty()
        {
            var q = InitQueue<TestStruct>();
            var ex = Record.Exception(() => q.Dequeue());
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Class Dequeue empty")]
        public void ClassQueue_Dequeue_Empty()
        {
            var q = InitQueue<TestClass>();
            var ex = Record.Exception(() => q.Dequeue());
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }


        [Fact]
        [Trait("DisplayName", "Struct Dequeue more than enque")]
        public void StructQueue_Dequeue_MoreThanExists()
        {
            var q = InitQueue<TestStruct>();

            foreach (var i in Enumerable.Range(0, 5))
                q.Enqueue(new TestStruct { Value = i });
            foreach (var _ in Enumerable.Range(0, 5))
                q.Dequeue();

            var ex = Record.Exception(() => q.Dequeue());
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Class Dequeue more than enque")]
        public void ClassQueue_Dequeue_MoreThanExists()
        {
            var q = InitQueue<TestClass>();

            foreach (var i in Enumerable.Range(0, 5))
                q.Enqueue(new TestClass { Value = i });
            foreach (var _ in Enumerable.Range(0, 5))
                q.Dequeue();

            var ex = Record.Exception(() => q.Dequeue());
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Struct Count Empty")]
        public void StructQueue_Count_Empty()
        {
            var q = InitQueue<TestStruct>();
            q.Count.Should().Be(0);
        }

        [Fact]
        [Trait("DisplayName", "Class Count Empty")]
        public void ClassQueue_Count_Empty()
        {
            var q = InitQueue<TestClass>();
            q.Count.Should().Be(0);
        }

        [Fact]
        [Trait("DisplayName", "Struct Count Not empty")]
        public void StructQueue_Count_NotEmpty()
        {
            var q = InitQueue<TestStruct>();

            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });
            foreach (var _ in Enumerable.Range(0, 7))
                q.Dequeue();
            q.Count.Should().Be(3);
        }

        [Fact]
        [Trait("DisplayName", "Class Count Not empty")]
        public void ClassQueue_Count_NotEmpty()
        {
            var q = InitQueue<TestClass>();

            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestClass { Value = i });
            foreach (var _ in Enumerable.Range(0, 7))
                q.Dequeue();
            q.Count.Should().Be(3);
        }


        [Fact]
        [Trait("DisplayName", "Struct Dequeue order")]
        public void StructQueue_Dequeue_Order()
        {
            var q = InitQueue<TestStruct>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });
            foreach (var i in Enumerable.Range(0, 10))
                q.Dequeue().Should().Be(new TestStruct { Value = i });
        }

        [Fact]
        [Trait("DisplayName", "Class Dequeue order")]
        public void ClassQueue_Dequeue_Order()
        {
            var testItem1 = new TestClass { Value = 1 };
            var testItem2 = new TestClass { Value = 2 };
            var testItem3 = new TestClass { Value = 3 };
            var testItem4 = new TestClass { Value = 4 };

            var q = InitQueue<TestClass>();

            q.Enqueue(testItem1);
            q.Enqueue(testItem2);
            q.Enqueue(testItem3);
            q.Enqueue(testItem4);

            var r1 = q.Dequeue();
            var r2 = q.Dequeue();
            var r3 = q.Dequeue();
            var r4 = q.Dequeue();

            r1.Should().BeSameAs(testItem1);
            r2.Should().BeSameAs(testItem2);
            r3.Should().BeSameAs(testItem3);
            r4.Should().BeSameAs(testItem4);
        }

        [Fact]
        [Trait("DisplayName", "Struct Foreach simple")]
        public void StructQueue_Foreach_Simple()
        {
            var q = InitQueue<TestStruct>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });

            int k = 0;
            foreach (var item in q)
                item.Should().Be(new TestStruct { Value = k++ });

        }

        [Fact]
        [Trait("DisplayName", "Class Foreach simple")]
        public void ClassQueue_Foreach_Simple()
        {
            var testItem1 = new TestClass { Value = 1 };
            var testItem2 = new TestClass { Value = 2 };
            var testItem3 = new TestClass { Value = 3 };
            var testItem4 = new TestClass { Value = 4 };

            var q = InitQueue<TestClass>();

            q.Enqueue(testItem1);
            q.Enqueue(testItem2);
            q.Enqueue(testItem3);
            q.Enqueue(testItem4);

            var arr = new[]
            {
                testItem1,testItem2,testItem3,testItem4
            };

            int k = 0;
            foreach (var item in q)
                item.Should().BeSameAs(arr[k++]);

        }

        [Fact]
        [Trait("DisplayName", "Struct Foreach invalidation Dequeue")]
        public void StructQueue_Foreach_InvalidationDequeue()
        {
            var q = InitQueue<TestStruct>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });
            void a()
            {
                foreach (var item in q)
                    q.Dequeue();
            }
            var ex = Record.Exception(a);
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Class Foreach invalidation Dequeue")]
        public void ClassQueue_Foreach_InvalidationDequeue()
        {
            var q = InitQueue<TestClass>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestClass { Value = i });
            void a()
            {
                foreach (var item in q)
                    q.Dequeue();
            }
            var ex = Record.Exception(a);
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Struct Foreach invalidation Enqueue")]
        public void StructQueue_Foreach_InvalidationEnqueue()
        {
            var q = InitQueue<TestStruct>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });
            void a()
            {
                foreach (var item in q)
                    q.Enqueue(default);
            }
            var ex = Record.Exception(a);
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Class Foreach invalidation Enqueue")]
        public void ClassQueue_Foreach_InvalidationEnqueue()
        {
            var q = InitQueue<TestClass>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestClass { Value = i });
            void a()
            {
                foreach (var item in q)
                    q.Enqueue(default);
            }
            var ex = Record.Exception(a);
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        [Trait("DisplayName", "Struct Foreach Reset")]
        public void StructQueue_Iterator_Reset()
        {
            var q = InitQueue<TestStruct>();
            foreach (var i in Enumerable.Range(0, 10))
                q.Enqueue(new TestStruct { Value = i });
            int k = 0;
            var iterator = q.GetEnumerator();
            while (iterator.MoveNext())
                iterator.Current.Should().Be(new TestStruct { Value = k++ });
            k.Should().Be(10);
            k = 0;
            iterator.Reset();
            while (iterator.MoveNext())
                iterator.Current.Should().Be(new TestStruct { Value = k++ });
            k.Should().Be(10);
        }

        [Fact]
        [Trait("DisplayName", "Class Foreach Reset")]
        public void ClassQueue_Iterator_Reset()
        {
            var q = InitQueue<TestClass>();

            var arr = new[]
            {
                new TestClass{ Value = 1 },
                new TestClass{ Value = 2 },
                new TestClass{ Value = 3 },
            };

            foreach (var i in Enumerable.Range(0, 3))
                q.Enqueue(arr[i]);
            int k = 0;
            var iterator = q.GetEnumerator();
            while (iterator.MoveNext())
                iterator.Current.Should().BeSameAs(arr[k++]);
            k.Should().Be(3);
            k = 0;
            iterator.Reset();
            while (iterator.MoveNext())
                iterator.Current.Should().BeSameAs(arr[k++]);
            k.Should().Be(3);
        }
    }
}