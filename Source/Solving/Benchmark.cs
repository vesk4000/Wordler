using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wordler
{
    internal class Benchmark : ITaskable<string>
    {
		public SortedList<double, string> gradedWords
			= new SortedList<double, string>(new DuplicateKeyComparerAscending<double>());

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

		public Benchmark(
			bool hard,
			string wordListPath,
			int topResultsToDisplay,
			int numThreads,
			WordClues wordClues,
			Type solution,
			string divisionsString) 
        {
			this.hard = hard;
			this.wordListPath = wordListPath;
			this.topResultsToDisplay = topResultsToDisplay;
			this.numThreads = numThreads;
			this.wordClues = wordClues;
			this.sol = solution;

			HashSet<string> words = File.ReadAllLines(wordListPath).ToHashSet();

			int guesses = 0;
			int failed = 0;

			var solve = new LiveTask<string, Solver>();
			solve.Run(new Solver(
				true,
				hard,
				wordListPath,
				topResultsToDisplay,
				numThreads,
				wordClues,
				solution,
				divisionsString
			));

			foreach (string word in words)
            {

            }
		}

        public void Poll(
			out int partsDone,
			out int partTotal,
			out List<(string Label, double Value)> chartElements,
			out string[] footnotes,
			out string currentResult,
			out Type _sol,
			out bool _hard)
        {
            _hard = hard;
            _sol = sol;
            partTotal = numWordsToCalc;
            partsDone = gradedWords.Count - numCachedWords;
            footnotes = new string[] { "footnote 1", "footnote 2" };
            currentResult = "wordl";
            chartElements = new List<(string, double)>();

            lock (usingGradedWords)
            {
                int i = 0;
                foreach (KeyValuePair<double, string> pair in gradedWords)
                {
                    if (i >= topResultsToDisplay)
                        break;
                    chartElements.Add(("[white]" + (i + 1).ToString() + ". " + pair.Value + "[/]", pair.Key));
                    ++i;
                }
            }
        }

        //public static List<string> wordList = File.ReadAllText(@"..\..\..\shortwordlist.txt").Split(";").ToList();
		//public static Dictionary<string, double> wordProbability;
		//public static List<string> wordProbabilityList = new List<string>();

		//public static void IcySolve()
		//{
		//	int guesses = 0;
		//	int failed = 0;

		//	ProbabilityCalculatorHDD.CalculateProbabilities(wordList, false);

		//	foreach (string goal in wordList)
		//	{
		//		wordProbabilityList.Clear();
		//		wordProbabilityList.AddRange(wordProbability.Keys.ToList());

		//		WordClues wordClues = new WordClues();
		//		int guessCount = 0;
		//		string guess = string.Empty;

		//		while (guess != goal)
		//		{
		//			guess = wordProbabilityList.First();
		//			wordClues = GenerateClues(wordClues, guess, goal);

		//			guessCount++;

		//			wordProbabilityList.RemoveAll(e => !wordClues.Match(e));

		//			ProbabilityCalculatorHDD.CalculateProbabilities(wordProbabilityList, true);
		//		}

		//		guesses += guessCount;

		//		if (guessCount > 6) { failed++; Console.WriteLine(goal); Console.WriteLine($"{goal}: {guessCount}"); }
		//	}
		//	Console.WriteLine($"Average: {(double)((double)guesses / (double)wordList.Count)}");
		//	Console.WriteLine($"Failed: {failed} -> {((double)((double)failed / (double)wordList.Count) * 100):F2}%");
		//}
	}
}
