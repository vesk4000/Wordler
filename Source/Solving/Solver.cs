using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordler
{
	class Solver : ITaskable<string>
	{
		public void Poll(
			out int partsDone,
			out int partTotal,
			out List<Tuple<string, decimal>> chartElements,
			out string[] footnotes,
			out string currentResult
		)
		{
			partTotal = 100;
			partsDone = 10;
			footnotes = new string[] { "footnote 1", "footnote 2" };
			currentResult = "wordl";
			chartElements = new List<Tuple<string, decimal>>
			{
				new Tuple<string, decimal>("apple", 1),
				new Tuple<string, decimal>("crane", 200),
				new Tuple<string, decimal>("crook", 50)
			};
		}
	}
}
