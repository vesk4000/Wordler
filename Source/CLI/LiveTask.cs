using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using System.Diagnostics;
using System.Threading;

namespace Wordler
{
	class LiveTask<TResult, TTask> where TTask : ITaskable<TResult>
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

			var pos = Console.GetCursorPosition();

			while(true) {
				AnsiConsole.Cursor.Hide();
				int partsTotal, partsDone;
				string[] footnotes;
				TResult currentResult;
				List<(string Label, double Value)> chartElements;

				taskedObject.Poll(out partsTotal, out partsDone, out chartElements, out footnotes, out currentResult);

				statusTable.Rows.Update(0, 0, new Markup("Uptime: " + upTimer.Elapsed));

				leaderboardChart = new BarChart();

				leaderboardChart.AddItems(
					chartElements,
					(element) => new BarChartItem(element.Label, element.Value)
				);


				if(chartElements.Count > 0)
					leaderboardTable.UpdateCell(0, 0, leaderboardChart);
				else
					leaderboardTable.UpdateCell(0, 0, new Markup(""));
				masterTable.Border(TableBorder.None);
				
				AnsiConsole.Write(masterTable);
				AnsiConsole.Cursor.SetPosition(pos.Left, pos.Top + 1);

				// TODO: Fix fps timings to take into account time spent doing the operations
				Thread.Sleep(1000 / fps);
			}

			AnsiConsole.Cursor.Show();
		}
	}
}
