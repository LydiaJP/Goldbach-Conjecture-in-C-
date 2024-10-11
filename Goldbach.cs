using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class GoldbachConjecture
{
    static List<int> primes = new List<int>();
    static HashSet<int> primeSet;
    static bool[] isPrime;

    static void Main(string[] args)
    {
        // Step 1: Generate primes up to 100,000
        GeneratePrimes(100000);

        if (args.Length > 0)
        {
            // Step 2: Write results from the file to my_result.txt
            string resultFilePath = @"C:\Users\19107\Downloads\Goldbach\Goldbach\bin\Debug\net8.0\my_result.txt";

            // Ensure file exists or create it (in overwrite mode)
            using (StreamWriter writer = new StreamWriter(resultFilePath, false)) { }

            // Read the numbers from the file and write results to my_result.txt
            List<int> numbersFromFile = GetNumbersFromFile(args[0]);

            foreach (int n in numbersFromFile)
            {
                List<(int, int)> goldbachPairs = FindGoldbachPairs(n);
                OutputResultsToFile(n, goldbachPairs, resultFilePath); // Write results to the file
            }

            Console.WriteLine("Results written to my_result.txt successfully.");
        }
        else
        {
            // Step 2: If no file is provided, process default numbers and print them to the console
            List<int> defaultNumbers = new List<int> { 3, 4, 14, 26, 100 };
            foreach (int n in defaultNumbers)
            {
                List<(int, int)> goldbachPairs = FindGoldbachPairs(n);
                PrintToConsole(n, goldbachPairs); // Print default results to the console
            }
        }
    }

    // Generate primes using Sieve of Eratosthenes
    static void GeneratePrimes(int limit)
    {
        isPrime = new bool[limit + 1];
        Array.Fill(isPrime, true);
        isPrime[0] = isPrime[1] = false;

        for (int p = 2; p * p <= limit; p++)
        {
            if (isPrime[p])
            {
                for (int multiple = p * p; multiple <= limit; multiple += p)
                {
                    isPrime[multiple] = false;
                }
            }
        }

        primeSet = new HashSet<int>();
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
                primeSet.Add(i); // Add to HashSet for faster lookup
            }
        }
    }

    // Find Goldbach pairs for a given number n
    static List<(int, int)> FindGoldbachPairs(int n)
    {
        List<(int, int)> goldbachPairs = new List<(int, int)>();

        // If the number is less than 4 or odd, return an empty list (no pairs)
        if (n < 4 || n % 2 != 0)
            return goldbachPairs;

        // Find Goldbach pairs for even numbers >= 4
        foreach (int p in primes)
        {
            if (p > n / 2)
                break;

            int q = n - p;
            if (primeSet.Contains(q))
            {
                goldbachPairs.Add((p, q));
            }
        }

        return goldbachPairs;
    }

    // Read numbers from file
    static List<int> GetNumbersFromFile(string filename)
    {
        return File.ReadAllLines(filename)
                   .Select(int.Parse)
                   .Where(x => x % 2 == 0 && x >= 4 && x <= 100000) // Only even numbers >= 4
                   .ToList();
    }

    // Output results to console (for default numbers)
    static void PrintToConsole(int n, List<(int, int)> goldbachPairs)
    {
        if (n < 4 || n % 2 != 0)  // Handle odd and numbers < 4
        {
            Console.WriteLine($"We found 0 Goldbach pair(s) for {n}.");
        }
        else
        {
            Console.WriteLine($"We found {goldbachPairs.Count} Goldbach pair(s) for {n}.");
            foreach (var pair in goldbachPairs.OrderBy(p => p.Item1))
            {
                Console.WriteLine($"{n} = {pair.Item1} + {pair.Item2}");
            }
        }
        Console.WriteLine(); // Blank line for separation
    }

    // Output results to file (for data.txt)
    static void OutputResultsToFile(int n, List<(int, int)> goldbachPairs, string resultFilePath)
    {
        // Append the results to the file
        using (StreamWriter writer = new StreamWriter(resultFilePath, true))
        {
            if (n < 4 || n % 2 != 0)  // Handle odd and numbers < 4
            {
                writer.WriteLine($"We found 0 Goldbach pair(s) for {n}.");
            }
            else
            {
                writer.WriteLine($"We found {goldbachPairs.Count} Goldbach pair(s) for {n}.");
                foreach (var pair in goldbachPairs.OrderBy(p => p.Item1))
                {
                    writer.WriteLine($"{n} = {pair.Item1} + {pair.Item2}");
                }
            }

            writer.WriteLine(); // Blank line for separation
        }
    }
}
