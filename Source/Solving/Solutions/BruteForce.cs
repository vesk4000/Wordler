using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace wordler {
	[KeyComparer(typeof(DuplicateKeyComparerAscending<double>))]
	class BruteForce : Solution {
		public BruteForce(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues)
			: base(solver, gradeableWords, potentialComputerWords, wordClues) { }

		public override void GradeWords() {
			foreach(string gradeableWord in gradeableWords) {
				lock(updateLock) {
					double sumReductions = 0;

					foreach (string potentialComputerWord in potentialComputerWords) {
						if (!wordClues.Match(potentialComputerWord))
							continue;

						WordClues tempWordClues = new WordClues(gradeableWord, potentialComputerWord) + wordClues;
						int numReducedWords = 0;

						foreach (string computerWord in potentialComputerWords) {
							if (!tempWordClues.Match(computerWord))
								++numReducedWords;
						}

						double reduction = (double)numReducedWords / potentialComputerWords.Count;
						sumReductions += reduction;
					}

					double avgReduction = sumReductions / potentialComputerWords.Count;

					CacheGradedWord(gradeableWord, avgReduction);
				}
			}
		}
	}
}
