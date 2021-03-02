using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    class Task_2
    {
        interface ITransport
        {
            void Move();
        }
        class Boat : ITransport
        {
            public void Move()
            {
                Console.WriteLine("плыть");
            }
        }
        class Car : ITransport
        {
            public void Move()
            {
                Console.WriteLine("ехать");
            }
        }
        class Аirplane : ITransport
        {
            public void Move()
            {
                Console.WriteLine("лететь");
            }
        }
        static void Main(string[] args)
        {
            var obj = new Boat();
            obj.Move();

        }
    }
}
