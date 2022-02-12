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
            Dictionary<string, int> wordGuesses = new Dictionary<string, int>();
            int guesses = 0;
            int failed = 0;
            foreach (string goal in wordList)
            {
                if (goal == "ionic") ;
                WordClues wordClues = new WordClues();
                int guessCount = 0;
                string guess = string.Empty;
                bool[] found = new bool[5];

                while (guess != goal)
                {
                    guess = wordProbability.Keys.FirstOrDefault(e => wordClues.Match(e));
                    wordClues = GenerateClues(wordClues, guess, goal);
                    foreach (var green in wordClues.Greens)
                    {
                        found[green.Key] = true;
                    }
                    guessCount++;
                    if (wordClues.Greens.Count >= 4) break;
                }
                if (wordClues.Greens.Count != 5)
                {
                    int remainingIndex = Array.IndexOf(found, false);
                    HashSet<char> remaining = wordProbability.Keys.Where(e => wordClues.Match(e)).Select(e => e[remainingIndex]).ToHashSet();
                    remaining = remaining.OrderByDescending(e => Program.letterProbability[e]).ToHashSet();
                    char[] guessChar = new char[5];

                    for (int i = 0; i < remaining.Count; i++)
                    {
                        int index = i % 5;
                        if (wordClues.Greens.ContainsValue(remaining.ElementAt(i)))
                        {
                            char temp = guessChar[remainingIndex];
                            guessChar[index] = temp;
                            guessChar[remainingIndex] = remaining.ElementAt(i);
                        }
                        else
                        {
                            if (guessChar[index] == 0) guessChar[index] = remaining.ElementAt(i);
                            else
                            {
                                index++;
                                guessChar[index] = remaining.ElementAt(i);
                            }
                        }
                        if (index == 4)
                        {
                            guess = string.Join("", guessChar);
                            guess = TryMatch(wordClues, guess, goal, remainingIndex);
                            Array.Clear(guessChar, 0, guessChar.Length);
                        }
                    }
                    if (guessChar.Any(e => e != 0))
                    {
                        if (guessChar.Count(e => e != 0) < 6 - guessCount)
                        {
                            foreach (var letter in guessChar.Where(e => e != 0))
                            {
                                guess = string.Concat(Enumerable.Repeat(string.Join("", letter), 5));
                                guess = TryMatch(wordClues, guess, goal, remainingIndex);
                            }
                        }
                        else
                        {
                            guess = string.Concat(Enumerable.Repeat(string.Join("", guessChar.Where(e => e != 0)), 5)).Substring(0, 5);
                            guess = TryMatch(wordClues, guess, goal, remainingIndex);
                        }


                    }
                    else if (guess != goal)
                    {
                        remaining = wordProbability.Keys.Where(e => wordClues.Match(e) && e[remainingIndex] != guess[remainingIndex]).Select(e => e[remainingIndex]).ToHashSet();
                        var temp = guess.ToCharArray();
                        temp[remainingIndex] = remaining.First();
                        guess = string.Join("", temp);
                        guess = TryMatch(wordClues, guess, goal, remainingIndex);

                    }

                }
                guesses += guessCount;
                if (guessCount > 6 || guess != goal) { failed++; wordGuesses.Add(goal, guessCount); }
                //Console.WriteLine(goal);
            }
            Console.WriteLine($"Average: {(double)((double)guesses / (double)wordList.Count)}");
            Console.WriteLine($"Failed: {failed} -> {((double)((double)failed / (double)wordList.Count) * 100):F2}%");
            wordGuesses = wordGuesses.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            foreach (var item in wordGuesses)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        private static string TryMatch(WordClues wordClues, string guess, string goal, int remainingIndex)
        {
            if (guess == "cicic") ;
            int yellowCount = wordClues.Yellows.Count();
            wordClues = GenerateClues(wordClues, guess, goal);
            if (wordClues.Greens.Count == 5)
            {
                return GuessGreens(wordClues.Greens);
            }
            if (wordClues.Yellows.Count > yellowCount)
            {
                if (wordClues.Yellows.Count(e => !wordClues.Greens.ContainsValue(e.Item2)) == 0) return guess;
                var temp = new Dictionary<int, char>();
                temp.Add(remainingIndex, wordClues.Yellows.FirstOrDefault(e => !wordClues.Greens.ContainsValue(e.Item2)).Item2);
                wordClues.Add(temp, new HashSet<Tuple<int, char>>(), new HashSet<char>());
                return GuessGreens(wordClues.Greens);
            }
            return guess;
        }

        public static void GiveClues()
        {
            wordProbability = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(@"..\..\..\wordProbability.txt"));
            WordClues wordClues = new WordClues();
            string guess = string.Empty;
            bool[] found = new bool[5];
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
                    found[index] = true;
                    greens.Add(index, letter);
                }
                wordClues.Add(greens, yellows, greys);
                if (wordClues.Greens.Count >= 4) break;
            }
            if (wordClues.Greens.Count != 5)
            {
                int remainingIndex = Array.IndexOf(found, false);
                HashSet<char> remaining = wordProbability.Keys.Where(e => wordClues.Match(e)).Select(e => e[remainingIndex]).ToHashSet();
                remaining = remaining.OrderByDescending(e => Program.letterProbability[e]).ToHashSet();
                char[] guessChar = new char[5];
                while (wordClues.Greens.Count != 5)
                {
                    for (int i = 0; i < remaining.Count; i++)
                    {
                        int index = i % 5;
                        if (wordClues.Greens.ContainsValue(remaining.ElementAt(i)))
                        {
                            char temp = guessChar[remainingIndex];
                            guessChar[index] = temp;
                            guessChar[remainingIndex] = remaining.ElementAt(i);
                        }
                        else
                        {
                            guessChar[index] = remaining.ElementAt(i);
                        }
                        if (index == 4)
                        {
                            Console.WriteLine(string.Join("", guessChar));
                            Console.Write("Enter Greys without spaces: ");
                            HashSet<char> greys = Console.ReadLine().ToHashSet();
                            Console.Write("Enter Yellows in format({letter}{position}): ");
                            HashSet<Tuple<int, char>> yellows = new HashSet<Tuple<int, char>>();
                            foreach (var item in Console.ReadLine().Split().ToArray())
                            {
                                if (item == string.Empty) break;
                                var letter = item[0];
                                if (!yellows.Contains(new Tuple<int, char>(item[1] - '0', letter)))
                                {
                                    wordClues.Greens.Add(item[1] - '0', letter);
                                    Console.WriteLine(GuessGreens(wordClues.Greens));
                                    return;
                                }
                            }
                            Console.Write("Enter Greens in format({letter}{position}): ");
                            Dictionary<int, char> greens = new Dictionary<int, char>();
                            foreach (var item in Console.ReadLine().Split().ToArray())
                            {
                                if (item == string.Empty) break;
                                if (!wordClues.Greens.ContainsKey(item[1] - '0'))
                                {
                                    wordClues.Greens.Add(item[1] - '0', item[0]);
                                    Console.WriteLine(GuessGreens(wordClues.Greens));
                                    return;
                                }
                            }
                            wordClues.Add(greens, yellows, greys);
                            Array.Clear(guessChar, 0, guessChar.Length);
                        }
                    }
                }
            }

        }

        private static string GuessGreens(Dictionary<int, char> greens) => string.Join("", greens.OrderBy(e => e.Key).ToDictionary(e => e.Key, e => e.Value).Values);

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