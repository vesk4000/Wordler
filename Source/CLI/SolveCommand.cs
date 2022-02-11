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

namespace wordler
{
	class SolveCommand : Command<SolveCommand.Settings>
	{
		public class Settings : CommandSettings
		{

		}

		public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
		{
			AnsiConsole.WriteLine("Hello there!");

			int fps = 10;

			var masterTable = new Table();
			masterTable.AddColumn("Master Table");

			var statusTable = new Table();
			statusTable.AddColumn("Status");

			Stopwatch upTimer = new Stopwatch();
			upTimer.Start();
			statusTable.AddRow("Uptime: " + upTimer.Elapsed);
			masterTable.AddRow(statusTable);

			var leaderboardTable = new Table();
			leaderboardTable.AddColumn("Best words so far");
			masterTable.AddRow(leaderboardTable);
			


			AnsiConsole.Live(masterTable).Start(ctx => {
				while(true) {
					statusTable.Rows.Update(0,0, "Uptime: " + upTimer.Elapsed);
					

					ctx.Refresh();

					// TODO: Fix fps timings to take into account time spent doing the operations
					Thread.Sleep(1000 / fps);
				}
			});

			return 0;
		}
	}
}
