using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using System.Text.Json;
using System.IO;

namespace Wordler
{
	class Program
	{
		static void Main(string[] args)
		{
			var wordlist = Solver.wordList;


			
			
			Solver.IcySolve();


			//Solver.GiveClues();
		}
	}
}
