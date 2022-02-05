using System.Diagnostics;

Console.WriteLine("Hello, World!");

//f is declared as MathOp delegate and it is a pointer
//to a method which fulfills the contract of the delegate
MathOp f;
f = Subtract;//f is initialized 
Console.WriteLine(f(20, 3));

MathOp g = Add; //Declaration and initialization
Console.WriteLine(g(20, 3));

//The calculation strategy is passed to the method below by using delegate parameter in the method
CalculateAndPrint(23, 3, Subtract);

//Below an anonymous method is passed as calculation strategy
//keyword delegate defines an anonymous method herein, fat arrow syntax can be used instead to 
//form a lambda expression
CalculateAndPrint(12, 3, delegate (int x, int y) { return x * y; });
CalculateAndPrint(12, 3, (x, y) => x * y);

//Example for using generic method
CalculateAndPrintGeneric(true, false, (x, y) => x || y);
//Or
CalculateAndPrintGeneric<Boolean>(true, false, (x, y) => x || y);
//Or
CalculateAndPrintGeneric<Boolean>(true, false, (bool x, bool y) => x || y);

//The generic method below uses a generic inbuilt delegate (Func)
CalculateAndPrintGenericFunc("A", "B", (x, y) => x + y);


void CalculateAndPrint(int x, int y, MathOp f)
{
    var result = f(x, y);
    Console.WriteLine(result);
}

//Think of generic as a way to pass type to method parameters (type parameters)
//A generic delegate is used in method below 
void CalculateAndPrintGeneric<T>(T x, T y, MathOpGeneric<T> f)
{
    var result = f(x, y);
    Console.WriteLine(result);
}

//Use generic inbuilt delegate Func in method below
void CalculateAndPrintGenericFunc<T>(T x, T y, Func<T, T, T> f)
{
    var result = f(x, y);
    Console.WriteLine(result);
}

int Add(int x, int y)
{
    return x + y;
}

int Subtract(int x, int y)
{
    return x - y;
}

//*****************************************************************************************************

List<Hero> heroes = new List<Hero>
{
    new("Wade", "Wilson", "Deadpool", false),
    new(string.Empty, string.Empty, "Homelander", true),
    new("Bruce", "Wayne", "Batman", false),
    new(string.Empty, string.Empty, "Stormfront", true)
};

//Use filter 
var filter1 = FilterHeros(heroes, h => !string.IsNullOrEmpty(h.LastName));
var heroesLastNameProvided = filter1.Select(s => s.HeroName).ToList();
var result1= string.Join(", ", heroesLastNameProvided);
Console.WriteLine(result1);

//Use generic filter
var filter2 = Filter<Hero>(heroes, h => h.CanFly);
var heroesWhoCanFly= filter2.Select(s => s.HeroName).ToList();
var result2 = string.Join(", ", heroesWhoCanFly);
Console.WriteLine(result2);

//The advantage of using a generic filter
Filter(new[] { "Homelander", "The Deep", "Stormfront" }, hn => hn.StartsWith("H"));
Filter(new[] { 1, 2, 3, 4, 5 }, n => n % 2 == 0);

// Func<T,bool> f is a predicate delegate since it gets data structure as parameter and returns boolean
//IEnumerable is for creating iterable collections
//(iteration means read only forward only operation on collection)

// Linq: Where implementation
IEnumerable<T> Filter<T>(IEnumerable<T> items, Func<T, bool> f)
{
    foreach (var item in items)
    {
        if (f(item))
        {
            yield return item;
        }
    }
}

List<Hero> FilterHeros(List<Hero> heros, FilterGeneric<Hero> f)//FilterDelegate f
{
    var filteredHeros = new List<Hero>();

    foreach (var hero in heros)
    {
        if (f(hero))
        {
            filteredHeros.Add(hero);
        }
    }
    return filteredHeros;
}

//*****************************************************************************************************

// Examples for using Action and Func generic delegates

var watch = Stopwatch.StartNew();
CountToNearlyInfinity(); // <<<< Method to benchmark
watch.Stop();
Console.WriteLine(watch.Elapsed.TotalMilliseconds);

MeasureTime(() => CountToNearlyInfinity());

Console.WriteLine($"The result is {MeasureTimeFunc(() => CalculateSomeResult())}");

void MeasureTime(Action a)
{
    var watch = Stopwatch.StartNew();
    a();
    watch.Stop();
    Console.WriteLine(watch.Elapsed.TotalMilliseconds);
}

int MeasureTimeFunc(Func<int> f)
{
    var watch = Stopwatch.StartNew();
    var result = f();
    watch.Stop();
    Console.WriteLine(watch.Elapsed.TotalMilliseconds);
    return result;
}

void CountToNearlyInfinity()
{
    for (var i = 0; i < 1000000000; i++) ;
}

int CalculateSomeResult()
{
    // Simulate some interesting calculation
    for (var i = 0; i < 1000000000; i++) ;

    // Return the result
    return 63;
}

//*****************************************************************************************************

//Example for using record class (record) instead of class 
record Hero(string FirstName, string LastName, string HeroName, bool CanFly);
//record class Hero(string FirstName, string LastName, string HeroName, bool CanFly);
class Hero1
{
    public Hero1(string firstName, string lastName, string heroName, bool canFly)
    {
        FirstName = firstName;
        LastName = lastName;
        HeroName = heroName;
        CanFly = canFly;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string HeroName { get; }
    public bool CanFly { get; }
}

//Think of a delegates as a type for methods similar to classes as a type for objects
delegate int MathOp(int a, int b);

//Define a generic delegate
delegate T MathOpGeneric<T>(T a, T b);

//Predicate delegate is a delegate which returns boolean and gets the data structure as parameter
delegate bool FilterDelegate(Hero h);
delegate bool FilterGeneric<T>(T h);
//Predicate delegate should satisfy some criteria of method and must have one input parameter
//and one boolean return type either true or false.

