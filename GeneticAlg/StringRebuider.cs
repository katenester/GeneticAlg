﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    /// <summary>
    /// Класс для восстановления строки с использованием генетического алгоритма.
    /// </summary>
    internal class StringRebuider
    {
        private Random random = new Random();
        private Action<int, string, string> generationCallback;

        /// <summary>
        /// Инициализирует новый экземпляр класса StringRebuilder с указанным делегатом для вывода информации о поколении.
        /// </summary>
        /// <param name="generationCallback"> Делегат для вывода информации о поколении </param>
        public StringRebuider(Action<int, string, string> generationCallback)
        {
            this.generationCallback = generationCallback;
        }

        /// <summary>
        /// Генерирует случайную строку указанной длины из символов ASCII.
        /// </summary>
        /// <param name="length"> Длина строки </param>
        /// <returns> Случайная строка указанной длины </returns>
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Оценивает фитнес индивида, измеряя схожесть строки с целевой строкой.
        /// </summary>
        /// <param name="individual"> Строка индивида </param>
        /// <param name="target"> Целевая строка для сравнения </param>
        /// <returns> Значение фитнеса, представляющее схожесть строки с целевой </returns>
        private int CalculateFitness(string individual, string target)
        {
            return individual.Zip(target, (a, b) => a == b ? 1 : 0).Sum();
        }

        /// <summary>
        /// Производит мутацию строки с заданной вероятностью мутации.
        /// </summary>
        /// <param name="individual"> Строка, подвергаемая мутации </param>
        /// <param name="mutationRate"> Вероятность мутации для каждого символа в строке. Значение должно быть в пределах [0, 1] </param>
        /// <returns> Мутированная строка </returns>
        private string Mutate(string individual, double mutationRate)
        {
            char[] mutated = individual.ToCharArray();
            for (int i = 0; i < mutated.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    int mutationType = random.Next(3);
                    switch (mutationType)
                    {
                        case 0:
                            mutated[i] = (char)(random.Next(32, 127));
                            break;
                        case 1:
                            if (mutated.Length > 1)
                                mutated[i] = (char)(random.Next(32, 127));
                            break;
                        case 2:
                            if (mutated.Length > 1)
                                mutated = mutated.Take(i).Concat(mutated.Skip(i + 1)).ToArray();
                            break;
                    }
                }
            }
            return new string(mutated);
        }

        /// <summary>
        /// Производит кроссовер двух родительских строк, создавая потомка
        /// </summary>
        /// <param name="parent1"> Первая родительская строка </param>
        /// <param name="parent2"> Вторая родительская строка </param>
        /// <returns> Потомок, полученный в результате кроссовера </returns>
        private string Crossover(string parent1, string parent2)
        {
            int crossoverPoint = random.Next(0, Math.Min(parent1.Length, parent2.Length));
            return parent1.Substring(0, crossoverPoint) + parent2.Substring(crossoverPoint);
        }

        /// <summary>
        /// Запускает генетический алгоритм для восстановления строки к целевой строке.
        /// </summary>
        /// <param name="target"> Целевая строка, к которой стремится алгоритм </param>
        /// <param name="populationSize"> Размер популяции индивидов в каждом поколении </param>
        /// <param name="mutationRate"> Вероятность мутации для каждого символа в гене. Значение должно быть в пределах [0, 1] </param>
        public void RunGeneticAlgorithm(string target, int populationSize, double mutationRate)
        {
            List<string> population = new List<string>();
            for (int i = 0; i < populationSize; i++)
            {
                population.Add(GenerateRandomString(target.Length));
            }

            int generation = 0;

            while (true)
            {
                List<Tuple<string, int>> fitnessScores = new List<Tuple<string, int>>();
                foreach (string individual in population)
                {
                    int fitness = CalculateFitness(individual, target);
                    fitnessScores.Add(new Tuple<string, int>(individual, fitness));
                }

                var sortedPopulation = fitnessScores.OrderByDescending(t => t.Item2).ToList();

                generationCallback?.Invoke(generation, sortedPopulation[0].Item1, target);

                if (sortedPopulation[0].Item2 == target.Length)
                {
                    Console.WriteLine("Target reached!");
                    break;
                }

                List<string> newPopulation = new List<string>();

                for (int i = 0; i < populationSize / 2; i++)
                {
                    string parent1 = sortedPopulation[i].Item1;
                    string parent2 = sortedPopulation[i + 1].Item1;

                    string child1 = Crossover(parent1, parent2);
                    string child2 = Crossover(parent2, parent1);

                    child1 = Mutate(child1, mutationRate);
                    child2 = Mutate(child2, mutationRate);

                    newPopulation.Add(child1);
                    newPopulation.Add(child2);
                }

                population = newPopulation;
                generation++;
            }
        }
    }
}
