//Варіант #9
using System.Text;

const int N = 3;
const double epsilon = 0.0001;
double maxGradient;
double[] solution = Enumerable.Repeat(1.0, N).ToArray();
double[] gradientU;
int counter = 0;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine($"Точність: {epsilon}");
Console.WriteLine("\nПочаткове наближення для x: ");
for (int i = 0; i < solution.Length; i++)
    Console.WriteLine($"Для x{i} = {solution[i]}");

while (true)
{
    gradientU = CalculateGradient(solution[0], solution[1], solution[2]);

    for (int i = 0; i < solution.Length; i++)
        solution[i] -= epsilon * gradientU[i];

    maxGradient = gradientU.Max(x => Math.Abs(x));
    counter++;

    if (maxGradient < epsilon)
        break;
}

Console.WriteLine("\nРозв'язання системи: ");
foreach (double value in solution)
    Console.Write($"{string.Format("{0:f2}", value)} ");
Console.WriteLine($"\nКількість виконаних ітерацій: {counter}");
Console.WriteLine($"Похибка обчислень: {string.Format("{0:f9}", maxGradient)}");

static double[] CalculateGradient(double x, double y, double z)
{
    double gradX = 8 * x - 4 * Math.Pow(y, 2) + 12 * Math.Pow(z, 2)
        + 4 * x * (y + Math.Pow(x, 2) + Math.Pow(z, 2) - 2.4) - 12 * x * (z - 3 * Math.Pow(x, 2)
        - 2 * Math.Pow(y, 2) + 2.32) - 19.36;
    double gradY = 2 * y + 2 * Math.Pow(x, 2) + 2 * Math.Pow(z, 2)
        + 4 * y * (-2 * x + 1 * Math.Pow(y, 2) - 3 * Math.Pow(z, 2) + 4.84) - 8 * y
        * (z - 3 * Math.Pow(x, 2) - 2 * Math.Pow(y, 2) + 2.32) - 4.8;
    double gradZ = 2 * z - 6 * Math.Pow(x, 2) - 4 * Math.Pow(y, 2)
        + 4 * z * (y + Math.Pow(x, 2) + Math.Pow(z, 2) - 2.4) - 12 * z
        * (-2 * x + 1 * Math.Pow(y, 2) - 3 * Math.Pow(z, 2) + 4.84) + 4.64;

    return new double[] { gradX, gradY, gradZ };
}