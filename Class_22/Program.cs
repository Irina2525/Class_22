using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер массива: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            Task<int[]> task1 = new Task<int[]>(func1, n); // Task<int[]> параметризиреум массивом, ожидаем получить массив , тк нужно вернуть результат 

            //Func<Task<int[]>, int[]> func2 = new Func<Task<int[]>, int[]>(SortArray);  // <Task<int[]>  кт будет возвращать массив целых чисел
            //Task<int[]> task2 = task1.ContinueWith<int[]>(func2);

            Action<Task<int[]>> action = new Action<Task<int[]>>(PrintArray);
            Task task2 = task1.ContinueWith(action);


            task1.Start();
            task2.Wait();
            Console.WriteLine();

            Action<Task<int[]>> func2 = new Action<Task<int[]>>(Summ);
            Task task3 = task1.ContinueWith(func2);

            Action<Task<int[]>> func3 = new Action<Task<int[]>>(Max);
            Task task4 = task1.ContinueWith(func3);

            task3.Wait();
            task4.Wait();


            Console.ReadKey();
        }
        // метод кт формирует массив 
        static int[] GetArray(object a) //преобразовали метод что бы он соот-вал делегату (не int а object)
        {
            int n = (int)a;  // получаем число путем приведения из object в int
            int[] array = new int[n];
            Random random = new Random(); // заполняем случайными числами массив 
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100);
            }
            return array;
        }
        ////метод кт сортирует массив   
        static int[] SortArray(Task<int[]> task) // метод принял Task задачу 
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count() - 1; i++) //Count возвращает кол-во эл-тов в массиве
            {
                for (int j = i + 1; j < array.Count(); j++)
                {
                    if (array[i] > array[j]) // если эл-ты массива стоят не правильно (когда предыдущий больше чем следующий)
                    {
                        int t = array[i]; //меняем их местами и используем 3ю переменную для этого t
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }
            return array;
        }

        // метод будет печатать массив и выводить его на экран 
        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i] } ");
            }
        }


        // метод кт вычислет сумму чисел массива
        static void Summ(Task<int[]> task)
        {
            int t = 0;
            int[] array = task.Result;
            for (int j = 0; j < array.Count(); j++) t = t + array[j];
            Console.WriteLine("Сумма массива = " + t);
        }


        // метод кт вычислет максимальное число в массиве
        static void Max(Task<int[]> task)
        {
            int t;
            int[] array = task.Result;
            t = array.Max();
            Console.WriteLine("Максимальное число в массиве = " + t);
        }
    }
}
