//Варіант #9
using System.Text;

double[,] matrix = new double[4, 4] { //Матриця коефіцієнтів СЛАР
    { -0.81, -0.07, 0.38, -0.21 },
    { -0.22, -0.92, 0.11, 0.33 },
    { 0.11, -0.07, -0.91, -0.11 },
    { 0.33, -0.41, 0.00, -1.00 }
};
double[] additional = new double[4] { //Матриця вільних членів
    0.81,
    0.64,
    -1.71,
    1.21
};
double accuracy = 0.0001;
Seidel seidel = new Seidel(matrix, additional, accuracy);

Console.OutputEncoding = Encoding.UTF8;

//Виводимо матрицю коефіцієнтів СЛАР членів до консолі
Console.WriteLine("Матриця коефіцієнтів СЛАР має вигляд: ");
for (int i = 0; i < 4; i++)
{
    for (int j = 0; j < 4; j++)
        Console.Write($"{matrix[i, j]}\t");
    Console.WriteLine();
}

//Виводимо матрицю вільних членів до консолі
Console.WriteLine("\nМатриця вільних членів має вигляд: ");
foreach (double value in additional)
    Console.WriteLine(value);

Console.WriteLine($"\nТочність: {accuracy}");

seidel.CalculateMatrix(); //Вираховуємо кінцеву матрицю

//Виводимо результат роботи програми
Console.WriteLine("\nРезультат роботи програми: "); 
foreach(double value in seidel.ResultMatrix)
    Console.WriteLine(string.Format("{0:f5}", value));
Console.WriteLine($"\nКількість ітерацій, що потрібно було провести для отримання результату: {seidel.Iterations}");
Console.WriteLine($"Похибка обчислень: {string.Format("{0:f8}", seidel.Error)}");

class Seidel
{
    private double[,] matrix; //Основна матриця
    private double[] additional; //Матриця вільних членів
    private double Accuracy { get; set; } //Точність
    public double[] ResultMatrix { get; set; } = new double[4]; //Кінцева матриця
    public int Iterations { get; set; } //Кількість проведених ітерацій
    public double Error { get; set; } //Похибка

    public Seidel(double[,] Matrix, double[] FreeElements, double Accuracy)
    {
        matrix = Matrix;
        additional = FreeElements;
        this.Accuracy = Accuracy;
    }

    public void CalculateMatrix() //Метод для вираховування матриці
    {
        double[,] a = new double[matrix.GetLength(0), matrix.GetLength(1) + 1]; //Створення розширеної матриці, яка включає вільні члени

        //Копіюємо наші дві матриці до розширеної матриці
        for (int i = 0; i < a.GetLength(0); i++)
            for (int j = 0; j < a.GetLength(1) - 1; j++)
                a[i, j] = matrix[i, j];

        for (int i = 0; i < a.GetLength(0); i++)
            a[i, a.GetLength(1) - 1] = additional[i];

        double[] previousValues = new double[matrix.GetLength(0)]; //Масив для збереження значень з попереднього кроку

        //Виконувати до досягнення необхідної точності
        while (true)
        {
            double[] currentValues = new double[a.GetLength(0)]; //Ініціалізація масиву значень невідомих на поточному кроці

            for (int i = 0; i < matrix.GetLength(0); i++) //Обчислення значень невідомих на поточному кроці
            {
                currentValues[i] = a[i, a.GetLength(0)]; //Ініціалізація i-тої невідомої значенням вільного члена i-тої рядка матриці

                for (int j = 0; j < a.GetLength(0); j++)
                {
                    //Для j < i використовуємо значення, обчислені на цьому кроці
                    if (j < i)
                        currentValues[i] -= a[i, j] * currentValues[j];

                    //Для j > i використовуємо значення з попереднього кроку
                    if (j > i)
                        currentValues[i] -= a[i, j] * previousValues[j];
                }

                currentValues[i] /= a[i, i]; //Ділимо на коефіцієнт при i-тій невідомій
            }

            //Обчислення поточної похибки відносно попереднього кроку
            double differency = 0; 

            for (int i = 0; i < a.GetLength(0); i++)
                differency += Math.Abs(currentValues[i] - previousValues[i]);

            Iterations++;
            Error = differency;

            //Якщо досягнута необхідна точність, то завершуємо процес
            if (differency < Accuracy)
                break;

            //Переходимо до наступного кроку, поточні значення стають значеннями на попередньому кроці
            previousValues = currentValues;
        }

        ResultMatrix = previousValues;
    }
}