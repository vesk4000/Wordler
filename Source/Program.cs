using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using System.Text.Json;
using System.IO;
using Spectre.Console.Cli;
using System.Runtime.InteropServices;

namespace Wordler
{
	class Program
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleOutputCP(uint wCodePageID);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleCP(uint wCodePageID);

		static int Main(string[] args)
		{
			#if DEBUG
			Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
			#endif

			/*Console.InputEncoding = System.Text.Encoding.UTF8;
			Console.OutputEncoding = System.Text.Encoding.UTF8;*/
			SetConsoleOutputCP(65001);
			SetConsoleCP(65001);

			var app = new CommandApp<SolveCommand>();

			app.Configure(config => {
				config.AddCommand<SolveCommand>("solve");

				config.SetExceptionHandler(ex => {
					if(ex is not CommandRuntimeException) {
						AnsiConsole.MarkupLine("[red]Fatal Error:[/] Something went wrong. Please report the issue to https://github.com/vesk4000/Wordler/issues");
						AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
						return -99;
					}
					
					// Command parsing exception
					AnsiConsole.MarkupLine("[red]Error:[/] " + ex.Message);
					return -99;
				});
			});
			
			return app.Run(args);
		}
	}
}
