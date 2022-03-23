using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wordler
{
	[SolutionNames("hdd-entropic|hdd|hdde")]
	[KeyComparer(typeof(DuplicateKeyComparerDescending<double>))]
	class HDDEntropic : Solution
	{
		public HDDEntropic(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues, bool hard, bool cache)
			: base(solver, gradeableWords, potentialComputerWords, wordClues, hard, cache) { }

		public override void GradeWords(CancellationToken cancellationToken)
		{
			List<string> gradeableWordsTemp = new List<string>(gradeableWords);

			gradeableWordsTemp.RemoveAll(e => !wordClues.Match(e));

			int letterListCount = gradeableWords.Count;
			Dictionary<char, double>[] letterProbability = new Dictionary<char, double>[5];

			//it's a modified version of eto1302's idea of finding the best words
			//but rather than calculating the general probability of the letters
			//this algorithm calculates the probability of a letter in a specific position and based on that formulates the probability of the words

			for (int i = 0; i < 5; ++i)
			{
				letterProbability[i] = new Dictionary<char, double>();
			}

			for (int i = 0; i < 5; ++i)
			{
				for (char j = 'a'; j <= 'z'; ++j)
				{
					int occurances = 0;

					foreach (var word in gradeableWordsTemp)
					{
						occurances += word[i] == j ? 1 : 0;
					}

					letterProbability[i].Add(j, (double)occurances / (double)letterListCount);
				}
			}

			foreach (var word in gradeableWords)
			{
				if (!wordClues.Match(word))
				{
					CacheGradedWord(word, 0);
					continue;
				}

				double probability = 0;
				int index = 0;

				foreach (var letter in word)
				{
					probability += (letterProbability[index][letter] / Math.Log(letterProbability[index][letter]));

					++index;
				}

				CacheGradedWord(word, -probability);
			}
		}
	}
}