using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordler
{
    class BenchmarkCommand : Command<BenchmarkCommand.Settings>
	{
		public class Settings : AppSettings
		{

		}

		public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
		{
			var task = new LiveTask<string, Benchmark>();
			task.Run(new Benchmark(
				settings.Hard,
				settings.WordList,
				settings.LeaderboardLength,
				settings.Threads,
				settings.wordClues,
				Extensions.GetSolutionType(settings.SolutionName),
				settings.Divide
			), settings.TimeLimit);

			return 0;
		}
    }
}
