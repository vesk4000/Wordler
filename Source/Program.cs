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
