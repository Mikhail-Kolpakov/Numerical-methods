//Варіант #9
using System.Text;

//Початкові дані
const int n = 9;
const int m = 1;
double a0, a1, R2, fisher;
double[] X = new double[n] { 11.2, 12.6, 18.6, 21.4, 25, 29.6, 31.1, 38.2, 40 };
double[] Y = new double[n] { 0.99, 0.74, 0.69, 0.58, 0.41, 0.33, 0.26, 0.19, 0.1 };
string[] formulasList = new string[n] {
    "y = a0 + a1 * x",
    "y = a0 + a1 * ln(x)",
    "y = a0 + a1 / x",
    "y = a0 * (a1)^x",
    "y = a0 * (x)^a1",
    "y = exp(a0 + a1 / x)",
    "y = 1 / (a0 + a1 * x)",
    "y = 1 / (a0 + a1 * ln(x))",
    "y = x / (a0 + a1 * x)"
};

// Вирішуємо, чи використовувати початкові дані, чи занести свої
Console.OutputEncoding = Encoding.UTF8;
InputData(X, Y);

// Вираховуємо середньоарифметичні значення для X та Y
double arithMeanX = X.Average();
double arithMeanY = Y.Average();

// Вираховуємо середньогеометричні значення для X та Y
double geomMeanX = X.Aggregate((current, next) => current * next);
geomMeanX = Math.Pow(geomMeanX, 1.0 / X.Length);
double geomMeanY = Y.Aggregate((current, next) => current * next);
geomMeanY = Math.Pow(geomMeanY, 1.0 / Y.Length);

// Вираховуємо середньогармонічні значення для X та Y
double harmMeanX = X.Length / X.Sum(x => 1 / x);
double harmMeanY = Y.Length / Y.Sum(y => 1 / y);

// Знаходимо X середнє
double[] values = new double[] { arithMeanX, geomMeanX, harmMeanX };
double[] averageX = Enumerable.Range(0, n).Select(i => values[i % values.Length]).ToArray();

// Знаходимо Y середнє
values = [arithMeanY, geomMeanY, harmMeanY];
double[] averageY = Enumerable.Range(0, n).Select(i => values[i / 3]).ToArray();

double[] regressionY = CalculateRegression(averageX, X, Y); //Вираховуємо Y регресійне

double[] absValues = CalculateAbsoluteValues(averageY, regressionY); // Розраховуємо абсолютні значення
double minAbsValue = absValues.Min(); // Знаходимо найменше абсолютне значення
int indexOfRecomendedFormula = Array.IndexOf(absValues, minAbsValue); //Знаходимо індекс рекомендованої формули

// Довиводимо частину результатів розрахунків програми до консолі
DisplayTable(averageX, averageY, regressionY, absValues);
Console.WriteLine($"\nМінімальне значення: {minAbsValue:f5}");
Console.WriteLine($"Рекомендоване рівняння: #{indexOfRecomendedFormula + 1}");
Console.WriteLine(formulasList[indexOfRecomendedFormula]);

int selectedFormula;

Console.Write("\nВведіть номер формули за якою бажаєте розв'язати(1-9): ");
while(!int.TryParse(Console.ReadLine(), out selectedFormula) || selectedFormula > n || selectedFormula < 1) //Перевірка на коректність вводу
    Console.Write("Був введений не коректний номер формули. Спробуйте ще раз(1-9): ");

CalculateFinalAnswer(X, Y, n, selectedFormula - 1, out a0, out a1, out R2, out fisher); // Знаходимо a0, a1, R2 та коеф. Фішера

// Довиводимо кінцеву частину результатів розрахунків програми до консолі
Console.WriteLine($"\nШукане рівняння: {FindFinalFormula(selectedFormula - 1, a0, a1)}");
Console.WriteLine($"Коефіцієнт детермінації: {R2:f4}");
Console.WriteLine($"Критерій Фішера: {fisher:f4}");

void InputData(double[] X, double[] Y) {
    Console.Write("Використати дані з варіанту #9?(yes/no): ");
    string agreement = Console.ReadLine()!.ToLower();

    while (!(agreement == "yes" || agreement == "no")) { //Перевірка на коректність вводу
        Console.Write("Була введена не коректна опція. Спробуйте ще раз(yes/no): ");
        agreement = Console.ReadLine()!.ToLower();
    }

    if (agreement == "yes") { //якщо користувач обрав заготовлену умову
        Console.WriteLine("\nПочаткові дані: ");
        Console.Write($"X = {{ {string.Join("\t", X)} }} \n");
        Console.Write($"Y = {{ {string.Join("\t", Y)} }} \n\n");
    }
    else { // Якщо користувач захотів ввести свою умову
        Console.WriteLine("\nЗаповніть масив X: ");
        for (int i = 0; i < X.Length; i++) {
            Console.Write($"Введіть значення з індексом [{i}]: ");
            while (!double.TryParse(Console.ReadLine(), out X[i]))
                Console.Write("Введено не коректне значення. Спробуйте ще раз: ");
        }
        
        Console.WriteLine("\nЗаповніть масив Y: ");
        for (int i = 0; i < Y.Length; i++) {
            Console.Write($"Введіть значення з індексом [{i}]: ");
            while (!double.TryParse(Console.ReadLine(), out Y[i]))
                Console.Write("Введено не коректне значення. Спробуйте ще раз: ");
        }
        Console.WriteLine();
    }
}

void DisplayTable(double[] averageX, double[] averageY, double[] regressionY, double[] finalValues) // Метод для виведення таблиці значень до консолі
{
    Console.WriteLine("N\t|\tX-сер\t|\tY-сер\t|\tY^\t|\tФормула\t|");
    for (int i = 0; i < finalValues.Length; i++)
        Console.WriteLine($"{i + 1}\t|\t{averageX[i]:f4}\t|\t{averageY[i]:f4}\t|\t{regressionY[i]:f5}\t|\t{finalValues[i]:f5}\t|");
}

double[] CalculateRegression(double[] valuesX, double[] arrayX, double[] arrayY) // Метод для знаходження y регресійного
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

double[] CalculateAbsoluteValues(double[] averageY, double[] regressionY) // Метод для знаходження кінцевих значень
{
    double[] finalValues = new double[averageY.Length];

    for (int i = 0; i < finalValues.Length; i++)
        finalValues[i] = Math.Abs((averageY[i] - regressionY[i]) / regressionY[i]);

    return finalValues;
}

void CalculateFinalAnswer(double[] X, double[] Y, int n, int formulaIndex, out double a0, out double a1, out double R2, out double fisher) // Метод для вирахування a0, a1, R2 та коеф. Фішера
{
    MakeFormulaReplacements(formulaIndex, out double sumX, out double sumY, out double sumXY, out double sumX2, out double sumY2);

    a1 = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
    a0 = (sumY - a1 * sumX) / n;
    R2 = a1 * (n * sumXY - sumX * sumY) / (n * sumY2 - Math.Pow(sumY, 2));
    fisher = R2 / (1 - R2) * (n - m - 1) / m;

    switch (formulaIndex) {
        case 3:
            a0 = Math.Exp(a0);
            a1 = Math.Exp(a1);
            break;
        case 4:
            a0 = Math.Exp(a0);
            break;
        case 8:
            a0 = a1;
            a1 = (sumY - a0 * sumX) / n;
            break;
    }
}

string FindFinalFormula(int selectedFormulaIndex, double a0, double a1) //Метод для отримання кінцевої формули
{
    string suitableFormula = formulasList[selectedFormulaIndex];
    string formulaWithIncludedA0AndA1 = suitableFormula.Replace("a0", Math.Round(a0, 3).ToString())
        .Replace("a1", Math.Round(a1, 3).ToString());

    return formulaWithIncludedA0AndA1;
}

void MakeFormulaReplacements(int formulaIndex, out double sumX, out double sumY, out double sumXY, out double sumX2, out double sumY2) { // Метод для виконання разрахунків в залежності від обраної формули
    switch (formulaIndex) {
        case 0:
            sumX = X.Sum();
            sumY = Y.Sum();
            sumXY = X.Zip(Y, (x, y) => x * y).Sum();
            sumX2 = X.Sum(x => Math.Pow(x, 2));
            sumY2 = Y.Sum(y => Math.Pow(y, 2));
            break;
        case 1:
            sumX = X.Sum(Math.Log);
            sumY = Y.Sum();
            sumXY = X.Zip(Y, (x, y) => Math.Log(x) * y).Sum();
            sumX2 = X.Sum(x => Math.Pow(Math.Log(x), 2));
            sumY2 = Y.Sum(y => Math.Pow(y, 2));
            break;
        case 2:
            sumX = X.Sum(x => 1 / x);
            sumY = Y.Sum();
            sumXY = X.Zip(Y, (x, y) => 1 / x * y).Sum();
            sumX2 = X.Sum(x => Math.Pow(1 / x, 2));
            sumY2 = Y.Sum(y => Math.Pow(y, 2));
            break;
        case 3:
            sumX = X.Sum();
            sumY = Y.Sum(Math.Log);
            sumXY = X.Zip(Y, (x, y) => x * Math.Log(y)).Sum();
            sumX2 = X.Sum(x => Math.Pow(x, 2));
            sumY2 = Y.Sum(y => Math.Pow(Math.Log(y), 2));
            break;
        case 4:
            sumX = X.Sum(Math.Log);
            sumY = Y.Sum(Math.Log);
            sumXY = X.Zip(Y, (x, y) => Math.Log(x) * Math.Log(y)).Sum();
            sumX2 = X.Sum(x => Math.Pow(Math.Log(x), 2));
            sumY2 = Y.Sum(y => Math.Pow(Math.Log(y), 2));
            break;
        case 5:
            sumX = X.Sum(x => 1 / x);
            sumY = Y.Sum(Math.Log);
            sumXY = X.Zip(Y, (x, y) => 1 / x * Math.Log(y)).Sum();
            sumX2 = X.Sum(x => Math.Pow(1 / x, 2));
            sumY2 = Y.Sum(y => Math.Pow(Math.Log(y), 2));
            break;
        case 6:
            sumX = X.Sum();
            sumY = Y.Sum(y => 1 / y);
            sumXY = X.Zip(Y, (x, y) => x * 1 / y).Sum();
            sumX2 = X.Sum(x => Math.Pow(x, 2));
            sumY2 = Y.Sum(y => Math.Pow(1 / y, 2));
            break;
        case 7:
            sumX = X.Sum(Math.Log);
            sumY = Y.Sum(y => 1 / y);
            sumXY = X.Zip(Y, (x, y) => Math.Log(x) * 1 / y).Sum();
            sumX2 = X.Sum(x => Math.Pow(Math.Log(x), 2));
            sumY2 = Y.Sum(y => Math.Pow(1 / y, 2));
            break;
        case 8:
            sumX = X.Sum(x => 1 / x);
            sumY = Y.Sum(y => 1 / y);
            sumXY = X.Zip(Y, (x, y) => 1 / (x * y)).Sum();
            sumX2 = X.Sum(x => Math.Pow(1 / x, 2));
            sumY2 = Y.Sum(y => Math.Pow(1 / y, 2));
            break;
        default:
            sumX = sumY = sumXY = sumX2 = sumY2 = default;
            break;
    }
}