using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordler
{
	class Solver : ITaskable<string>
	{
		// TODO: Implement ability to sort the list in multiple ways
		// Prolly gonna do that with a generic in the Solver
		// So something like Solver<TComparer> : ITaskable<string> where TComparer : IComparer
		private SortedList<double, string> gradedWords
				= new SortedList<double, string>(new DuplicateKeyComparer<double>());

		private int topResultsToDisplay;

		public Solver(int topResultsToDisplay = 10) {
			this.topResultsToDisplay = topResultsToDisplay;
			gradedWords.Add(1, "apple");
			gradedWords.Add(200, "crane");
			gradedWords.Add(50, "crook");

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

			int i = 0;
			foreach(KeyValuePair<double, string> pair in gradedWords)
			{
				if (i > topResultsToDisplay)
					break;
				chartElements.Add((pair.Value, pair.Key));
				++i;
			}


		}
	}
}
