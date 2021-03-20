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

    public interface IQueue<T>: IEnumerable<T>, IEnumerator<T>
    {
        public int Count { get; }
        public void Enqueue(T item);
        public T Dequeue();

    }

    public class MyQueue<T> : IQueue<T>
    {
        private Node<T> first;  
        private Node<T> last;
        public int Count { get; private set; }
        private int CountReset { get; set; }
        private int _version { get; set; }
        private int _versioninside { get; set; }
        public T Current => CurrentNote.item;
        private Node<T> CurrentNote;

        object IEnumerator.Current => Current;

        private class Node<T>
        {
            public T item;
            public Node<T> next;
        }
        public MyQueue()
        {
            first = null;
            last = null;
            Count = 0;
        }
        public bool IsEmpty()
        {
            return first == null;
        }
        public void Enqueue(T item)
        {
            Node<T> oldlast = last;
            last = new Node<T>();
            last.item = item;
            last.next = null;
            if (IsEmpty()) first = last;
            else oldlast.next = last;
            Count++;
            _version++;
            CurrentNote = first;
        }

        public T Dequeue()
        {
            if (IsEmpty()) throw new InvalidOperationException("Queue underflow");
            T item = first.item;
            first = first.next;
            Count--;
            if (IsEmpty()) last = null;
            CurrentNote = first;
            _version++;
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            _versioninside = _version;
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (_versioninside != _version) throw new InvalidOperationException("Queue underflow");
            if (CurrentNote==last) return false;
            if (CountReset==0)
            {
                CountReset++;
                return true;
            }
            CurrentNote = CurrentNote.next;
            return true;
        }

        public void Reset()
        {
            CountReset = 0;
            CurrentNote = first;
        }

        public void Dispose()
        {
        }
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