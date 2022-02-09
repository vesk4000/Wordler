using System;
using System.Collections.Generic;
using System.Threading;

namespace Wordler
{
	class Program
	{
		static void Main(string[] args)
		{
			/*for(int i = 0; i < Solver.initWordList.Count; i += Solver.initWordList.Count / 8)
			{
				List<Thread> threads = new List<Thread>();
				Thread thread = new Thread(Solver.Solve());
				thread.Start();
			}
			;*/

			Solver.Solve();
		}
	}
}
