using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using System.IO;

namespace Wordler {
	class AppSettings : CommandSettings {

		[Description("Only words that match the current clues can be guessed")]
		[CommandOption("-h|--hard")]
		[DefaultValue(false)]
		public bool Hard { get; set; }

		[Description("List of guessable words")]
		[CommandOption("-w|--wordlist|--word-list")]
		[DefaultValue("wordlist.txt")]
		public string WordList { get; set; }

		[Description("The maximum amount of items which will be shown on the leaderboard")]
		[CommandOption("-l|--leaderboard|--leaderboard-length|--leaderboardlength")]
		[DefaultValue(10)]
		public int LeaderboardLength { get; set; }

		[Description("The amount of simultenuous threads which will be used for solving")]
		[CommandOption("-t|--threads")]
		[DefaultValue(1)]
		public int Threads { get; set; }

		[Description("Any green clues that we have gotten so far")]
		[CommandOption("-g|--greens|--green")]
		[DefaultValue("")]
		public string Greens { get; set; }

		[Description("Any yellow clues that we have gotten so far")]
		[CommandOption("-y|--yellows|--yellow")]
		[DefaultValue("")]
		public string Yellows { get; set; }

		[Description("Any grey clues that we have gotten so far")]
		[CommandOption("-r|--greys|--grey")]
		[DefaultValue("")]
		public string Greys { get; set; }

		[Description("Any whole word clues that we have gotten so far")]
		[CommandOption("-c|--clues|--clue")]
		[DefaultValue("")]
		public string Clues { get; set; }

		[Description("The solution (strategy) that is used to determine the best words to use")]
		[CommandOption("-s|--solution|--strategy")]
		[DefaultValue("brute-force")]
		public string SolutionName { get; set; }

		[Description("The part of all possible words that should be considered for best word")]
		[CommandOption("-d|--divide")]
		[DefaultValue("1/1")]
		public string Divide { get; set; }

		[Description("Uploads your cache to Pastebin. You must specify your Pastebin credentials, seperated by spaces: Developer API Key (can be found at https://pastebin.com/doc_api#1), Username and Password.")]
		[CommandOption("-p|--pastebin")]
		[DefaultValue("")]
		public string Pastebin { get; set; }

		[Description("Time (in seconds) after which to conclude the grading of words")]
		[CommandOption("--tl|--timelimit|--time-limit")]
		[DefaultValue(3_600_000)]
		public int TimeLimit { get; set; }

		public WordClues wordClues;
		public PastebinAPI.User user;

		public override ValidationResult Validate() {

			if(!File.Exists(WordList) && !File.Exists(Directory.GetCurrentDirectory() + "/" + WordList))
				return ValidationResult.Error("Specified word list doesn't exist");

			List<string> words = File.ReadAllLines(WordList).ToList();
			int length = -1;
			foreach(var word in words) {
				if(word != "")
					length = word.Length;
				if(word.Length != length) {
					length = -1;
					break;
				}
			}
			if(length == -1)
				return ValidationResult.Error("Invalid word list (all words must be the same length and there must be at least 1 word");

			if(LeaderboardLength <= 0)
				return ValidationResult.Error("Leaderboard length must be at least 1");

			if(LeaderboardLength <= 0)
				return ValidationResult.Error("There must be at least 1 thread used for solving");

			List<string> greenTokens = Greens.Tokenize();
			if(greenTokens == null)
				return ValidationResult.Error("Failed parsing green clues");

			List<string> yellowTokens = Yellows.Tokenize();
			if(yellowTokens == null)
				return ValidationResult.Error("Failed parsing yellow clues");

			if(Greys.Any(c => !Char.IsLetter(c)))
				return ValidationResult.Error("Failed parsing grey clues");
			List<string> greyTokens = Greys.Tokenize();

			WordClues wordClues = new WordClues(greenTokens, yellowTokens, greyTokens);

			List<string> clueTokens = Clues.Tokenize();
			if(clueTokens.Any(c => !Char.IsLetter(c[0])))
				return ValidationResult.Error("Failed parsing word clues");

			wordClues += new WordClues(clueTokens, length);

			if(!wordClues.IsValid(length))
				return ValidationResult.Error("Incompatible word clues");

			this.wordClues = wordClues;

			if(Extensions.GetSolutionType(SolutionName) == null)
				return ValidationResult.Error("No such solution/strategy exists");

			(int Part, int TotalParts)? div = Divide.GetDivision();
			if(div is null)
				return ValidationResult.Error("Invalid divide of words");

			if(Pastebin != "")
			{
				string[] pastebinCredentials = Pastebin.Split(' ');
				if (pastebinCredentials.Count() != 3)
					return ValidationResult.Error("Invalid Pastebin credentials");

				string APIKey = pastebinCredentials[0];
				string username = pastebinCredentials[1];
				string password = pastebinCredentials[2];
				try
				{
					PastebinAPI.Pastebin.DevKey = APIKey;
					user = PastebinAPI.Pastebin.LoginAsync(username, password).GetAwaiter().GetResult();

				}
				catch(PastebinAPI.PastebinException)
				{
					return ValidationResult.Error("Invalid Pastebin credentials");
				}
			}

			if (TimeLimit < 0)
				return ValidationResult.Error("Invalid time limit");

			return ValidationResult.Success();
		}
	}
}
