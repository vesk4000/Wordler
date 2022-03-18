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
			wordler.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\Wordler.exe";
			wordler.StartInfo.UseShellExecute = false;
			wordler.StartInfo.CreateNoWindow = true;
			wordler.StartInfo.Arguments = "--tl 15 --ps \" Wait-Process -Id @ID; $directorypath =  (Get-Item .).FullName; $path = $directorypath + '\\Wordlerer\\';If (!(test-path $path)) { mkdir $path }; Remove-Item $directorypath; \"";
			wordler.Start();
			return;
		}
	}
}
