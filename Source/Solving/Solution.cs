using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Wordler {
	abstract class Solution {
		public List<string> gradeableWords;
		public List<string> potentialComputerWords;
		private List<(string word, double grade)> cache = new List<(string word, double grade)>();
		private object usingCache = new Object();
		protected object updateLock = new Object();
		public Solver solver;

		protected WordClues wordClues;

		private Thread solution;
		private Thread cacher;

		public abstract void GradeWords();

		public Solution(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues) {
			this.solver = solver;
			this.gradeableWords = gradeableWords;
			this.potentialComputerWords = potentialComputerWords;
			this.wordClues = wordClues;
			cacher = new Thread(new ThreadStart(ContinuouslyAddGradedWords));
			cacher.Start();
			solution = new Thread(new ThreadStart(GradeWords));
			solution.Start();
		}

		private void ContinuouslyAddGradedWords() {
			bool endFlag = false;
			while(!endFlag) {
				lock(updateLock) { }

				lock(solver.usingGradedWords) {

					lock(usingCache) {
						/*if (cache.Count == 0)
							endFlag = true;*/

						foreach ((string word, double grade) cachedGradedWord in cache) {
							solver.gradedWords.Add(cachedGradedWord.grade, cachedGradedWord.word);
						}
						cache.Clear();
					}
				}
			}
		}
		
		protected void CacheGradedWord(string word, double grade) {
			lock (usingCache) {
				cache.Add((word, grade));
			}
		}
	}
}
