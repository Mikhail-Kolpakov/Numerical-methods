// Варіант #9
using System.Text;

float a, b; // Межі інтегрування
float epsilon; // Точність

// Вводимо межі інтегрування та точність
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Введіть межі інтегрування: ");

Console.Write("Введіть a: ");
while (!float.TryParse(Console.ReadLine(), out a))
    Console.Write("Межа \"a\" введена не коректно. Спробуйте ще раз: ");

Console.Write("Введіть b: ");
while (!float.TryParse(Console.ReadLine(), out b))
    Console.Write("Межа \"b\" введена не коректно. Спробуйте ще раз: ");

Console.Write("Введіть точність: ");
while (!float.TryParse(Console.ReadLine(), out epsilon))
    Console.Write("Межа \"epsilon\" введена не коректно. Спробуйте ще раз: ");

// Обчислюємо інтеграл та виводимо кінцевий результат
Console.WriteLine();
CalculateSimpson(a, b, epsilon);

void CalculateSimpson(float a, float b, float accuracy) // Головний метод для проведення розрахунків
{
    int n = 2; // Початкова кількість кроків
    float prevSimpson = 0; // Попередній результат
    float currentSimpson = SimpsonIteration(a, b, n); // Поточний результат

    while (Math.Abs(currentSimpson - prevSimpson) > accuracy)
    {
        Console.WriteLine($"При n = {n} результат = {currentSimpson:f5}");

        n *= 2; // Подвоюємо кількість кроків
        prevSimpson = currentSimpson;
        currentSimpson = SimpsonIteration(a, b, n);
    }

    Console.WriteLine($"\nКількість кроків n при кінцевому результаті: {n}");
    Console.WriteLine($"Остаточний результат інтегрування: {currentSimpson:f5}");
    Console.WriteLine($"Кінцева точність: {Math.Abs(currentSimpson - prevSimpson):f5}");
}

float SimpsonIteration(float a, float b, int n) // Метод для обчислення інтегралу за формулою Сімпсона
{
    float h = (b - a) / n; // Крок інтегрування
    float result = Func(a) + Func(b); // Початкове значення результату
    
    for (int i = 1; i < n; i += 2)
    {
        float x = a + i * h;
        result += 4 * Func(x);
    }

    for (int i = 2; i < n - 1; i += 2)
    {
        float x = a + i * h;
        result += 2 * Func(x);
    }

    return (h / 3) * result;
}

float Func(float x) => // Функція дана за моїм варіантом #9
    (float)(Math.Sqrt(x) / Math.Cos(x));