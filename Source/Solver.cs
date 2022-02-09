using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Wordler
{
	public class Solver
	{
		//TODO change directory
		public static List<string> wordList = File.ReadAllText(@"C:\Users\zala2\Documents\VDM\wordler\wordlist.txt").Split("\",\"").ToList();
		public static string Solve(List<WordClues> wordClues = null)
		{
			OrderedDictionary wordsOrder = new OrderedDictionary();

			foreach(string bestWord in wordList)
			{
				decimal coefficient = 0;
				foreach(string computerWord in wordList)
				{
					WordClues tempClues = GenerateClues(bestWord, computerWord);
					int matchWordsCount = 0;
					foreach(string tempWord in wordList)
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

		public static WordClues GenerateClues(string guessWord, string computerWord)
		{
			WordClues clues = new WordClues();
			for (int i = 0; i < guessWord.Length; ++i)
			{
				if (guessWord[i] == computerWord[i])
				{
					clues.Greens.Add(i, guessWord[i]);
				}
				else if (computerWord.Contains(guessWord[i]))
				{
					clues.Yellows.Add(i, guessWord[i]);
				}
				else
				{
					clues.Greys.Add(guessWord[i]);
				}
			}
			return clues;
		}
	}
}
