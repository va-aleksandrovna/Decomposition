using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Decomposition
{
    class Program
    {
        static object lockObj = new object();
        static Queue<long> numbersQueue = new Queue<long>();
        static StreamWriter writer;
        static bool isFinish = false;
        static string input;
        static int time = 3000; // время задержки

        //static AutoResetEvent waitHandler = new AutoResetEvent(true);  // объект-событие
        static ManualResetEvent waitHandler = new ManualResetEvent(true);

        static void Main(string[] args)
        {
            if (args.Length < 3) // Проверяем, что пути к файлам указаны и количество потоков
            {
                Console.WriteLine("Используйте: program.exe input_file output_file кол-во_потоков");
                return;
            }

            // cd C:\Users\DNS\Desktop\Decomposition\Decomposition\bin\Debug\net5.0
            // Decomposition.exe primer1.txt result1.txt 4

            string inputFile = args[0];
            string outputFile = args[1];

            int numThreads = Convert.ToInt32(args[2]); // Количество потоков

            // Открываем файл для чтения
            StreamReader reader;
            try
            {
                reader = new StreamReader(inputFile);
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка при открытии файла для чтения");
                return;
            }

            // Открываем файл для записи
            try
            {
                writer = new StreamWriter(outputFile);
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка при открытии файла для записи");
                reader.Close();
                return;
            }

            // Чтение чисел из файла и добавление их в очередь
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (long.TryParse(line, out long number))
                {
                    numbersQueue.Enqueue(number);
                }
                else
                {
                    writer.WriteLine($"Неверный формат числа : {line}");
                }
            }
            reader.Close();

            // Создание потоков и их запуск
            Thread[] threads = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                threads[i] = new Thread(ComputePrimeFactors);
                threads[i].Name = $"Поток {i}"; // устанавливаем имя для каждого потока
                threads[i].Start();
                Thread.Sleep(200);
            }

            while (!isFinish)
            {
                input = Console.ReadLine();

                switch (input.ToLower())
                {
                    case "pause":
                        waitHandler.Reset();
                        Console.WriteLine("Программа приостановлена. Выходной файл закрыт");
                        writer.Close();
                        break;
                    case "resume":
                        writer = new StreamWriter(outputFile, true);
                        Console.WriteLine("Программа продолжает работу");
                        waitHandler.Set();
                        break;
                    case "exit":
                        isFinish = true;
                        waitHandler.Reset();
                        //Console.WriteLine("Программа завершает работу");
                        break;
                    case "":
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }

            //// Ожидание завершения всех потоков
            //for (int i = 0; i < numThreads; i++)
            //{
            //    threads[i].Join();
            //    Console.WriteLine($"Поток {i} закрыт");
            //}

            writer.Close();

            Console.WriteLine("Разложение завершено успешно");
        }

        static void ComputePrimeFactors() // вычисление разложения и запись в файл
        {
            while (!isFinish)
            {
                ulong number;

                waitHandler.WaitOne();

                lock (lockObj)
                {
                    if (numbersQueue.Count == 0)
                    {
                        isFinish = true;
                        Thread.Sleep(time);
                        Console.WriteLine("Нажмите Enter");
                        break;
                    }
                    number = (ulong)numbersQueue.Dequeue();
                }

                waitHandler.WaitOne();

                Factorization factor = new Factorization(number);
                factor.Factorize();

                if (number == factor.GetNumber()) // проверка на правильность разложения числа
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {number}"); // проверка работы потоков

                    writer.WriteLine($"{number}: {factor.GetFactors()}");

                    Thread.Sleep(time);
                }
                else
                {
                    writer.WriteLine("Разложение неверно");
                }
            }
        }
    }
}
