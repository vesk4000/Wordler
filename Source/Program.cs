using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using System.Text.Json;
using System.IO;

namespace Wordler
{
    class Program
    {
        static void Main(string[] args)
        {
            //	/*for(int i = 0; i < Solver.initWordList.Count; i += Solver.initWordList.Count / 8)
            //	{
            //		List<Thread> threads = new List<Thread>();
            //		Thread thread = new Thread(Solver.Solve());
            //		thread.Start();
            //	}
            //	;*/

            //	//Solver.Solve();


            //	var table = new Table().Centered();

            //	AnsiConsole.Live(table)
            //		.Start(ctx => {
            //			table.AddColumn("Foo");
            //			ctx.Refresh();
            //			Thread.Sleep(1000);
            //			Thread.Sleep(50);

            //			table.AddRow(new BarChart()
            //	.Width(60)
            //	.Label("[green bold underline]Number of fruits[/]")
            //	.CenterLabel()
            //	.AddItem("Apple", 12, Color.Yellow)
            //	.AddItem("Orange", 54, Color.Green)
            //	.AddItem("Banana", 33, Color.Red));
            //			ctx.Refresh();
            //			while (true) {
            //				Thread.Sleep(50);
            //				ctx.Refresh();
            //			}


            //		/*table.AddColumn("Bar");
            //		ctx.Refresh();*/

            //		});


            //	var bar = new BarChart()
            //.Width(60)
            //.Label("[green bold underline]Number of fruits[/]")
            //.CenterLabel()
            //.AddItem("Apple", 12, Color.Yellow)
            //.AddItem("Orange", 54, Color.Green)
            //.AddItem("Banana", 33, Color.Red);

            //	AnsiConsole.Live(bar).Start(ctx => { });


            //	/*AnsiConsole.Live(table)
            //	.Start(ctx => {
            //		table.AddColumn("Foo");
            //		ctx.Refresh();
            //		Thread.Sleep(1000);

            //		table.AddColumn("Bar");
            //		ctx.Refresh();
            //		Thread.Sleep(1000);
            //	});*/
            Solver.EtoSolve();
        }
    }
}
