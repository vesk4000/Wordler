using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordler
{
	class Solver : ITaskable<string>
	{
<<<<<<< HEAD
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
=======
		public void Poll(
			out int partsDone,
			out int partTotal,
			out List<Tuple<string, decimal>> chartElements,
			out string[] footnotes,
			out string currentResult
		)
		{
>>>>>>> 7283a3361ac8ab0eaacdf49ae1b4ae63989b8f67
			partTotal = 100;
			partsDone = 10;
			footnotes = new string[] { "footnote 1", "footnote 2" };
			currentResult = "wordl";
<<<<<<< HEAD

			chartElements = new List<(string, double)>();

			int i = 0;
			foreach(KeyValuePair<double, string> pair in gradedWords)
			{
				if (i > topResultsToDisplay)
					break;
				chartElements.Add((pair.Value, pair.Key));
				++i;
			}


=======
			chartElements = new List<Tuple<string, decimal>>
			{
				new Tuple<string, decimal>("apple", 1),
				new Tuple<string, decimal>("crane", 200),
				new Tuple<string, decimal>("crook", 50)
			};
>>>>>>> 7283a3361ac8ab0eaacdf49ae1b4ae63989b8f67
		}
	}
}
