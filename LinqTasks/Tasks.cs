﻿using LinqTasks.Extensions;
using LinqTasks.Models;

namespace LinqTasks;

public static partial class Tasks
{
    public static IEnumerable<Emp> Emps { get; set; }
    public static IEnumerable<Dept> Depts { get; set; }

    static Tasks()
    {
        Depts = LoadDepts();
        Emps = LoadEmps();
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
    /// </summary>
    public static IEnumerable<Emp> Task1()
    {
        return Emps
            .Where(e => e.Job == "Backend programmer");
    }

    /// <summary>
    ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
    /// </summary>
    public static IEnumerable<Emp> Task2()
    {
        return Emps
            .Where(e => e.Job == "Frontend programmer" && e.Salary > 1000)
            .OrderByDescending(e => e.Ename);
    }


    /// <summary>
    ///     SELECT MAX(Salary) FROM Emps;
    /// </summary>
    public static int Task3()
    {
        return Emps
            .Max(e => e.Salary);
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
    /// </summary>
    public static IEnumerable<Emp> Task4()
    {
        return Emps
            .Where(e => e.Salary == Task3());
    }

    /// <summary>
    ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
    /// </summary>
    public static IEnumerable<object> Task5()
    {
        return Emps.Select(e => new
        {
            Nazwisko = e.Ename,
            Praca = e.Job
        });
    }

    /// <summary>
    ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
    ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
    ///     Rezultat: Złączenie kolekcji Emps i Depts.
    /// </summary>
    public static IEnumerable<object> Task6()
    {
        return Emps
            .Join(Depts, e => e.Deptno, d => d.Deptno,
                (e, d) => new
                {
                    e.Ename,
                    e.Job,
                    d.Dname
                });
    }

    /// <summary>
    ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
    /// </summary>
    public static IEnumerable<object> Task7()
    {
        return Emps
            .GroupBy(e => e.Job)
            .Select(e => new
            {
                Praca = e.Key, // Key bierze z GropBy (Key -> wynik poprzedniego metoda rozszerzeń
                LiczbaPracownikow = e.Count()
            });
    }

    /// <summary>
    ///     Zwróć wartość "true" jeśli choć jeden
    ///     z elementów kolekcji pracuje jako "Backend programmer".
    /// </summary>
    public static bool Task8()
    {
        return Emps.Any(e => e.Job == "Backend programmer");
    }

    /// <summary>
    ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
    ///     ORDER BY HireDate DESC;
    /// </summary>
    public static Emp Task9()
    {
        return Emps
            .OrderByDescending(e => e.HireDate)
            .FirstOrDefault(e => e.Job == "Frontend programmer");
    }

    /// <summary>
    ///     SELECT Ename, Job, Hiredate FROM Emps
    ///     UNION
    ///     SELECT "Brak wartości", null, null;
    /// </summary>
    public static IEnumerable<object> Task10()
    {
        var emp = Emps
            .Select(e => new
            {
                Ename = e.Ename,
                Job = e.Job,
                HireDate = e.HireDate
            });

        var def = new[]
        {
            new
            {
                Ename = "Brak wartości",
                Job = (string)null,
                HireDate = (DateTime?)null
            }
        };

        return emp.Union(def);
    }

    /// <summary>
    ///     Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
    ///     1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
    ///     2. Chcemy zwrócić listę obiektów o następującej strukturze:
    ///     [
    ///     {name: "RESEARCH", numOfEmployees: 3},
    ///     {name: "SALES", numOfEmployees: 5},
    ///     ...
    ///     ]
    ///     3. Wykorzystaj typy anonimowe
    /// </summary>
    public static IEnumerable<object> Task11()
    {
        return Emps
            .GroupBy(emp => emp.Deptno)
            .Join(Depts,
                empJoin => empJoin.Key,
                deptJoin => deptJoin.Deptno,
                (emp, dept) => new
                {
                    name = dept.Dname,
                    numOfEmployees = emp.Count()
                }
            )
            .Where(res => res.numOfEmployees > 1);
    }

    /// <summary>
    ///     Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
    ///     Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
    ///     Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
    ///     Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
    /// </summary>
    public static IEnumerable<Emp> Task12()
    {
        IEnumerable<Emp> result = Emps.GetEmpsWithSubordinates();

        return result;
    }

    /// <summary>
    ///     Poniższa metoda powinna zwracać pojedyczną liczbę int.
    ///     Na wejściu przyjmujemy listę liczb całkowitych.
    ///     Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
    ///     Zakładamy, że zawsze będzie jedna taka liczba.
    ///     Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
    /// </summary>
    public static int Task13(int[] arr)
    {
        return arr
            .GroupBy(num => num)
            .Single(num => num.Count() % 2 != 0)
            .Key;
    }

    /// <summary>
    ///     Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
    ///     Posortuj rezultat po nazwie departament rosnąco.
    /// </summary>
    public static IEnumerable<Dept> Task14()
    {
        // Lista obiektów (Deptno, Count).
        var empCounts = Emps
            .GroupBy(emp => emp.Deptno)
            .Select(emp => new
            {
                Deptno = emp.Key,
                Count = emp.Count()
            });
        
        // Lista samych departamentów.
        var onlyDeptno = empCounts
            .Select(emp => emp.Deptno);

        // Lista samych numerów Deptno, które mają 5 osób.
        var fiveCounts = empCounts
            .Where(ec => ec.Count is 5)
            .Select(ec => ec.Deptno).ToList();
        
        // Zmapowanie numerów na Deptno.
        var mappedFiveDepts = Depts
            .Where(dept => fiveCounts.Contains(dept.Deptno));


        // Wybranie departamentów, których nie ma u żadnych pracowników (0).
        var noCountsDepts = Depts
            .Where(dept => !onlyDeptno.Contains(dept.Deptno));

        return mappedFiveDepts.Concat(noCountsDepts);
    }
}