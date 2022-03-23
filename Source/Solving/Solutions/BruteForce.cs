using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Wordler {
	[SolutionNames("bruteforce|brute-force")]
	[KeyComparer(typeof(DuplicateKeyComparerDescending<double>))]
	class BruteForce : Solution {
		private static Dictionary<WordClues, (int ReducedWords, int ReducableWords)> computedWordClues
			= new Dictionary<WordClues, (int ReducedWords, int ReducableWords)>();
		private static object computedWordCluesLock = new Object();


		public BruteForce(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues, bool hard, bool cache)
			: base(solver, gradeableWords, potentialComputerWords, wordClues, hard, cache) { }

		// gradeableWords
		// potentialComputerWords
		// hard
		// wordClues
		// CacheGradedWord(string gradeableWord, double grade);
		public override void GradeWords(CancellationToken cancelToken) {
			foreach(string gradeableWord in gradeableWords) {
				lock(updateLock) {
					double sumReductions = 0;

					if(hard && !wordClues.Match(gradeableWord)) {
						CacheGradedWord(gradeableWord, 0);
						continue;
					}

					int numPotCompWords = potentialComputerWords.Count;

					foreach (string potentialComputerWord in potentialComputerWords) {
						if(cancelToken.IsCancellationRequested)
							return;

						if (!wordClues.Match(potentialComputerWord)) {
							--numPotCompWords;
							continue;
						}

						WordClues tempWordClues = new WordClues(gradeableWord, potentialComputerWord) + wordClues;

						int numReducedWords = 0;
						int numReducableWords = 0;
						//bool toCompute = true;
						/*lock(computedWordCluesLock)
						{
							if(computedWordClues.ContainsKey(tempWordClues))
							{
								toCompute = false;
								numReducedWords = computedWordClues[tempWordClues].ReducedWords;
								numReducableWords = computedWordClues[tempWordClues].ReducableWords;
							}
						}*/

						/*if(toCompute)
						{*/
						foreach(string computerWord in potentialComputerWords)
							if(wordClues.Match(computerWord))
							{
								++numReducableWords;
								if(!tempWordClues.Match(computerWord))
								{
									++numReducedWords;
								}
							}
							/*lock(computedWordCluesLock)
							{
								computedWordClues[tempWordClues] = (numReducedWords, numReducableWords);
							}*/
						//}

						double reduction = 0;
						if(wordClues.Match(gradeableWord))
						{
							++numReducableWords;
							++numReducedWords;
						}
						if(numReducableWords != 0)
							reduction = (double)numReducedWords / numReducableWords;
						sumReductions += reduction;
					}

					double avgReduction = (double)sumReductions / numPotCompWords;

					CacheGradedWord(gradeableWord, avgReduction);
				}
			}
		}
	}
}
