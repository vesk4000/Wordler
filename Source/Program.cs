using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using System.Text.Json;
using System.IO;

namespace Wordler
{
    class Program
    {
        public static Dictionary<char, double> letterProbability;
        static void Main(string[] args)
        {            
            CalculateProbabilities();
            if(Console.ReadLine() == "1")Solver.EtoSolve();
            else Solver.GiveClues();
        }

        private static void CalculateProbabilities()
        {
            int letterListCount = Solver.wordList.Count * 5;
            var wordlist = Solver.wordList;
            letterProbability = new Dictionary<char, double>();
            for (char i = 'a'; i <= 'z'; ++i)
            {
                int occurances = 0;
                foreach (var word in wordlist)
                {
                    occurances += word.Count(e => e == i);
                }
                letterProbability.Add(i, (double)occurances / (double)letterListCount);
            }
            letterProbability = letterProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            Dictionary<string, double> wordProbability = new Dictionary<string, double>();
            long wordFrequencySum = 570765890;
            foreach (var word in wordlist)
            {
                double probability = 0;
                foreach (var letter in string.Join("", word.ToCharArray().Distinct()))
                {
                    probability += (letterProbability[letter] / Math.Log(letterProbability[letter]));
                    
                }
                wordProbability.Add(word, -probability);
            }
            wordProbability = wordProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            File.WriteAllText(@"..\..\..\wordProbability.txt", JsonSerializer.Serialize(wordProbability));
        }
    }
}
