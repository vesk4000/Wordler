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

		public WordClues wordClues;

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
			if(Clues.Any(c => !Char.IsLetter(c)))
				return ValidationResult.Error("Failed parsing word clues");

			wordClues += new WordClues(clueTokens);

			if(!wordClues.IsValid())
				return ValidationResult.Error("Incompatible word clues");

			this.wordClues = wordClues;

			return ValidationResult.Success();
		}
	}
}
