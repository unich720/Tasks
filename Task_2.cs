using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    ///<summary>
    /// Интерфейс транспорта 
    ///</summary>
    public interface ITransport
    {
        ///<summary>
        /// Метод воспроизводящий действие с определенным видом транспорта
        ///</summary>
        void Move();
    }
    ///<summary>
    /// Класс описывающий лодку
    ///</summary>
    public class Boat : ITransport
    {
        ///<summary>
        /// Метод воспроизводящий действие с лодкой
        ///</summary>
        public void Move()
        {
            Console.WriteLine("плыть");
        }
    }
    ///<summary>
    /// Класс описывающий машину
    ///</summary>
    public class Car : ITransport
    {
        ///<summary>
        /// Метод воспроизводящий действие с машиной
        ///</summary>
        public void Move()
        {
            Console.WriteLine("ехать");
        }
    }
    ///<summary>
    /// Класс описывающий самолет
    ///</summary>
    public class Аirplane : ITransport
    {
        ///<summary>
        /// Метод воспроизводящий действие с самолетом
        ///</summary>
        public void Move()
        {
            Console.WriteLine("лететь");
        }
    }
    public static void Main(string[] args)
    {
        var obj = new Boat();
        obj.Move();

    }
}
