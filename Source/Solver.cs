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
        public static List<string> wordList = File.ReadAllText(@"..\..\..\shortwordlist.txt").Split(";").ToList();
        public static Dictionary<string, double> wordProbability;

        public static void EtoSolve()
        {
            wordProbability = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(@"..\..\..\wordProbability.txt"));
            int guesses = 0;
            int failed = 0;
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
                if (guessCount > 6) { failed++; Console.WriteLine(goal); }
                Console.WriteLine($"{goal}: {guessCount}");
            }
            Console.WriteLine($"Average: {(double)((double)guesses / (double)wordList.Count)}");
            Console.WriteLine($"Failed: {failed} -> {((double)((double)failed / (double)wordList.Count) * 100):F2}%");
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

        public static void GiveClues()
        {
            wordProbability = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(@"..\..\..\wordProbability.txt"));
            WordClues wordClues = new WordClues();
            string guess = string.Empty;
            while (true)
            {
                guess = wordProbability.Keys.FirstOrDefault(e => wordClues.Match(e));
                Console.WriteLine(guess);
                Console.Write("Enter Greys without spaces: ");
                HashSet<char> greys = Console.ReadLine().ToHashSet();
                Console.Write("Enter Yellows in format({letter}{position}): ");
                HashSet<Tuple<int, char>> yellows = new HashSet<Tuple<int, char>>();
                foreach (var item in Console.ReadLine().Split().ToArray())
                {
                    if (item == string.Empty) break;
                    var letter = item[0];
                    var index = item[1] - '0';
                    yellows.Add(new Tuple<int, char>(index, letter));
                }
                Console.Write("Enter Greens in format({letter}{position}): ");
                Dictionary<int, char> greens = new Dictionary<int, char>();
                foreach (var item in Console.ReadLine().Split().ToArray())
                {
                    if (item == string.Empty) break;
                    var letter = item[0];
                    var index = item[1] - '0';
                    greens.Add(index, letter);
                }

                wordClues.Add(greens, yellows, greys);

            }

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
