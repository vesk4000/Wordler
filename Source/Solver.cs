using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Wordler
{
    public class Solver
    {
        public static string goal = "cigar";

        public static List<string> wordList = File.ReadAllText(@"..\..\..\wordlist.txt").Split(";").ToList();
        public static Dictionary<string, double> wordProbability;

        public static void EtoSolve()
        {
            wordProbability = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(@"..\..\..\wordProbability.txt"));
            int guesses = 0;
            foreach (string goal in wordList)
            {
                WordClues wordClues = new WordClues();
                int guessCount = 0;
                string guess = string.Empty;

                while (guess != goal)
                {
                    guess = wordProbability.Keys.FirstOrDefault(e => wordClues.Match(e));
                    wordClues = GenerateClues(wordClues, guess, goal);
                    guessCount++;
                }
                guesses += guessCount;
                Console.WriteLine($"{goal}: {guessCount}");
            }
            Console.WriteLine($"Average: {(double)((double)guesses / (double)wordList.Count)}");
        }

        public static string InitSolve(List<WordClues> wordClues = null)
        {
            OrderedDictionary wordsOrder = new OrderedDictionary();

            foreach (string bestWord in wordList)
            {
                decimal coefficient = 0;
                foreach (string computerWord in wordList)
                {
                    WordClues tempClues = GenerateClues(new WordClues(), bestWord, computerWord);
                    int matchWordsCount = 0;
                    foreach (string tempWord in wordList)
                    {
                        if (tempClues.Match(tempWord))
                            ++matchWordsCount;
                    }
                    int notMatchWordsCount = wordList.Count - matchWordsCount;
                    coefficient += (matchWordsCount / notMatchWordsCount);
                    //Console.WriteLine("current: " + bestWord + " : " + coefficient);
                }
                wordsOrder.Add(bestWord, coefficient);
                Console.WriteLine(bestWord + " : " + coefficient);
            }
            foreach (DictionaryEntry word in wordsOrder)
            {
                Console.WriteLine($"{word.Key} : {word.Value}");
            }
            return "Yomum";
        }

        public static WordClues GenerateClues(WordClues currentClues, string guessWord, string computerWord)
        {
            for (int i = 0; i < guessWord.Length; ++i)
            {
                if (guessWord[i] == computerWord[i])
                {
                    if (!currentClues.Greens.ContainsKey(i)) currentClues.Greens.Add(i, guessWord[i]);
                }
                else if (computerWord.Contains(guessWord[i]))
                {
                    currentClues.Yellows.Add(new Tuple<int, char>(i, guessWord[i]));
                }
                else
                {
                    currentClues.Greys.Add(guessWord[i]);
                }
            }
            return currentClues;
        }
    }
}
