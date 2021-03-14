using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    ///<summary>
    /// Базовый абстрактный класс
    ///</summary>
    public abstract class Base
    {
        ///<summary>
        /// Пустой конструктор
        ///</summary>
        ///<param name="i">
        /// Простое число 
        ///</param>
        public Base(int i)
        {

        }
    }
    ///<summary>
    /// Класс с примером 1
    ///</summary>
    public class Foo_1 : Base
    {
        ///<summary>
        /// Коструктор класса Foo 1 вызывающий коструктор базового класса с проверкой на null
        ///</summary>
        ///<param name="s">
        /// Простая строка
        ///</param>
        ///<exception cref="System.ArgumentNullException">
        /// Строка может прийти пустой
        ///</exception>
        public Foo_1(string s) : base(s != null ? s.Length : throw new ArgumentNullException())
        {

        }
    }
    ///<summary>
    /// Класс с примером 2
    ///</summary>
    public class Foo_2 : Base
    {
        ///<summary>
        /// Коструктор класса Foo 2 вызывающий коструктор базового класса,
        /// который вызывает статический класс проверки строки на null
        ///</summary>
        ///<param name="s">
        /// Простая строка
        ///</param>
        public Foo_2(string s) : base(CheckNull(s))
        {

        }
        ///<summary>
        /// Статический метод проверки строки на null
        ///</summary>
        ///<param name="s">
        /// Простая строка
        ///</param>
        ///<exception cref="System.ArgumentNullException">
        /// Строка может прийти пустой
        ///</exception>
        static int CheckNull(string s)
        {
            if (s != null)
                return s.Length;
            else
                throw new ArgumentNullException();
        }
    }
    public static void Main(string[] args)
    {
        string s = null;
        var f_1 = new Foo_1(s);
        var f_2 = new Foo_2(s);

    }
}
