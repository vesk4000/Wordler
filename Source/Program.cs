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
        public static List<string> shortWordList = File.ReadAllText(@"..\..\..\shortwordlist.txt").Split(";").ToList();
        public static List<string> longWordList = File.ReadAllText(@"..\..\..\longwordlist.txt").Split(";").ToList();
        static void Main(string[] args)
        {            
            CalculateProbabilities(shortWordList);
            if(Console.ReadLine() == "1") Solver.EtoSolve(shortWordList, shortWordList);
            else Solver.GiveClues();
        }

        private static void CalculateProbabilities(List<string> wordList)
        {
            int letterListCount = wordList.Count * 5;
            var wordlist = wordList;
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
