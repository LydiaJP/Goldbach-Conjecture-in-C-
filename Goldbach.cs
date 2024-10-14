using System;
using System.Collections.Generic;
using System.IO;

class GoldbachConjecture
{
    static List<int> GetPrimes(int maxNumber)
    {
        bool[] isPrime = new bool[maxNumber + 1];
        Array.Fill(isPrime, true);
        isPrime[0] = isPrime[1] = false;

        for (int i = 2; i * i <= maxNumber; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= maxNumber; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        List<int> primes = new List<int>();
        for (int i = 2; i <= maxNumber; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }
        return primes;
    }

    static List<(int, int)> Goldbach(int value, HashSet<int> primeSet)
    {
        List<(int, int)> result = new List<(int, int)>();

        foreach (int prime in primeSet)
        {
            if (prime > value / 2) break;

            int difference = value - prime;
            if (primeSet.Contains(difference))
            {
                result.Add((prime, difference));
            }
        }

        return result;
    }
    static void PrintGoldbach(int value, List<(int, int)> primePairs)
    {
        if (primePairs.Count == 0)
        {
            Console.WriteLine($"We found no Goldbach pair(s) for {value}.");
        }
        else
        {
            Console.WriteLine($"We found {primePairs.Count} Goldbach pair(s) for {value}:");
            foreach (var (p, q) in primePairs)
            {
                Console.WriteLine($"{value} = {p} + {q}");
            }
        }

        Console.WriteLine();  // Blank line
    }
    static List<int> ReadInput(string[] args)
    {
        List<int> data = new List<int>();

        if (args.Length > 0 && File.Exists(args[0])) // Handle file input.
        {
            foreach (string line in File.ReadLines(args[0]))
            {
                if (int.TryParse(line, out int num) && num >= 4 && num <= 100000 && num % 2 == 0)
                {
                    data.Add(num);
                }
            }
        }
        else
        {
            // Try to parse command-line arguments as numbers.
            foreach (string arg in args)
            {
                if (int.TryParse(arg, out int num))
                {
                    data.Add(num);
                }
            }

            // Use default values if no valid numbers were provided.
            if (data.Count == 0)
            {
                data = new List<int> { 3, 4, 14, 26, 100 };
            }
        }

        return data;
    }

    static void Main(string[] args)
    {
        const int MaxValue = 100000;
        List<int> primes = GetPrimes(MaxValue);
        HashSet<int> primeSet = new HashSet<int>(primes);

        List<int> inputNumbers = ReadInput(args);

        foreach (int value in inputNumbers)
        {
            // Only attempt Goldbach pairs for valid even numbers.
            if (value < 4 || value > MaxValue || value % 2 != 0)
            {
                Console.WriteLine($"We found no Goldbach pair(s) for {value}.");
                continue;
            }

            List<(int, int)> primePairs = Goldbach(value, primeSet);
            PrintGoldbach(value, primePairs);
        }
    }
}
