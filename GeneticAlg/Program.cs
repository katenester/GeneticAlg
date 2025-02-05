﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlg
{
    class Program
    {
        /// ---------------------------------------------------- Часть задачи о рюкзаке, я спрячу это в класс
        /// <summary>
        ///  Константы для параметров генетического алгоритма.
        /// </summary>
        private const double CROSSOVERPROBABILITY = 0.8;
        private const double MUTATIONPROBABILITY = 0.1;
        private const int POPULATIONSIZE = 100;
        private const int GENERATIONCOUNT = 500;

        /// <summary>
        /// Инициализация экземпляра MainAlgorithm с параметрами.
        /// </summary>
        private static readonly BackpackMainAlgorithm<int> Algorithm = new BackpackMainAlgorithm<int>(CROSSOVERPROBABILITY,
            MUTATIONPROBABILITY, POPULATIONSIZE,
            GENERATIONCOUNT, BackpackExtractItemsFromInt32Value.Sort, BackpackFitnessFunction.CalculateFitness,
            BackpackBreeding.Crossover, BackpackBreeding.Mutation);

        /// <summary>
        /// Очистка списков предметов и имен.
        /// </summary>
        /// <param name="result"></param>
        private static void ClearLists(BackpackGenome<int> result)
        {
            result.ItemsPicked.Clear();
            BackpackExtractItemsFromInt32Value.Selection.Clear();
            BackpackProblem.ListOfNames.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        private static void RunApplication(BackpackMainAlgorithm<int> a)
        {
            // Генерация случайных предметов.
            BackpackProblem.GenerateRandomItems();
            // Эволюция популяции.
            BackpackGenome<int> Result = a.Evolve(BackpackProblem.GenerateRandomSolutions(POPULATIONSIZE));

            // Вывод результатов в консоль.
            Console.WriteLine("Выбранные предметы:");
            Console.WriteLine();

            foreach (BackpackItem t in Result.ItemsPicked)
            {
                Console.WriteLine(t.Name + "  " + "\t" + " (Ценность:" + t.Worth + ") " +
                                  "\t" + "(Вес:" + t.Weight + ")");
            }

            Console.WriteLine();
            Console.WriteLine("Максимальный вес: " + BackpackFitnessFunction.MaxValue);
            Console.WriteLine("Текущий вес: " + Result.ItemsPicked.Sum(t => t.Weight));
            Console.WriteLine("Текущая ценность: " + Result.ItemsPicked.Sum(t => t.Worth));
            Console.WriteLine();
            Console.WriteLine("Вес всех предметов: " + BackpackExtractItemsFromInt32Value.Selection.Sum(t => t.Weight));
            Console.WriteLine("Ценность всех предметов: " + BackpackExtractItemsFromInt32Value.Selection.Sum(t => t.Worth));
            Console.ReadKey();

            ClearLists(Result);
            Console.Clear();

            // Рекурсивный запуск приложения с новым экземпляром алгоритма.
            RunApplication(new BackpackMainAlgorithm<int>(CROSSOVERPROBABILITY, MUTATIONPROBABILITY, POPULATIONSIZE,
                GENERATIONCOUNT, BackpackExtractItemsFromInt32Value.Sort, BackpackFitnessFunction.CalculateFitness,
                BackpackBreeding.Crossover, BackpackBreeding.Mutation));
        }

        /// ---------------------------------------------------- конец


        static int NumQueens;

        static void GenerationCallback(int generation, string bestIndividual, string target)
        {
            Console.WriteLine($"Generation {generation}: Best fit - {bestIndividual.Length}, Best individual - {bestIndividual}");
        }

        static void ExecuteMenuItem(int index)
        {
            Console.Clear();
            Console.WriteLine($"Вы выбрали: {index + 1}");
            if (index == 0)
            {
                RunApplication(Algorithm);
            }
            else if (index == 1)
            {
                while (true)
                {
                    Console.WriteLine("Введите число королев: ");
                    if (int.TryParse(Console.ReadLine(), out NumQueens) && (NumQueens > 0))
                    {
                        Console.WriteLine("Запуск алгоритма");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка. Некорректное значение");
                    }
                }
                //Время работы программы.
                //Stopwatch stopWatch = new();
                //stopWatch.Start();
                //Вызываем метод geneticAlg() для выполнения генетического алгоритма.
                QueenBoards solution = QueenGenAlg.QueenGeneticAlg(NumQueens);
                //stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                //Console.WriteLine($"Время работы алгоритма : {ts}");
                //Выводит решение на экран.
                Console.WriteLine(solution.ToString() + "\n");
                solution.PrintBoard();
                Console.ReadLine();
            }
            else if (index == 2)
            {
                string target = "HelloWorld";
                int populationSize = 100;
                double mutationRate = 0.03;

                StringRebuider geneticAlgorithm = new StringRebuider(GenerationCallback);
                geneticAlgorithm.RunGeneticAlgorithm(target, populationSize, mutationRate);
            }
            else if (index == 3)
            {
                Console.Write("Введите количество городов (минимум 3): ");
                // Входные данные.
                // Количество городов. Минимум 3 городa.
                int numOfCities;
                while (!int.TryParse(Console.ReadLine(), out numOfCities) || numOfCities < 3)
                {
                    Console.Write("Неправильный ввод данных. Попробуйте ещё раз: ");
                }

                double[,] distances = new double[numOfCities, numOfCities];

                Console.WriteLine("Заполните матрицу расстояний: ");
                for (int i = 0; i < numOfCities; i++)
                {
                    for (int j = 0; j < numOfCities; j++)
                    {
                        while (!double.TryParse(Console.ReadLine(), out distances[i, j]) || distances[i, j] < 0)
                        {
                            Console.Write("Неправильный ввод данных. Попробуйте ещё раз: ");
                        }
                    }
                }

                Console.WriteLine("Матрица расстояний: ");
                for (int i = 0; i < numOfCities; i++)
                {
                    for (int j = 0; j < numOfCities; j++)
                        Console.Write(string.Format("{0,3}", distances[i, j]));
                    Console.WriteLine();
                }

                // Параметры генетического алгоритма.
                // Размер одной популяции = 100.
                int populationSize = 100;
                // Сколько сменится поколений.

                int maxGenerations = 100;

                // Создание начальной популяции (популяция - это множество особей, то есть маршрутов).
                List<List<int>> population = TravelingSalesman.InitializePopulation(numOfCities, populationSize);

                Console.WriteLine("Начальная популяция: ");
                foreach (List<int> list in population)
                {
                    foreach (int number in list)
                    {
                        Console.Write(number);
                    }
                    Console.WriteLine(" ");
                }

                // Основной алгоритм.
                for (int generation = 0; generation < maxGenerations; generation++)
                {
                    // Вычисление приспособленности особей в текущей популяции.
                    Dictionary<List<int>, double> fitness = TravelingSalesman.CalculateFitness(population, distances);

                    // Выбор рандомных особей для скрещивания.
                    List<List<int>> selectedParents = TravelingSalesman.SelectParents(population, fitness);

                    Console.WriteLine("Выбранные родители: ");
                    foreach (List<int> list in selectedParents)
                    {
                        foreach (int number in list)
                        {
                            Console.Write(number);
                        }
                        Console.WriteLine(" ");
                    }

                    // Скрещивание выбранных особей.
                    List<List<int>> offspring = TravelingSalesman.Crossover(selectedParents);

                    // Мутация потомства.
                    TravelingSalesman.Mutate(offspring);

                    Console.WriteLine("Потомки: ");
                    foreach (List<int> list in offspring)
                    {
                        foreach (int number in list)
                        {
                            Console.Write(number);
                        }
                        Console.WriteLine(" ");
                    }

                    // Замещение старой популяции новой.
                    population = TravelingSalesman.ReplacePopulation(population, offspring, fitness, distances);

                    Console.WriteLine("Отсортированная новая популяция: ");
                    Console.WriteLine("Особь:    Расстояние:");
                    foreach (List<int> chromosome in population)
                    {
                        foreach (int number in chromosome)
                        {
                            Console.Write(number);
                        }
                        Console.Write("     ");
                        Console.WriteLine(TravelingSalesman.CalculateTotalDistance(chromosome, distances));
                    }

                    // Если наблюдается постоянство сильных особей, значит скорее всего нашли оптимальный маршрут, выходим из цикла.
                    if ((TravelingSalesman.CalculateTotalDistance(population[0], distances) == TravelingSalesman.CalculateTotalDistance(population[1], distances))
                        && (TravelingSalesman.CalculateTotalDistance(population[1], distances)) == TravelingSalesman.CalculateTotalDistance(population[2], distances))
                    {
                        Console.WriteLine("Поколение: ");
                        Console.WriteLine(generation);
                        break;
                    }
                }

                Console.Write("Путь: ");
                foreach (int number in population[0])
                {
                    Console.Write(number);
                }
                Console.WriteLine(" ");
                Console.Write("Расстояние: ");
                Console.Write(TravelingSalesman.CalculateTotalDistance(population[0], distances));
        }

        Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void Main()
        {
            Console.CursorVisible = false;

            string[] menuItems = { "Задача о рюкзаке", "Расстановка ферзей", "Воссоздание строки", "Задача коммивояжера" };
            int selectedItemIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите пункт меню:");

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedItemIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(">");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    Console.WriteLine(" " + menuItems[i]);

                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItemIndex = (selectedItemIndex - 1 + menuItems.Length) % menuItems.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedItemIndex = (selectedItemIndex + 1) % menuItems.Length;
                        break;

                    case ConsoleKey.Enter:
                        ExecuteMenuItem(selectedItemIndex);
                        break;
                }
            }

        }
    }
}





