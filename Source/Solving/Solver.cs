using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace wordler
{
	class Solver : ITaskable<string> {
		// TODO: Implement ability to sort the list in multiple ways
		// Prolly gonna do that with a generic in the Solver
		// So something like Solver<TComparer> : ITaskable<string> where TComparer : IComparer
		public SortedList<double, string> gradedWords
			= new SortedList<double, string>(new DuplicateKeyComparerDescending<double>());
		// A thread lock for gradedWords
		public object usingGradedWords = new Object();

		private int topResultsToDisplay;

		private string wordListPath;

		public bool hard;

		private int numThreads;

		public WordClues wordClues;

		private List<Solution> solutions;

		public Solver(
			bool hard,
			string wordListPath,
			int topResultsToDisplay,
			int numThreads,
			WordClues wordClues,
			Type solution
		) {
			this.hard = hard;
			this.wordListPath = wordListPath;
			this.topResultsToDisplay = topResultsToDisplay;
			this.numThreads = numThreads;
			this.wordClues = wordClues;

			var customAttributes = (KeyComparerAttribute[])solution.GetCustomAttributes(typeof(KeyComparerAttribute), true);
			var comparerAttribute = customAttributes[0];
			gradedWords = new SortedList<double, string>((IComparer<double>)Activator.CreateInstance(comparerAttribute.KeyComparer));

			List<string> words = File.ReadAllLines(wordListPath).ToList();

			List<IEnumerable<string>> wordsChunkedEnumerable = words.Chunk(numThreads).ToList();
			List<List<string>> wordsChunked = new List<List<string>>();

			foreach(IEnumerable<string> chunk in wordsChunkedEnumerable) {
				wordsChunked.Add(chunk.ToList());
			}
			

			solutions = new List<Solution>();
			for(int i = 0; i < numThreads; ++i) {
				solutions.Add(new BruteForce(this, wordsChunked[i], words, wordClues));
			}
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

			lock(usingGradedWords) {
				int i = 0;
				foreach (KeyValuePair<double, string> pair in gradedWords) {
					if (i >= topResultsToDisplay)
						break;
					chartElements.Add((pair.Value, pair.Key));
					++i;
				}
			}
			
		}

		public void AddWord(string word, double grade) {
			lock(usingGradedWords) {
				gradedWords.Add(grade, word);

				Interlocked.Exchange(ref usingGradedWords, 0);
			}
		}
	}
}
