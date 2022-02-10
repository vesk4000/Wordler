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
            CalculateProbabilities();
            Solver.EtoSolve();
        }

        private static void CalculateProbabilities()
        {
            int letterListCount = Solver.wordList.Count * 5;
            var wordlist = Solver.wordList;
            Dictionary<char, double> letterProbability = new Dictionary<char, double>();
            for (char i = 'a'; i <= 'z'; ++i)
            {
                int occurances = 0;
                foreach (var word in wordlist)
                {
                    occurances += word.Count(e => e == i);
                }
                letterProbability.Add(i, (double)occurances / (double)letterListCount);
            }
            letterProbability = letterProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            Dictionary<string, double> wordProbability = new Dictionary<string, double>();
            foreach (var word in wordlist)
            {
                double probability = 0;
                foreach (var letter in word)
                {
                    probability += (letterProbability[letter] / Math.Log2(letterProbability[letter]));
                }
                wordProbability.Add(word, -probability);
            }
            wordProbability = wordProbability.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);
            File.WriteAllText(@"..\..\..\wordProbability.txt", JsonSerializer.Serialize(wordProbability));
        }
    }
}
