using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Wordler
{
	class Solver : ITaskable<string> {
		public SortedList<double, string> gradedWords
			= new SortedList<double, string>(new DuplicateKeyComparerAscending<double>());

		// A thread lock for gradedWords
		public object usingGradedWords = new Object();

		private int topResultsToDisplay;

		private string wordListPath;

		public bool hard;

		private int numThreads;

		public WordClues wordClues;

		private List<Solution> solutions;

		private int numCachedWords;

		private int numWordsToCalc;

		private Type sol;

		public Solver(
			bool hard,
			string wordListPath,
			int topResultsToDisplay,
			int numThreads,
			WordClues wordClues,
			Type solution,
			string divisionsString
		) {
			this.hard = hard;
			this.wordListPath = wordListPath;
			this.topResultsToDisplay = topResultsToDisplay;
			this.numThreads = numThreads;
			this.wordClues = wordClues;
			this.sol = solution;

			var customAttributes = (KeyComparerAttribute[])solution.GetCustomAttributes(typeof(KeyComparerAttribute), true);
			var comparerAttribute = customAttributes[0];
			gradedWords = new SortedList<double, string>((IComparer<double>)Activator.CreateInstance(comparerAttribute.KeyComparer));

			Cacher.LoadFromWordListPath(wordListPath);
			Cacher.SetSection(hard, solution);

			HashSet<string> wordsPreCache = File.ReadAllLines(wordListPath).ToHashSet();

			List<string> wordsNoCache = wordsPreCache.ToList();
			
			List<(string Key, double Value)> cachedWords = wordClues.IsEmpty() ? Cacher.GetCache<string, double>() : new List<(string Key, double Value)>();
			foreach(var word in cachedWords) {
				if(wordsPreCache.Contains(word.Key)) {
					wordsPreCache.Remove(word.Key);
					gradedWords.Add(word.Value, word.Key);
					++numCachedWords;
				}
			}

			List<string> words = wordsPreCache.ToList();
			

			(int Part, int TotalParts) div = divisionsString.GetDivision() ?? (1, 1);

			List<IEnumerable<string>> wordsDividedEnumerable = words.Chunk(div.TotalParts).ToList();
			List<string> wordsDivided = wordsDividedEnumerable[div.Part - 1].ToList();

			numWordsToCalc = wordsDivided.Count;

			List<IEnumerable<string>> wordsChunkedEnumerable = wordsDivided.Chunk(numThreads).ToList();
			List<List<string>> wordsChunked = new List<List<string>>();

			foreach(IEnumerable<string> chunk in wordsChunkedEnumerable) {
				wordsChunked.Add(chunk.ToList());
			}

			solutions = new List<Solution>();
			for(int i = 0; i < numThreads; ++i) {
				solutions.Add((Solution)Activator.CreateInstance(solution, this, wordsChunked[i], wordsNoCache, wordClues, hard));
			}
		}

		public void Poll(
			out int partsDone,
			out int partTotal,
			out List<(string Label, double Value)> chartElements,
			out string[] footnotes,
			out string currentResult,
			out Type _sol,
			out bool _hard
		) {
			_hard = hard;
			_sol = sol;
			partTotal = numWordsToCalc;
			partsDone = gradedWords.Count - numCachedWords;
			footnotes = new string[] { "footnote 1", "footnote 2" };
			currentResult = "wordl";
			chartElements = new List<(string, double)>();

			lock(usingGradedWords) {
				int i = 0;
				foreach (KeyValuePair<double, string> pair in gradedWords) {
					if (i >= topResultsToDisplay)
						break;
					chartElements.Add(  ("[white]" + (i + 1).ToString() + ". " + pair.Value + "[/]", pair.Key));
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
