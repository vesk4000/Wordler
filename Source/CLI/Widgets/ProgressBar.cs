using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Wordler {
	static class ProgressBar {
		private static int spriteNumber = 0;

		public static string RenderMarkup(int width, string name, int partsDone, int partsTotal, string colorBase, string colorDone) {
			char part = '━';
			
			string ans = "";
			int length = 0;

			ans += $"[white]{name}[/]";
			length += name.Length + 1;

			string progress = Convert.ToString((int)Math.Floor((double)partsDone / partsTotal * 100)) + "%";
			progress = new string(' ', (4 - progress.Length)) + progress;
			length += progress.Length + 1;

			int doneLength, leftLength;
			doneLength = (int)Math.Floor(((double)partsDone / partsTotal) * (width - length));
			leftLength = width - length - doneLength;

			ans += $"[{colorDone}]" + new string(part, doneLength) + "[/]" + new string(part, leftLength);

			ans += " " + (partsDone == partsTotal ? $"[{colorDone}]{progress}[/]" : $"{progress}");

			char[] sprites = new char[] { '⠟', '⠻', '⠽', '⠾', '⠷', '⠯' };
			if(partsDone != partsTotal)
				ans += $" {sprites[spriteNumber]}";
			else ans += $" [{colorDone}]✓[/]";

			spriteNumber++;
			if(spriteNumber >= sprites.Length)
				spriteNumber = 0;

			return ans;
		}
	}
}
