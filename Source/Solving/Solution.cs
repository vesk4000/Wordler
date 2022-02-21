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
		public bool hard;

		protected WordClues wordClues;

		private Thread solution;
		private Thread cacher;

		public abstract void GradeWords();

		public Solution(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords, WordClues wordClues, bool hard) {
			this.solver = solver;
			this.gradeableWords = gradeableWords;
			this.potentialComputerWords = potentialComputerWords;
			this.wordClues = wordClues;
			this.hard = hard;
			solution = new Thread(new ThreadStart(GradeWords));
			solution.Start();
			cacher = new Thread(new ThreadStart(ContinuouslyAddGradedWords));
			cacher.Start();
		}

		private void ContinuouslyAddGradedWords() {
			return;
			bool endFlag = false;
			while(!endFlag) {
				lock(updateLock) { }

				lock(solver.usingGradedWords) {

					lock(usingCache) {
						if (cache.Count == 0)
							endFlag = true;

						foreach ((string word, double grade) cachedGradedWord in cache) {
							solver.gradedWords.Add(cachedGradedWord.grade, cachedGradedWord.word);
						}

						if(wordClues.IsEmpty())
							Cacher.AddCache(cache);

						cache.Clear();
					}
				}

				Thread.Sleep(100);
			}
		}
		
		protected void CacheGradedWord(string word, double grade) {
			lock(solver.usingGradedWords) {
				solver.gradedWords.Add(grade, word);
				if(wordClues.IsEmpty())
					Cacher.AddCache(new List<(string Key, double Value)> { (word, grade) });
			}
			/*lock (usingCache) {
				cache.Add((word, grade));
			}*/
		}
	}
}
