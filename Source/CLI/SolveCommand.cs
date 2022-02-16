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
			var task = new LiveTask<string, string>();
			task.Run(new Solver());

			return 0;
		}
	}
}
