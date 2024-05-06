using System.ComponentModel.DataAnnotations;
using LinqTasks.Models;

namespace LinqTasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("======= ZAD X =======");

        IEnumerable<Emp> result = Tasks.Task1();

        foreach (Emp emp in result)
        {
            Console.WriteLine(emp);
        }

        var enumerable = Tasks.Task11().ToList();
        Console.Out.WriteLine(enumerable.Count);
        foreach (Object o in enumerable)
        {
            Console.Out.WriteLine(o);
        }
    }
}