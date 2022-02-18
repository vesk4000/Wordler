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
		public static List<string> wordProbabilityList = new List<string>();

		public static void IcySolve()
		{
			int guesses = 0;
			int failed = 0;

			ProbabilityCalculatorHDD.CalculateProbabilities(wordList, false);

			foreach (string goal in wordList)
			{
				wordProbabilityList.Clear();
				wordProbabilityList.AddRange(wordProbability.Keys.ToList());

				WordClues wordClues = new WordClues();
				int guessCount = 0;
				string guess = string.Empty;

				while (guess != goal)
				{
					guess = wordProbabilityList.First();
					wordClues = GenerateClues(wordClues, guess, goal);
					
					guessCount++;

					wordProbabilityList.RemoveAll(e => !wordClues.Match(e));

					ProbabilityCalculatorHDD.CalculateProbabilities(wordProbabilityList, true);
				}

				guesses += guessCount;
				
				if (guessCount > 6) { failed++; Console.WriteLine(goal); Console.WriteLine($"{goal}: {guessCount}"); }
			}
			Console.WriteLine($"Average: {(double)((double)guesses / (double)wordList.Count)}");
			Console.WriteLine($"Failed: {failed} -> {((double)((double)failed / (double)wordList.Count) * 100):F2}%");
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
					currentClues.Yellows.Add((i, guessWord[i]));
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
