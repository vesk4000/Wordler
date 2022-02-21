using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordler{
	public class WordClues
	{
		public Dictionary<int, char> Greens = new Dictionary<int, char>();
		public HashSet<(int Position, char Letter)> Yellows = new HashSet<(int, char)>();
		public HashSet<char> Greys = new HashSet<char>();

		public WordClues() { }

		public WordClues(Dictionary<int, char> inputGreens,
						 HashSet<(int, char)> inputYellows,
						 HashSet<char> inputGreys)
		{
			Greens = new Dictionary<int, char>(inputGreens);
			Yellows = new HashSet<(int, char)>(inputYellows);
			Greys = new HashSet<char>(inputGreys);
		}

		public WordClues(List<string> greens,
			List<string> yellows,
			List<string> greys) {

			Dictionary<int, char> Greens = new Dictionary<int, char>();
			HashSet<(int, char)> Yellows = new HashSet<(int, char)>();
			HashSet<char> Greys = new HashSet<char>();

			Queue<char> letters;
			Queue<int> positions;

			if(greens is not null) {
				letters = new Queue<char>();
				positions = new Queue<int>();

				foreach(string green in greens) {
					if(green.All(c => Char.IsLetter(c)))
						letters.Enqueue(green[0]);
					else if(green.All(c => Char.IsNumber(c)))
						positions.Enqueue(int.Parse(green));
				}

				while(Math.Min(letters.Count, positions.Count) > 0) {
					Greens.Add(positions.Dequeue(), letters.Dequeue());
				}

				this.Greys = Greys;
				this.Greens = Greens;
				this.Yellows = Yellows;
			}

			if(yellows is not null) {
				letters = new Queue<char>();
				positions = new Queue<int>();

				foreach(string yellow in yellows) {
					if(yellow.All(c => Char.IsLetter(c)))
						letters.Enqueue(yellow[0]);
					else if(yellow.All(c => Char.IsNumber(c)))
						positions.Enqueue(int.Parse(yellow));
				}

				while(Math.Min(letters.Count, positions.Count) > 0)
					Yellows.Add((positions.Dequeue(), letters.Dequeue()));
			}

			if(greys is not null) {
				foreach(string grey in greys)
					Greys.Add(grey[0]);
			}
		}
		
		public WordClues(string guessWord, string computerWord) {
			this.Greens = new Dictionary<int, char>();
			this.Yellows = new HashSet<(int, char)>();
			this.Greys = new HashSet<char>();

			for (int i = 0; i < guessWord.Length; ++i) {
				if (guessWord[i] == computerWord[i]) {
					Greens.Add(i, guessWord[i]);
				}
				else if (computerWord.Contains(guessWord[i])) {
					Yellows.Add((i, guessWord[i]));
				}
				else {
					Greys.Add(guessWord[i]);
				}
			}
		}

		public bool IsValid(int wordLength = 5) {
			foreach(char grey in Greys)
				if(Greens.ContainsValue(grey) || Yellows.Any(c => c.Letter == grey))
					return false;

			Dictionary<char, int> yellowChecker = new Dictionary<char, int>();

			foreach(var yellow in Yellows) {
				if(Greens.ContainsKey(yellow.Position) && Greens[yellow.Position] == yellow.Letter)
					return false;
				if(!yellowChecker.ContainsKey(yellow.Letter))
					yellowChecker[yellow.Letter] = 1;
				else
					++yellowChecker[yellow.Letter];
			}

			foreach(var yellowCheck in yellowChecker)
				if(yellowCheck.Value >= wordLength)
					return false;

			return true;
		}

		public WordClues(List<string> clues, int wordLength = 5) {
			this.Greens = new Dictionary<int, char>();
			this.Yellows = new HashSet<(int, char)>();
			this.Greys = new HashSet<char>();

			Queue<char> letters = new Queue<char>();

			bool isAddingLetters = true;
			int curr = 0;
			foreach(var clue in clues) {
				++curr;

				if(curr > wordLength) {
					isAddingLetters = !isAddingLetters;
					curr = 1;
				}

				if(isAddingLetters)
					letters.Enqueue(clue[0]);
				else {
					if(clue[0] == 'g') {
						if(!Greens.ContainsKey(curr - 1))
							Greens.Add(curr - 1, letters.Peek());
					}
					else if(clue[0] == 'y') {
						if(!Yellows.Contains((curr - 1, letters.Peek())))
							Yellows.Add((curr - 1, letters.Peek()));
					}
					else if(clue[0] == 'r') {
						if(!Greys.Contains(letters.Peek()))
							Greys.Add(letters.Peek());
					}
					letters.Dequeue();
				}
			}
		}

		public WordClues(WordClues wordClues)
		{
			this.Greens = new Dictionary<int, char>(wordClues.Greens);
			this.Yellows = new HashSet<(int Position, char Letter)>(wordClues.Yellows);
			this.Greys = new HashSet<char>(wordClues.Greys);
		}

		public bool IsEmpty() {
			return Greens.Count == 0 && Yellows.Count == 0 && Greys.Count == 0;
		}

		public static WordClues operator+(WordClues a, WordClues b) {
			WordClues b1 = new WordClues(b);
			b1.cached = false;
			foreach(var green in a.Greens)
				if(!b1.Greens.Contains(green))
					b1.Greens.Add(green.Key, green.Value);

			foreach(var yellow in a.Yellows)
				if(!b1.Yellows.Contains(yellow))
					b1.Yellows.Add(yellow);

			foreach(var grey in a.Greys)
				if(!b1.Greys.Contains(grey))
					b1.Greys.Add(grey);

			return b1;
		}

		private bool cached = false;
		private bool[] greys;

		public bool Match(string word)
		{
			if(IsEmpty())
				return true;

			if(!cached) {
				cached = true;
				greys = new bool[26];
				foreach(char c in Greys)
					greys[c - 'a'] = true;
			}

			foreach(var green in Greens)
				if(word[green.Key] != green.Value)
					return false;

			for(int i = 0; i < word.Length; ++i) {
				char letter = word[i];
				if(greys[letter - 'a'])
					return false;
				/*if(Greys.Contains(letter))
					return false;*/
				if(Yellows.Contains((i, letter)))
					return false;
			}

			HashSet<char> wordHashSet = word.ToHashSet();

			foreach(var yellow in Yellows)
				if(!wordHashSet.Contains(yellow.Letter))
					return false;

			return true;
		}


		public void Add(Dictionary<int, char> greens, HashSet<(int, char)> yellows, HashSet<char> greys)
		{
			cached = false;
			foreach (var green in greens)
			{
				if (!Greens.ContainsKey(green.Key)) Greens.Add(green.Key, green.Value);
			}
			foreach (var yellow in yellows)
			{
				if (!Yellows.Any(e => e.Item1 == yellow.Item1 && e.Item2 == yellow.Item2)) Yellows.Add(yellow);
			}
			foreach (var grey in greys)
			{
				if (!Greys.Any(e => e == grey)) Greys.Add(grey);
			}
		}
	}
}
