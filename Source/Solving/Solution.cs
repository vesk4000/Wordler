using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace wordler {
	abstract class Solution {
		public List<string> gradeableWords;
		public List<string> potentialComputerWords;
		private List<(string word, double grade)> cache = new List<(string word, double grade)>();
		private int usingCache = 0;
		protected int updateLock = 0;
		public Solver solver;

		private Thread solution;
		private Thread cacher;

		public abstract void GradeWords();

		public Solution(Solver solver, List<string> gradeableWords, List<string> potentialComputerWords) {
			this.solver = solver;
			this.gradeableWords = gradeableWords;
			this.potentialComputerWords = potentialComputerWords;
			cacher = new Thread(new ThreadStart(ContinuouslyAddGradedWords));
			cacher.Start();
			solution = new Thread(new ThreadStart(GradeWords));
			solution.Start();
		}

		private void ContinuouslyAddGradedWords() {
			bool endFlag = false;
			while(!endFlag) {
				if (0 == Interlocked.Exchange(ref updateLock, 1)) {
					Interlocked.Exchange(ref updateLock, 0);
				}

				if(0 == Interlocked.Exchange(ref solver.usingGradedWords, 1)) {

					if(0 == Interlocked.Exchange(ref usingCache, 1)) {
						/*if (cache.Count == 0)
							endFlag = true;*/

						foreach ((string word, double grade) cachedGradedWord in cache) {
							solver.gradedWords.Add(cachedGradedWord.grade, cachedGradedWord.word);
						}
						cache.Clear();

						Interlocked.Exchange(ref usingCache, 0);
					}


					Interlocked.Exchange(ref solver.usingGradedWords, 0);
				}
			}
		}
		
		protected void CacheGradedWord(string word, double grade) {
			if(0 == Interlocked.Exchange(ref usingCache, 1)) {
				cache.Add((word, grade));

				Interlocked.Exchange(ref usingCache, 0);
			}
		}
	}
}
