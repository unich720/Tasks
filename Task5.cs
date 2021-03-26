using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    class Task5
    {
        static void Main(string[] args)
        {
            var persons = new List<Person>
            {
                new Person { Name = "Alexey", City = "Moscow" },
                new Person { Name = "BOBA", City = "Moscow" },
                new Person { Name = "Vladislav", City = "St. Peterburg" },
                new Person { Name = "Sergey", City = "Vladimir" },
            };
            var weathers = new List<Weather>
            {
                new Weather { Now = "Solar", City = "Moscow" },
                new Weather { Now = "Rainy", City = "Tallin" },
                new Weather { Now = "Solar", City = "Vladimir" },
                new Weather { Now = "Sun", City = "Moscow" },
            };
            var join = persons.MyFullJoin(weathers, x => x.City, y => y.City, (first, second, id) => new { id, first, second });
            foreach (var j in join)
            {
                Console.WriteLine($"{ j.first?.Name ?? "NULL" } | { j.id } | { j.second?.Now ?? "NULL" }");
            }
            Console.ReadKey();
        }
    }
    ///<summary>
    /// Класс погоды
    ///</summary>
    public class Weather
    {
        ///<summary>
        /// Свойство выражающее какая сейчас погода
        ///</summary>
        ///<value> 
        /// Свойство Now возвращающий значение в string 
        ///</value>
        public string Now { get; set; }
        ///<summary>
        /// Свойство выражающее город
        ///</summary>
        ///<value> 
        /// Свойство City возвращающий значение в string 
        ///</value>
        public string City { get; set; }
    }
    ///<summary>
    /// Класс человека
    ///</summary>
    public class Person
    {
        ///<summary>
        /// Свойство выражающее имя человека
        ///</summary>
        ///<value> 
        /// Свойство Name возвращающий значение в string 
        ///</value>
        public string Name { get; set; }
        ///<summary>
        /// Свойство выражающее город
        ///</summary>
        ///<value> 
        /// Свойство City возвращающий значение в string 
        ///</value>
        public string City { get; set; }
    }
    ///<summary>
    /// Свой класс расширений
    ///</summary>
    public static class MyExtensionTwo
    {
        ///<summary>
        /// Метод расширения MyFullJoin
        ///</summary>
        public static IEnumerable<TResult> MyFullJoin<TA, TB, TKey, TResult>(
        this IEnumerable<TA> a,
        IEnumerable<TB> b,
        Func<TA, TKey> selectKeyA,
        Func<TB, TKey> selectKeyB,
        Func<TA, TB, TKey, TResult> projection)
        {
            var alookup = a.ToLookup(selectKeyA);
            HashSet<TKey> allKey = new HashSet<TKey>();
            foreach (var item in b)
            {
                var selbkey = selectKeyB(item);
                if (alookup[selbkey].Any())
                {
                    foreach (var aitem in alookup[selbkey])
                    {
                        yield return projection(aitem, item, selbkey);
                    }
                }
                else
                {
                    yield return projection(default, item, selbkey);
                }
                allKey.Add(selbkey);
            }
            foreach (var aitem in a)
            {
                var selbkey = selectKeyA(aitem);
                if (!allKey.Contains(selbkey))
                {
                    yield return projection(aitem, default, selbkey);
                }
            }
        }
    }
}
