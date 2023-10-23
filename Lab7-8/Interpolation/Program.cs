//Варіант #9
using System.Text;

const int amountOfValues = 5;
int[] xSet = new int[amountOfValues] { 0, 1, 2, 3, 4 }; //Масив для збереження значень для ряду X
int[] ySet = new int[amountOfValues] { 2, 6, 14, 38, 90 }; //Масив для збереження значень для ряду Y
float p; //Значення для обчислення

Console.OutputEncoding = Encoding.UTF8;

Console.Write("Введіть точку для якої бажаєте знайти значення функції (p): ");
while (!float.TryParse(Console.ReadLine(), out p)) //Робимо перевірку на коретність введеного значенння для обчислення
    Console.Write("Введено не коректне значення, спробуйте ще раз: ");

Console.WriteLine($"\nПісля виконання розрахунків отриманий результат: {Interpolation(xSet, ySet, p)}");

float Interpolation(int[] x, int[] y, float valueToCalculate) //Метод для обрахунку кінцевого значення
{
    float sum = 0;

    for (int i = 0; i < y.Length; i++) //Основний цикл розрахунків
    {
        float prod = y[i];
        for (int j = 0; j < x.Length; j++)
            if (i != j)
                prod *= (valueToCalculate - x[j]) / (x[i] - x[j]);
        sum += prod;
    }

    return sum;
}