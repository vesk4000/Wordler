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
		static void Main(string[] args)
		{
			Solver.IcySolve();
			//Solver.GiveClues();
		}

		public static void CalculateProbabilities(List<string> wordlist)
		{
            #region Eto's probability calculation
            //int letterListCount = Solver.wordList.Count * 5;
            //Dictionary<char, double> letterProbability = new Dictionary<char, double>();
            //for (char i = 'a'; i <= 'z'; ++i)
            //{
            //    int occurances = 0;
            //    foreach (var word in wordlist)
            //    {
            //        occurances += word.Count(e => e == i);
            //    }
            //    letterProbability.Add(i, (double)occurances / (double)letterListCount);
            //}
            //letterProbability = letterProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            //Dictionary<string, double> wordProbability = new Dictionary<string, double>();
            //foreach (var word in wordlist)
            //{
            //    double probability = 0;
            //    foreach (var letter in string.Join("", word.ToCharArray().Distinct()))
            //    {
            //        probability += (letterProbability[letter] / Math.Log(letterProbability[letter]));
            //    }
            //    wordProbability.Add(word, -probability);
            //}
            //Solver.wordProbability = wordProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            #endregion

            #region icyDenev's way of finding best words
            //it's a modified version of eto1302's idea of finding the best words
            //but rather than calculating the general probability of the letters
            //this algorithm calculates the probability of a letter in a specific position and based on that formulates the probability of the words

            int letterListCount = Solver.wordList.Count;
			Dictionary<char, double>[] letterProbability = new Dictionary<char, double>[5];

			for (int i = 0; i < 5; ++i)
			{
				letterProbability[i] = new Dictionary<char, double>();
			}

			for (int i = 0; i < 5; ++i)
			{
				for (char j = 'a'; j <= 'z'; ++j)
				{
					int occurances = 0;

					foreach (var word in wordlist)
					{
						occurances += word[i] == j ? 1 : 0;
					}

					letterProbability[i].Add(j, (double)occurances / (double)letterListCount);
				}
			}

			letterProbability = letterProbability.Select(e => e.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value)).ToArray();
			Dictionary<string, double> wordProbability = new Dictionary<string, double>();

			foreach (var word in wordlist)
			{
				double probability = 0;
				int index = 0;

				foreach (var letter in word)
				{
					probability += (letterProbability[index][letter] / Math.Log(letterProbability[index][letter]));

					++index;
				}

				wordProbability.Add(word, -probability);
			}
			#endregion

			Solver.wordProbability = wordProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
			//File.WriteAllText(@"..\..\..\wordProbability.txt", JsonSerializer.Serialize(Solver.wordProbability));
		}
	}
}
