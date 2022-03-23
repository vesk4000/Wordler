using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Wordler {
	[SolutionNames("bruteforce|brute-force")]
	[KeyComparer(typeof(DuplicateKeyComparerDescending<double>))]
	class BruteForce : Solution {
		public BruteForce(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues, bool hard, bool cache)
			: base(solver, gradeableWords, potentialComputerWords, wordClues, hard, cache) { }

		// gradeableWords
		// potentialComputerWords
		// hard
		// wordClues
		// CacheGradedWord(string gradeableWord, double grade);
		public override void GradeWords() {
			foreach(string gradeableWord in gradeableWords) {
				lock(updateLock) {
					double sumReductions = 0;

					if(hard && !wordClues.Match(gradeableWord)) {
						CacheGradedWord(gradeableWord, 0);
						continue;
					}

					int numPotCompWords = potentialComputerWords.Count;

					

					foreach (string potentialComputerWord in potentialComputerWords) {
						if (!wordClues.Match(potentialComputerWord)) {
							--numPotCompWords;
							continue;
						}

						WordClues tempWordClues = new WordClues(gradeableWord, potentialComputerWord) + wordClues;
						int numReducedWords = 0;
						int numReducableWords = 0;
						foreach (string computerWord in potentialComputerWords) {
							if(wordClues.Match(computerWord)) {
								++numReducableWords;
								if(!tempWordClues.Match(computerWord)) {
									++numReducedWords;
								}
							}
								
						}
						
						if(wordClues.Match(gradeableWord)) {
							++numReducableWords;
							++numReducedWords;
						}
						double reduction = 0;
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
