using System;
using System.Diagnostics;
using System.IO;

namespace Wordler_Launcher
{
	class Program
	{
		static void Main(string[] args)
		{
			Process wordler = new Process();
			wordler.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\Wordler\\Wordler.exe";
			wordler.StartInfo.UseShellExecute = false;
			wordler.StartInfo.CreateNoWindow = true;
			wordler.StartInfo.Arguments = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Wordler\\launcher_args.txt");
			wordler.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory() + "\\Wordler";
			//Console.WriteLine($"{wordler.StartInfo.FileName} {wordler.StartInfo.Arguments}");
			wordler.Start();
			return;
		}
	}
}
