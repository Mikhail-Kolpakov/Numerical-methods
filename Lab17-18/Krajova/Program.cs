// Варіант #9
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Ініціалізуємо a1, a2, b1, b2, A, B, перша межа, друга межа, кількість точок, похибка
var (alpha1, alpha2, beta1, beta2, a, b, v1, v2, n, epsilon) = GetInitialValues();
var count = 0;

DisplayStartValues(alpha1, alpha2, beta1, beta2, a, b, v1, v2, n); // Виводимо на консоль початкові значення

while (true)
{
    count++;
    var h = (v2 - v1) / n; // Крок

    // Обчислення для кроку h та h / 2 методом скінченних різниць
    float[] y = Solve(h, n, v1, alpha1, alpha2, beta1, beta2, a, b);
    float[] y2 = Solve(h / 2, n * 2 - 1, v1, alpha1, alpha2, beta1, beta2, a, b);

    var maxDiff = Enumerable.Range(0, n).Select(i => Math.Abs(y2[i * 2] - y[i])).Max(); // Знаходимо похибку

    // Виводимо результати
    if (maxDiff < epsilon)
    {
        DisplayResults(y, h, v1, n, count, maxDiff);
        break;
    }

    n *= 2;
}

(float, float, float, float, float, float, float, float, int, float) GetInitialValues() // Метод для встановлення початкових значень
{
    Console.Write("Чи бажаєте ви використовувати значення з варіант №9?(y/n): ");
    string agreement = Console.ReadLine()!.ToLower();

    while (agreement is not ("y" or "n"))
    {
        Console.Write("Була введена не коректна опція. Спробуйте ще раз(y/n): ");
        agreement = Console.ReadLine()!.ToLower();
    }

    if (agreement == "y")
        return (1, 2, 1, 0, 0.6F, 1, 1, 1.3F, 3, 0.001F); // Використовуємо значення за замовчуваннням
    
    // Вводимо свої значення
    Console.WriteLine();
    var newEpsilon = InputValidValue<float>(float.TryParse, "похибки");
    var newAlpha1 = InputValidValue<float>(float.TryParse, "a1");
    var newAlpha2 = InputValidValue<float>(float.TryParse, "a2");
    var newBeta1 = InputValidValue<float>(float.TryParse, "b1");
    var newBeta2 = InputValidValue<float>(float.TryParse, "b2");
    var newA = InputValidValue<float>(float.TryParse, "A");
    var newB = InputValidValue<float>(float.TryParse, "B");
    Console.WriteLine("Введіть значення меж (a; b): ");
    var newV1 = InputValidValue<float>(float.TryParse, "a");
    var newV2 = InputValidValue<float>(float.TryParse, "b");
    var newN = InputValidValue<int>(int.TryParse, "кількості відрізків n");

    return (newAlpha1, newAlpha2, newBeta1, newBeta2, newA, newB, newV1, newV2, newN, newEpsilon);
}

T InputValidValue<T>(TryParseHandler<T> tryParse, string valueFor) // Метод для введення значень та перевірки на їх коректність
{
    T value;
    
    Console.Write($"Введіть значення для {valueFor}: ");
    while (!tryParse(Console.ReadLine(), out value))
        Console.Write("Було введено не коректне значення. Спробуйте ще раз: ");

    return value;
}

void DisplayStartValues(float alpha1, float alpha2, float beta1, float beta2, float a, float b, float v1,
    float v2, int n) // Метод для відображення початкових значень
{
    Console.WriteLine("\nПочаткові дані: ");
    Console.WriteLine($"a1: {alpha1}");
    Console.WriteLine($"a2: {alpha2}");
    Console.WriteLine($"b1: {beta1}");
    Console.WriteLine($"b2: {beta2}");
    Console.WriteLine($"A: {a}");
    Console.WriteLine($"B: {b}");
    Console.WriteLine($"Межі: ({v1}; {v2})");
    Console.WriteLine($"Кількість точок n: {n}");
}

void DisplayResults(float[] y, float h, float a, int n, int count, float maxDiff) // Метод дял виведення повних результатів
{
    Console.WriteLine($"\nКiлькiсть точок = {n}");
    Console.WriteLine($"h = {h:f12}");
    Display(y, h, a);
    Console.WriteLine($"\nКінцевий крок: {h:f12}");
    Console.WriteLine($"Метод було викликано: {count} раз");
    Console.WriteLine($"Похибка апроксимації: {maxDiff:f12}");
}

void Display(float[] y, float h, float a) // Метод який використовується для виводу розрахунків на кожному кроці
{
    for (int i = 0; i < y.Length; i++)
        Console.WriteLine($"{i}) {a + i * h:f6}\t{y[i]:f6}");
}

// Зазначаємо невеликі методи для позначення a[i], b[i], c[i] та d[i]
float Ai(float h) => 
    1 - h * -0.5F / 2;

float Bi(float h) => 
    (float)Math.Pow(h, 2) * 3 - 2;

float Ci(float h) => 
    1 + h * -0.5F / 2;

float Di(float x, float h) => 
    (float)(Math.Pow(h, 2) * 2 * Math.Pow(x, 2));

float[] Solve(float h, int n, float border, float alpha1, float alpha2, float beta1, float beta2, float a, float b) // Метод основних розрахунків
{
    // Ініціалізуємо списки
    var x = Enumerable.Range(0, n + 1).Select(i => border + i * h).ToList();
    (float[] y, float[] s, float[] t) = (new float[n + 1], new float[n + 1], new float[n + 1]);

    // Задаємо коефіцієнти граничних умов
    (float b0, float c0, float d0) = (h * alpha1 - alpha2, alpha2, h * a);
    (float an, float bn, float dn) = (-beta2, h * beta1 + beta2, h * b);
    
    // Обчислюємо коефіцієнти
    s[0] = -c0 / b0;
    t[0] = d0 / b0;

    for (int i = 1; i < n; i++)
    {
        s[i] = -Ci(h) / (Bi(h) + Ai(h) * s[i - 1]);
        t[i] = (Di(x[i], h) - Ai(h) * t[i - 1]) / (Bi(h) + Ai(h) * s[i - 1]);
    }

    // Обрахування
    y[n] = (dn - an * t[n - 1]) / (bn + an * s[n - 1]);
    for (int i = n - 1; i >= 0; i--)
        y[i] = s[i] * y[i + 1] + t[i];

    return y;
}

internal delegate bool TryParseHandler<T>(string? s, out T result);