// Варіант #9
using System.Text;

(float x2I, float y2I, float x1I, float y1I) = (0, 0, 0, 0); // Початкові дані для x та y кроків 0.1 та 0.2
(float h1, float h2) step = (0.1F, 0.2F); // Кроки
float epsilon; // Точність
const int order = 4; // Порядок за яким знаходимо

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Рівняння за яким буде проводитись розв'язок:\nf(x, y) = (1 - y^2) * cos(x) + 0.6 * y");

Console.WriteLine("\nПочаткові дані для x та y: ");
Console.WriteLine($"x = {x2I}");
Console.WriteLine($"y = {y2I}");

Console.Write("\nВведіть точність: ");
while(!float.TryParse(Console.ReadLine(), out epsilon))
    Console.Write("Точність введена не коректно. Спробуйте ще раз: ");
Console.WriteLine();

float error = Math.Abs(y2I - y1I) / 15; // Похибка на поточному кроці
int counter = 0; // Лічильник кількості проведених операцій

do
{
    float h2Sum = CalculateError(x2I, y2I, step.h2, order); // Вираховуємо для кроку 0.2
    float h1Sum = CalculateError(x1I, y1I, step.h1, order); // Вираховуємо для кроку 0.1
    
    Console.WriteLine($"Ітерація #{counter + 1}: ");
    Console.WriteLine($"Значення похибки на кроці x = {x2I:f1}: {error}");
    Console.WriteLine($"Y = {y2I:f3}\n");
     
    x1I += step.h1;
    y1I += h1Sum;
    h1Sum = CalculateError(x1I, y1I, step.h1, order); // Вираховуємо для кроку 0.1
     
    x2I += step.h2;
    y2I += h2Sum;
    x1I += step.h1;
    y1I += h1Sum;
    
    error = Math.Abs(y2I - y1I) / 15; // Похибка на поточному кроці
    
    counter++;
} while (epsilon > error);

Console.WriteLine($"Кількість виконаних операцій: {counter}");
Console.WriteLine($"Кінцева похибка, на якій зацінчилось виконання: {error}");

float CalculateError(float xi, float yi, float step, int order) // Метод для вирахування похибки
{
    // Масив та змінні для збереження формульних значень
    var kValues = new float[order];
    float deltaYSum = 0;

    // Робимо основні табличні розрахунки
    for (int i = 0; i < order; i++)
    {
        float xValue, yValue;
        
        switch (i)
        {
            case 0:
                xValue = xi;
                yValue = yi;
                kValues[i] = step * Function(xValue, yValue);
                deltaYSum += kValues[i];
                break;
            case 1:
            case 2:
                xValue = xi + step / 2;
                yValue = yi + kValues[i - 1] / 2;
                kValues[i] = step * Function(xValue, yValue);
                deltaYSum += 2 * kValues[i];
                break;
            default:
                xValue = xi + step;
                yValue = yi + kValues[i - 1];
                kValues[i] = step * Function(xValue, yValue);
                deltaYSum += kValues[i];
                break;
        }
    }
    
    return deltaYSum / 6; // Повертаємо суму
}

float Function(float x, float y) => // Метод для задання функції даною за умовою завдання
    (float)((1 - Math.Pow(y, 2)) * Math.Cos(x) + 0.6 * y);