//Варіант #9
using System.Text;

(float a, float b, float epsilon, float epsilon0) = (0, 0, 0, 0);
float m, M, alpha;
(float x0, float r, float xr) = (0, 1, 0);

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Введіть проміжок для дослідження функції: ");
Input(ref a, 'A');
Input(ref b, 'B');

Console.WriteLine("\nВведіть Ɛ: ");
Input(ref epsilon, 'Ɛ');

Console.WriteLine("\nМетодом простих ітерацій: ");

Console.WriteLine($"\nПроміжок обчислення функції: ({a}, {b})");
Console.WriteLine($"Перевіримо умову f({a}) * f({b}) < 0: ");
Console.WriteLine($"f({a}) = {string.Format("{0:f2}", f(a))}; f({b}) = {string.Format("{0:f2}", f(b))}; " +
    $"f({a}) * f({b}) = {string.Format("{0:f2}", f(a) * f(b))}");

m = f1(a < b ? a : b);
M = f1(a < b ? b : a);
alpha = 2 / (m + M);

x0 = a < b ? a : b;
Iterations(x0, ref r, xr, epsilon, out int counter);
Output(r, counter, "простих ітерацій");

Console.WriteLine("\nМетодом Ньютона: ");

m = Math.Abs(f1(a));
M = Math.Abs(f2(b));
epsilon0 = (float)(Math.Sqrt(2 * (m * epsilon) / M));
r = 1;
Newton(a, b, x0, ref r, xr, epsilon0, out counter);
Output(r, counter, "Ньютона");

void Input(ref float border, char typeOfBorder)
{
    bool result;
    Console.Write($"{typeOfBorder} = ");
    result = float.TryParse(Console.ReadLine(), out border);
    while (result == false)
    {
        Console.WriteLine("\nВведено некоректне значення!");
        Console.Write($"Введіть {typeOfBorder} ще раз: ");
        result = float.TryParse(Console.ReadLine(), out border);
    }
}

void Output(float r, int counter, string typeOfMethod)
{
    Console.WriteLine($"\nРезультат виконання методу {typeOfMethod}: {string.Format("{0:f7}", r)}");
    Console.WriteLine($"Ітерацій було виконано: {counter}");
    Console.WriteLine($"Похибка знайденого значення: {string.Format("{0:f7}", r)}");
}

float f(float x) =>
    (float)(2 * Math.Exp(x) - 5 * x - 2);

float f1(float x) =>
    (float)(2 * Math.Exp(x) - 5);

float f2(float x) =>
    (float)(2 * Math.Exp(x));

float Fita(float x, float alpha) =>
    x - alpha * f(x);

void Iterations(float x0, ref float r, float xr, float epsilon, out int counter)
{
    counter = 0;
    while (r >= epsilon)
    {
        xr = Fita(x0, alpha);
        r = Math.Abs(xr - x0);
        x0 = xr;
        counter++;
    }
}

void Newton(float a, float b, float x0, ref float r, float xr, float epsilon0, out int counter)
{
    counter = 0;

    Console.WriteLine($"\nПеревіряємо умову f({x0}) * f2({x0}) > 0: ");
    Console.WriteLine($"f({x0}) = {string.Format("{0:f2}", f(x0))}; f2({x0}) = {string.Format("{0:f2}", f2(x0))}; " +
        $"f({x0}) * f2({x0}) = {string.Format("{0:f2}", f(x0) * f2(x0))}");
    if (f(a) * f2(a) > 0)
    {
        x0 = a;
        Console.WriteLine("Умова виконується");
    }
    else
    {
        x0 = b;
        Console.WriteLine("Умова не виконується. Змініть початкове наближення функції.");
        Console.WriteLine($"\nПеревіряємо умову f({x0}) * f2({x0}) > 0: ");
        Console.WriteLine($"f({x0}) = {string.Format("{0:f2}", f(x0))}; f2({x0}) = {string.Format("{0:f2}", f2(x0))}; " +
            $"f({x0}) * f2({x0}) = {string.Format("{0:f2}", f(x0) * f2(x0))}");
        Console.WriteLine("Умова виконується");
    }

    while(r >= epsilon0)
    {
        xr = x0 - f(x0) / f1(x0);
        r = Math.Abs(xr - x0);
        x0 = xr;
        counter++;
    }
}