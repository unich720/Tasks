using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    class Task_4
    {
        static void Main(string[] args)
        {
            foreach (var item in GetEnumeration().MyWhere(x => x % 2 == 0).MyTake(5))
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
        ///<summary>
        /// Метод вызова бесконечного цикла для увеличения i
        ///</summary>
        public static IEnumerable<int> GetEnumeration()
        {
            int i = 0;
            while (true)
            {
                yield return i++;
            }
        }
    }
    ///<summary>
    /// Собственный итератор для where
    ///</summary>
    public class MyWhereIterator<T> : IEnumerable<T>, IEnumerator<T>
    {
        ///<summary>
        /// Итератор 
        ///</summary>
        private IEnumerator<T> _iterator;
        ///<summary>
        /// Фунция типа T с возращаемым значением bool
        ///</summary>
        private Func<T, bool> _func;
        ///<summary>
        /// Лямбда возвращающая текущее значение
        ///</summary>
        public T Current => _iterator.Current;
        ///<summary>
        /// Лямбда возвращающая текущее значение в object
        ///</summary>
        object IEnumerator.Current => Current;
        ///<summary>
        /// Коструктор класса MyWhereIterator устанавливающий начальные значениия
        ///</summary>
        ///<param name="source">
        /// Источник типа IEnumerable
        ///</param>
        ///<param name="func">
        /// Делагат принимающий T и возвращающий bool
        ///</param>
        public MyWhereIterator(IEnumerable<T> source, Func<T, bool> func)
        {
            _iterator = source.GetEnumerator();
            _func = func;
        }
        ///<summary>
        /// Метод освобождения памяти
        ///</summary>
        public void Dispose()
        {
            _iterator.Dispose();
        }
        ///<summary>
        /// Метод возвращающий итератор
        ///</summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }
        ///<summary>
        /// Метод перемещения между значениями
        ///</summary>
        public bool MoveNext()
        {
            while (_iterator.MoveNext())
            {
                if (_func(_iterator.Current))
                {
                    return true;
                }
            }
            return false;
        }
        ///<summary>
        /// Метод приводящий итератор в начальное значение
        ///</summary>
        public void Reset()
        {
            _iterator.Reset();
        }
        ///<summary>
        /// 
        ///</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
    ///<summary>
    /// Собственный итератор для Take
    ///</summary>
    public class MyTakeIterator<T> : IEnumerable<T>, IEnumerator<T>
    {
        ///<summary>
        /// Итератор 
        ///</summary>
        private IEnumerator<T> _iterator;
        ///<summary>
        /// Лямбда возвращающая текущее значение
        ///</summary>
        public T Current => _iterator.Current;
        ///<summary>
        /// Лямбда возвращающая текущее значение в object
        ///</summary>
        object IEnumerator.Current => Current;
        ///<summary>
        /// Коструктор класса MyWhereIterator устанавливающий начальные значениия
        ///</summary>
        ///<param name="source">
        /// Источник типа IEnumerable
        ///</param>
        ///<param name="count">
        /// Переменная принимающая сколько надо выбрать значений
        ///</param>
        public MyTakeIterator(IEnumerable<T> source, int count)
        {
            _iterator = source.GetEnumerator();
            _count = count;
        }
        ///<summary>
        /// Метод освобождения памяти
        ///</summary>
        public void Dispose()
        {
            _iterator.Dispose();
        }
        ///<summary>
        /// Метод возвращающий итератор
        ///</summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }
        ///<summary>
        /// Метод перемещения между значениями
        ///</summary>
        public bool MoveNext()
        {
            return (_iterator.MoveNext() && _tempcount++ != _count);
        }
        ///<summary>
        /// Метод приводящий итератор в начальное значение
        ///</summary>
        public void Reset()
        {
            _tempcount = 0;
            _iterator.Reset();
        }
        ///<summary>
        /// 
        ///</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
        ///<summary>
        /// Количество значений всего
        ///</summary>
        private int _count;
        ///<summary>
        /// Текущее количетсво значений
        ///</summary>
        private int _tempcount;
    }
    ///<summary>
    /// Класс с своими расширениями
    ///</summary>
    public static class MyExtension
    {
        ///<summary>
        /// Метод расширения возвращающий итератор со значениями заданым условием делегата predicate
        ///</summary>
        ///<param name="source">
        /// Источник типа IEnumerable
        ///</param>
        ///<param name="predicate">
        /// Делагат принимающий TSource и возвращающий bool
        ///</param>
        ///<exception cref="System.ArgumentNullException">
        /// Source и делегат может быть пустыми
        ///</exception>
        public static IEnumerable<TSource> MyWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("Source null");
            if (predicate == null) throw new ArgumentNullException("Predicate null");
            return new MyWhereIterator<TSource>(source, predicate);
        }
        ///<summary>
        /// Метод приводящий итератор в начальное значение
        ///</summary>
        ///<param name="source">
        /// Источник типа IEnumerable
        ///</param>
        ///<param name="count">
        /// Переменная принимающая сколько надо выбрать значений
        ///</param>
        ///<exception cref="System.ArgumentNullException">
        /// Source и count может быть пустыми
        ///</exception>
        public static IEnumerable<TSource> MyTake<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("Source null");
            if (count == default) throw new ArgumentNullException("Count = 0");
            return new MyTakeIterator<TSource>(source, count);
        }
    }

}

