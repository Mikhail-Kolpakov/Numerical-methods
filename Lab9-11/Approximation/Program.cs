//Варіант #9
using System.Text;

double[] X = new double[] { 11.2, 12.6, 18.6, 21.4, 25, 29.6, 31.1, 38.2, 40 };
double[] Y = new double[] { 0.99, 0.74, 0.69, 0.58, 0.41, 0.33, 0.26, 0.19, 0.1 };
string[] formulasList = new string[] {
    "y = a0 + a1 * X",
    "y = a0 + a1 * ln(x)",
    "y = a0 + a1 / x",
    "y = a0 * (a1)^x",
    "y = a0 * (x)^a1",
    "y = exp(a0 + a1 / x)",
    "y = 1 / (a0 + a1 * x)",
    "y = 1 / (a0 + a1 * ln(x))",
    "y = x / (a0 + a1 * x)"
};
int n = X.Length;
const int m = 1;

double arithmeticalMeanX = X.Average();
double arithmeticalMeanY = Y.Average();

double geometricMeanX = X.Aggregate((current, next) => current * next);
geometricMeanX = Math.Pow(geometricMeanX, 1.0 / X.Length);
double geometricMeanY = Y.Aggregate((current, next) => current * next);
geometricMeanY = Math.Pow(geometricMeanY, 1.0 / Y.Length);

double harmonicMeanX = X.Length / X.Sum(x => 1 / x);
double harmonicMeanY = Y.Length / Y.Sum(y => 1 / y);

double[] values = new double[] { arithmeticalMeanX, geometricMeanX, harmonicMeanX };
double[] averageX = Enumerable.Range(0, 9).Select(i => values[i % values.Length]).ToArray();

values = [arithmeticalMeanY, geometricMeanY, harmonicMeanY];
double[] averageY = Enumerable.Range(0, 9).Select(i => values[i / 3]).ToArray();

double[] regressionY = CalculateRegression(averageX, X, Y);

double[] finalValues = CalculateFinalValues(averageY, regressionY);
double minFinalValue = finalValues.Min();

Console.OutputEncoding = Encoding.UTF8;
FindA0AndA1(X, Y, n, out double a0, out double a1);
double R2 = CalculateCoefDeter(X, Y, a1, n);
double fisher = CalculateFisher(R2, n, m);

DisplayTable(averageX, averageY, regressionY, finalValues);
Console.WriteLine($"\nМінімальне значення: {string.Format("{0:f5}", minFinalValue)}");
Console.WriteLine($"Потрібне рівняння: #{Array.IndexOf(finalValues, minFinalValue) + 1}");
Console.WriteLine($"\nШукане рівняння: {FindFinalFormula(minFinalValue, finalValues, a0, a1)}");
Console.WriteLine($"Коефіцієнт детермінації: {string.Format("{0:f4}", R2)}");
Console.WriteLine($"Критерій Фішера: {string.Format("{0:f4}", fisher)}");

double[] CalculateRegression(double[] valuesX, double[] arrayX, double[] arrayY)
{
    double[] regressionY = new double[arrayX.Length];

    for (int i = 0; i < regressionY.Length; i++)
    {
        double lowerBoundX = arrayX.Where(v => v <= valuesX[i]).Max();
        double upperBoundX = arrayX.Where(v => v >= valuesX[i]).Min();

        double lowerBoundY = arrayY[Array.IndexOf(arrayX, lowerBoundX)];
        double upperBoundY = arrayY[Array.IndexOf(arrayX, upperBoundX)];

        regressionY[i] = lowerBoundY + ((upperBoundY - lowerBoundY) / (upperBoundX - lowerBoundX))
            * (valuesX[i] - lowerBoundX);
    }

    return regressionY;
}

double[] CalculateFinalValues(double[] averageY, double[] regressionY)
{
    double[] finalValues = new double[averageY.Length];

    for (int i = 0; i < finalValues.Length; i++)
        finalValues[i] = Math.Abs((averageY[i] - regressionY[i]) / regressionY[i]);

    return finalValues;
}

void DisplayTable(double[] averageX, double[] averageY, double[] regressionY, double[] finalValues)
{
    Console.WriteLine("N\t|\tX-сер\t|\tY-сер\t|\tY^\t|\tФормула\t|");
    for (int i = 0; i < finalValues.Length; i++)
        Console.WriteLine($"{i + 1}\t|\t{string.Format("{0:f4}", averageX[i])}\t|\t{string.Format("{0:f4}", averageY[i])}\t|" +
            $"\t{string.Format("{0:f5}", regressionY[i])}\t|\t{string.Format("{0:f5}", finalValues[i])}\t|");
}

void FindA0AndA1(double[] X, double[] Y, int n, out double a0, out double a1)
{
    Console.WriteLine("\nДля a1: ");//
    Console.WriteLine($"1. {X.Zip(Y, (x, y) => Math.Log(x) * y).Sum()}");
    Console.WriteLine($"2. {X.Sum(x => Math.Log(x))}");
    Console.WriteLine($"3. {Y.Sum()}");
    Console.WriteLine($"4. {X.Sum(x => Math.Pow(Math.Log(x), 2))}");
    Console.WriteLine($"5. {Math.Pow(X.Sum(x => Math.Log(x)), 2)}\n");//
    a1 = (X.Zip(Y, (x, y) => Math.Log(x) * y).Sum() - 1 / n * X.Sum(x => Math.Log(x)) * Y.Sum())
        / (X.Sum(x => Math.Pow(Math.Log(x), 2)) - 1 / n * Math.Pow(X.Sum(x => Math.Log(x)), 2));
    a0 = 1 / n * Y.Sum() - a1 / n * X.Sum(x => Math.Log(x));
}

string FindFinalFormula(double minFinalValue, double[] finalValues, double a0, double a1)
{
    int index = Array.IndexOf(finalValues, minFinalValue);
    string suitableFormula = formulasList[index];
    string formulaWithIncludedA0AndA1 = suitableFormula.Replace("a0", Math.Round(a0, 3).ToString())
        .Replace("a1", Math.Round(a1, 2).ToString());

    return formulaWithIncludedA0AndA1;
}

double CalculateCoefDeter(double[] X, double[] Y, double a1, int n) =>
    (a1 * (n * X.Zip(Y, (x, y) => Math.Log(x) * y).Sum() - X.Sum(x => Math.Log(x)) * Y.Sum()))
        / (n * Y.Sum() - Math.Pow(Y.Sum(), 2));

double CalculateFisher(double coefDeter, int n, int m) =>
    coefDeter / (1 - coefDeter) * (n - m - 1) / m;