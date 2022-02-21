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
			Color accent = new Color(155, 215, 155); //0,215,175 | 0,255,135 | 95,215,215

			var masterTable = new Table();
			masterTable.Width = 100;
			masterTable
				.AddColumn("Master Table")
				.HideHeaders();

			var statusTable = new Table();
			var statusPanel = new Panel(statusTable);
			masterTable.AddRow(statusPanel);

			statusPanel.Header = new PanelHeader("[white] Status [/]");
			statusPanel
				.RoundedBorder()
				.BorderColor(accent);

			statusTable
				.AddColumn("Status")
				.HideHeaders()
				.NoBorder()
				.Width(98);

			var statsTable = new Table();
			statusTable
				.AddRow(statsTable)
				.AddRow(ProgressBar.RenderMarkup(92, "Progress", 75, 100, "white", "seagreen3"));

			Stopwatch upTimer = new Stopwatch();
			upTimer.Start();
			var upTimeMarkup = new Markup(upTimer.Elapsed.ToString());


			statsTable
				.AddColumn("Time Label")
				.AddColumn("Time Stats", c => c.Width = 20)
				.AddColumn("Part Label")
				.AddColumn("Part Stats", c => c.Width = 20)
				.AddColumn("Task Label")
				.AddColumn("Task Stats")
				.HideHeaders()
				.NoBorder();
			statsTable
				.AddRow("[white]Uptime [/]", "Uptime", "[white]Done [/]", "Done", "[white]Solution [/]", "Solution")
				.AddRow("[white]ETA [/]", "ETA", "[white]Total [/]", "Total", "[white]Hard Mode [/]", "HardMode");

			BarChart leaderboardChart = new BarChart();
			var leaderboardPanel = new Panel(leaderboardChart);
			leaderboardPanel.Header = new PanelHeader("[white] Results [/]");
			masterTable.AddRow(leaderboardPanel);

			var pos = Console.GetCursorPosition();

			while(true) {
				AnsiConsole.Cursor.Hide();
				AnsiConsole.Cursor.SetPosition(pos.Left, pos.Top + 1);


				int partsTotal, partsDone;
				string[] footnotes;
				TResult currentResult;
				List<(string Label, double Value)> chartElements;
				bool hard;
				Type sol;

				taskedObject.Poll(out partsDone, out partsTotal, out chartElements, out footnotes, out currentResult, out sol, out hard);

				statsTable.UpdateCell(0, 1, new Markup(upTimer.Elapsed.ToString(@"hh\:mm\:ss")));
				statsTable.UpdateCell(0, 3, new Markup(partsDone.ToString()));
				statsTable.UpdateCell(0, 5, new Markup(sol.Name));
				if(partsDone == 0)
					statsTable.UpdateCell(1, 1, "N/A");
				else if(partsDone == partsTotal)
					statsTable.UpdateCell(1, 1, "Finished");
				else
					statsTable.UpdateCell(1, 1, new Markup(new TimeSpan(0, 0, 0, 0, (int)Math.Floor(upTimer.Elapsed.TotalMilliseconds / partsDone * (partsTotal - partsDone))).ToString(@"hh\:mm\:ss")));
				statsTable.UpdateCell(1, 3, new Markup(partsTotal.ToString()));
				statsTable.UpdateCell(1, 5, new Markup(hard ? "On" : "Off"));

				statusTable.UpdateCell(1, 0, new Markup(ProgressBar.RenderMarkup(90, "Progress ", partsDone, partsTotal, "white", "seagreen3")));

				leaderboardChart = new BarChart();



				leaderboardChart.AddItems(
					chartElements,
					(element) => new BarChartItem(element.Label, element.Value, getAccent())
				);

				var leaderboardPanelWithContents = new Panel(leaderboardChart);
				var leaderboardPanelWithoutContents = new Panel("");

				if(chartElements.Count > 0)
					leaderboardPanel = leaderboardPanelWithContents;
				else
					leaderboardPanel = leaderboardPanelWithoutContents;

				leaderboardPanel.Header = new PanelHeader("[white] Results [/]");
				leaderboardPanel
					.RoundedBorder()
					.BorderColor(accent);

				masterTable.UpdateCell(1, 0, leaderboardPanel);

				masterTable.Border(TableBorder.None);

				masterTable.UpdateCell(0, 0, statusPanel);
					
				if (leaderboardPanel == leaderboardPanelWithoutContents)
					masterTable.UpdateCell(1, 0, new Markup(""));
				if (upTimer.ElapsedMilliseconds < 300)
					masterTable.UpdateCell(0, 0, new Markup(""));

				AnsiConsole.Write(masterTable);

				if(partsTotal == partsDone)
					break;

				Thread.Sleep(1000 / fps);
			}

			AnsiConsole.Cursor.Show();
		}

		private bool num = true;
		private Color[] accents = new Color[] { new Color(155, 215, 155), new Color(215, 215, 215) };

		private Random rng = new Random();

		private Color RngColor()
		{
			return ColorFromHSV(rng.Next(0, 360), 0.5, 0.8);
		}

		private Color getAccent()
		{
			num = !num;
			return accents[num ? 1 : 0];
		}

		public static Color ColorFromHSV(double hue, double saturation, double value)
		{
			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			double f = hue / 60 - Math.Floor(hue / 60);

			value = value * 255;
			int v = Convert.ToInt32(value);
			int p = Convert.ToInt32(value * (1 - saturation));
			int q = Convert.ToInt32(value * (1 - f * saturation));
			int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

			if (hi == 0)
				return new Color((byte)v, (byte)t, (byte)p);
			else if (hi == 1)
				return new Color((byte)q, (byte)v, (byte)p);
			else if (hi == 2)
				return new Color((byte)p, (byte)v, (byte)t);
			else if (hi == 3)
				return new Color((byte)p, (byte)q, (byte)v);
			else if (hi == 4)
				return new Color((byte)t, (byte)p, (byte)v);
			else
				return new Color((byte)v, (byte)p, (byte)q);
		}
	}
}
