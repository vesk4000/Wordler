using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordler
{
	interface ITaskable<T>
	{
		public void Poll(
			out int partsDone,
			out int partTotal,
			out List<(string Label, double Value)> chartElements,
			out string[] footnotes,
			out T currentResult
		);
	}
}
