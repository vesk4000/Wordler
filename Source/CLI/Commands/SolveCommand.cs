using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Wordler {
	class SolveCommand : Command<SolveCommand.Settings> {
		public class Settings : AppSettings {

		}

		public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings) {
			var task = new LiveTask<string, Solver>();
			task.Run(new Solver(
				settings.Hard,
				settings.WordList,
				settings.LeaderboardLength,
				settings.Threads,
				settings.wordClues,
				Extensions.GetSolutionType(settings.SolutionName),
				settings.Divide
			), settings.TimeLimit);

			settings.user.CreatePasteAsync
			(
				File.ReadAllText(Cacher.path),
				"Wordler Cache " + DateTime.Now.ToString(),
				PastebinAPI.Language.XML,
				PastebinAPI.Visibility.Private
			).Wait();

			return 0;
		}
	}
}
