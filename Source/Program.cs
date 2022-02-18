using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using System.Text.Json;
using System.IO;
using Spectre.Console.Cli;

namespace wordler
{
	class Program
	{
		static int Main(string[] args)
		{
			WordClues wordClues = new WordClues(new List<string>("a a h e d g g g g g".Split(" ")), 5);
			wordClues.Yellows.Add((4, 'h'));
			wordClues.Yellows.Add((4, 'e'));
			wordClues.Yellows.Add((2, 'a'));
			wordClues.Yellows.Add((4, 'a'));
			wordClues.Yellows.Add((2, 'e'));
			wordClues.Greys = "l i f r g s t b c z o u k m p n v y".Split(" ").Select(e => char.Parse(e)).ToHashSet<char>();

			Console.WriteLine(wordClues.Match("aahed"));

			#if DEBUG
			Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
			#endif

			var app = new CommandApp<SolveCommand>();

			app.Configure(config => {
				config.AddCommand<SolveCommand>("solve");
			});
			
			return app.Run(args);


		}
	}
}
