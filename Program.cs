using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class Person
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Weather
    {
        public string Now { get; set; }
        public string City { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var persons = new List<Person>
            {
                new Person { Name = "Alexey", City = "Moscow" },
                new Person { Name = "Max", City = "St. Peterburg" },
                new Person { Name = "Sergey", City = "Sochi" },
                new Person { Name = "Jon", City = "Irkutsk" },
                new Person { Name = "Masha", City = "Irkutsk" }
            };
            var weathers = new List<Weather>
            {
                new Weather { Now = "Solar", City = "Moscow" },
                new Weather { Now = "Solar-Cloudy", City = "Moscow" },
                new Weather { Now = "Rainy", City = "Tallin" },
                new Weather { Now = "Hot", City = "Sochi" },
                new Weather { Now = "Cold", City = "Irkutsk" }
            };

            // LEFT JOIN SQL-like syntax
            var result_1 =
                from person in persons
                join weather in weathers on person.City equals weather.City into result
                from item in result.DefaultIfEmpty()
                select new { person.Name, Now = item == null ? "NULL" : item.Now };

            // LEFT JOIN syntax of extension methods
            var result_2 = persons.GroupJoin(weathers, person => person.City, weather => weather.City, (person, collection) => new
            {
                person = person,
                collection = collection
            }).SelectMany(x => x.collection.DefaultIfEmpty<Weather>(), (x, item) => new
            {
                Name = x.person.Name,
                Now = item == null ? "NULL" : item.Now
            });

            // INNER JOIN SQL-like syntax
            var result_3 =
               from person in persons
               join weather in weathers on person.City equals weather.City
               select new { person.Name, weather.Now };

            //INNER JOIN syntax of extension methods
            var result_4 = persons.Join(weathers, x => x.City, x => x.City, (x, y) => new { x.Name, y.Now });

            // Console.WriteLine("LEFT JOIN: " + result_1.SequenceEqual(result_2));
            // Console.WriteLine("INNER JOIN: " + result_3.SequenceEqual(result_4));

            foreach (var item in result_1)
            {
                Console.WriteLine(item.Name + " " + item.Now);
            }

            Console.ReadKey();
        }
    }
}