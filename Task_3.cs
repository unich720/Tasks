using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    class Task_3
    {
        abstract class Base
        {
            public Base(int i)
            {

            }
        }
        class Foo_1 : Base
        {
            public Foo_1(string s) : base(s != null ? s.Length : throw new ArgumentNullException())
            {

            }
        }

        class Foo_2 : Base
        {
            public Foo_2(string s) : base(Foo_2.CheckNull(s))
            {

            }
            static int CheckNull(string s)
            {
                if (s != null)
                    return s.Length;
                else
                    throw new ArgumentNullException();
            }
        }
        static void Main(string[] args)
        {
            string s = null;
            var f_1 = new Foo_1(s);
            var f_2 = new Foo_2(s);

        }
    }
}
