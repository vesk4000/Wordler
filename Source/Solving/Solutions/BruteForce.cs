using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace wordler {
	class BruteForce : Solution {
		public BruteForce(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords)
			: base(solver, gradeableWords, potentialComputerWords) { }

		public override void GradeWords() {
			foreach(string gradeableWord in gradeableWords) {
				if(0 == Interlocked.Exchange(ref updateLock, 1)) {
					double sumReductions = 0;

					foreach (string potentialComputerWord in potentialComputerWords) {

						WordClues wordClues = new WordClues(gradeableWord, potentialComputerWord);
						int numReducedWords = 0;

						foreach (string computerWord in potentialComputerWords) {
							if (!wordClues.Match(computerWord))
								++numReducedWords;
						}

						double reduction = (double)numReducedWords / potentialComputerWords.Count;
						sumReductions += reduction;
					}

					double avgReduction = sumReductions / potentialComputerWords.Count;

					CacheGradedWord(gradeableWord, avgReduction);

					Interlocked.Exchange(ref updateLock, 0);
				}
			}
		}
	}
}
