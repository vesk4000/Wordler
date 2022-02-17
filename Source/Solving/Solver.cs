using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace wordler
{
	class Solver : ITaskable<string> {
		// TODO: Implement ability to sort the list in multiple ways
		// Prolly gonna do that with a generic in the Solver
		// So something like Solver<TComparer> : ITaskable<string> where TComparer : IComparer
		public SortedList<double, string> gradedWords
			= new SortedList<double, string>(new DuplicateKeyComparer<double>());
		// A thread lock for gradedWords
		public int usingGradedWords = 0;

		private int topResultsToDisplay;

		private BruteForce solutions;

		public Solver(int topResultsToDisplay = 10) {
			this.topResultsToDisplay = topResultsToDisplay;
			List<string> words = new List<string> { "apple", "crook", "brook", "books", "qwert"};
			solutions = new BruteForce(this, words, words);
		}

		public void Poll(
			out int partsDone,
			out int partTotal,
			out List<(string Label, double Value)> chartElements,
			out string[] footnotes,
			out string currentResult
		) {
			partTotal = 100;
			partsDone = 10;
			footnotes = new string[] { "footnote 1", "footnote 2" };
			currentResult = "wordl";
			chartElements = new List<(string, double)>();

			if (0 == Interlocked.Exchange(ref usingGradedWords, 1)) {
				int i = 0;
				foreach (KeyValuePair<double, string> pair in gradedWords) {
					if (i > topResultsToDisplay)
						break;
					chartElements.Add((pair.Value, pair.Key));
					++i;
				}

				Interlocked.Exchange(ref usingGradedWords, 0);
			}
			
		}

		public void AddWord(string word, double grade) {
			if(0 == Interlocked.Exchange(ref usingGradedWords, 1)) {
				gradedWords.Add(grade, word);

				Interlocked.Exchange(ref usingGradedWords, 0);
			}
		}
	}
}
