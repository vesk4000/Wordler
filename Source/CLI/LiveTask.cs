using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using System.Diagnostics;
using System.Threading;

namespace wordler
{
	class LiveTask<TResult, TTask>// where TTask : ITaskable<TResult>
	{
		public void Run(ITaskable<TResult> taskedObject, int fps = 10)
		{
			AnsiConsole.WriteLine("Hello here!");

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
			BarChart leaderboardChart = new BarChart();
			leaderboardTable.AddRow(leaderboardChart);
			masterTable.AddRow(leaderboardTable);



			AnsiConsole.Live(masterTable).Start(ctx => {
				while (true)
				{
					statusTable.Rows.Update(0, 0, new Markup("Uptime: " + upTimer.Elapsed));

					leaderboardChart = new BarChart();
					var chartElements = new List<(string Label, double Value)>
					{
						("apple", 1),
						("crane", 200),
						("crook", 50)
					};
					leaderboardChart.AddItems(
						chartElements,
						(element) => new BarChartItem(element.Label, element.Value)
					);

					leaderboardTable.UpdateCell(0, 0, leaderboardChart);

			ctx.Refresh();

					// TODO: Fix fps timings to take into account time spent doing the operations
					Thread.Sleep(1000 / fps);
				}
			});

			
		}
	}
}
