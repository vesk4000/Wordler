using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordler
{
	public class WordClues
	{
		private Dictionary<int, char> greens;
		private HashSet<(int Position, char Letter)> yellows;
		private HashSet<char> greys;
		private Dictionary<char, (int Amount, bool Definitive)> numOccurancesOfLetter;
		

		private void Init
		(
			Dictionary<int, char> greens,
			HashSet<(int, char)> yellows,
			HashSet<char> greys,
			Dictionary<char, (int Amount, bool Definitive)> numOccurancesOfLetter
		)
		{
			this.greens = new Dictionary<int, char>(greens);
			this.yellows = new HashSet<(int, char)>(yellows);
			this.greys = new HashSet<char>(greys);
			this.numOccurancesOfLetter =
				new Dictionary<char, (int Amount, bool Definitive)>(numOccurancesOfLetter);
		}


		private void Init()
		{
			greens = new Dictionary<int, char>();
			yellows = new HashSet<(int, char)>();
			greys = new HashSet<char>();
			numOccurancesOfLetter = new Dictionary<char, (int Amount, bool Definitive)>();
		}


		// TODO: Add error checking here
		private void Init(IList<string> words, IList<string> clues)
		{
			Init();

			WordClues theseClues = new WordClues();

			for(int i = 0; i < words.Count; ++i)
			{
				WordClues tempClues = new WordClues();
				string word = words[i];
				string currClues = clues[i];
				for(int j = 0; j < word.Length; ++j)
				{
					if(currClues[j] == 'g')
					{
						tempClues.greens.Add(j, word[j]);
						tempClues.IncreaseNumOccurancesOfLetter(word[j]);
					}
					else if(currClues[j] == 'y')
					{
						tempClues.yellows.Add((j, word[j]));
						tempClues.IncreaseNumOccurancesOfLetter(word[j]);
					}
				}
				for(int j = 0; j < word.Length; ++j)
				{
					if(currClues[j] == 'r')
					{
						if(tempClues.greens.ContainsValue(word[j]) || tempClues.yellows.Any(c => c.Letter == word[j]))
						{
							tempClues.yellows.Add((j, word[j]));
							tempClues.MakeNumOccuranceOfLetterDefinitive(word[j]);
						}
						else
							tempClues.greys.Add(word[j]);
					}
				}
				theseClues += tempClues;
			}

			greens = theseClues.greens;
			yellows = theseClues.yellows;
			greys = theseClues.greys;
			numOccurancesOfLetter = theseClues.numOccurancesOfLetter;
		}


		public WordClues()
		{
			Init();
		}


		public WordClues
		(
			Dictionary<int, char> greens,
			HashSet<(int, char)> yellows,
			HashSet<char> greys,
			Dictionary<char, (int Amount, bool Definitive)> numOccurancesOfLetter
		)
		{
			Init(greens, yellows, greys, numOccurancesOfLetter);
		}


		public WordClues(WordClues other)
		{
			Init(other.greens, other.yellows, other.greys, other.numOccurancesOfLetter);
		}


		public WordClues(IList<string> words, IList<string> clues)
		{
			Init(words, clues);
		}


		// TODO: Add ability to set numOccurancesOfLetter using this constructor and a new paramter
		public WordClues
		(
			List<string> greens,
			List<string> yellows,
			List<string> greys
		)
		{
			Init();

			Queue<char> letters;
			Queue<int> positions;

			if(greens is not null)
			{
				letters = new Queue<char>();
				positions = new Queue<int>();

				foreach(string green in greens)
				{
					if(green.All(c => Char.IsLetter(c)))
						letters.Enqueue(green[0]);
					else if(green.All(c => Char.IsNumber(c)))
						positions.Enqueue(int.Parse(green));
				}

				while(Math.Min(letters.Count, positions.Count) > 0)
				{
					IncreaseNumOccurancesOfLetter(letters.Peek());
					this.greens.Add(positions.Dequeue(), letters.Dequeue());
				}
			}

			if(yellows is not null)
			{
				letters = new Queue<char>();
				positions = new Queue<int>();

				foreach(string yellow in yellows)
				{
					if(yellow.All(c => Char.IsLetter(c)))
						letters.Enqueue(yellow[0]);
					else if(yellow.All(c => Char.IsNumber(c)))
						positions.Enqueue(int.Parse(yellow));
				}

				while(Math.Min(letters.Count, positions.Count) > 0)
				{
					if(!this.yellows.Contains((positions.Dequeue(), letters.Dequeue())))
						IncreaseNumOccurancesOfLetter(letters.Peek());
					this.yellows.Add((positions.Dequeue(), letters.Dequeue()));
				}
			}

			if(greys is not null)
				foreach(string grey in greys)
					this.greys.Add(grey[0]);
		}
		

		public WordClues(string guessWord, string computerWord) {
			Init();

			for (int i = 0; i < guessWord.Length; ++i)
			{
				if (guessWord[i] == computerWord[i])
				{
					greens.Add(i, guessWord[i]);
					IncreaseNumOccurancesOfLetter(guessWord[i]);
				}
			}

			for(int i = 0; i < guessWord.Length; ++i)
			{
				if(guessWord[i] != computerWord[i] && computerWord.Contains(guessWord[i]))
				{
					yellows.Add((i, guessWord[i]));
					if
					(
						greens.Where(c => c.Value == guessWord[i]).Count()
						+ yellows.Where(c => c.Letter == guessWord[i]).Count()
						<= computerWord.Where(c => c == guessWord[i]).Count()
					)
						IncreaseNumOccurancesOfLetter(guessWord[i]);
					else
						MakeNumOccuranceOfLetterDefinitive(guessWord[i]);
				}
			}

			for(int i = 0; i < guessWord.Length; ++i)
				if(!computerWord.Contains(guessWord[i]))
					greys.Add(guessWord[i]);
		}


		public WordClues(List<string> clues, int wordLength = 5)
		{
			List<string> wholeWords = new List<string>();
			List<string> wholeClues = new List<string>();

			string wholeString = "";

			bool isAddingLetters = true;
			int curr = 0;
			foreach(string clue in clues)
			{
				wholeString += clue;
				++curr;
				if(curr >= wordLength)
				{
					if(isAddingLetters)
						wholeWords.Add(wholeString);
					else
						wholeClues.Add(wholeString);
					isAddingLetters = !isAddingLetters;
					curr = 0;
					wholeString = "";
				}
			}

			Init(wholeWords, wholeClues);
		}


		// TODO: Fix this
		public bool IsValid(int wordLength = 5) {
			return true;
			/*foreach(char grey in Greys)
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

			return true;*/
		}


		public bool IsEmpty()
		{
			return greens.Count == 0 && yellows.Count == 0 && greys.Count == 0;
		}


		public static WordClues operator+(WordClues a, WordClues b)
		{
			WordClues newWordClues = new WordClues(b);
			foreach(var green in a.greens)
				if(!newWordClues.greens.Contains(green))
					newWordClues.greens.Add(green.Key, green.Value);

			foreach(var yellow in a.yellows)
				if(!newWordClues.yellows.Contains(yellow))
					newWordClues.yellows.Add(yellow);

			foreach(var grey in a.greys)
				if(!newWordClues.greys.Contains(grey))
					newWordClues.greys.Add(grey);

			newWordClues.numOccurancesOfLetter = new Dictionary<char, (int Amount, bool Definitive)>();
			foreach(var letter in a.numOccurancesOfLetter)
			{
				if(b.numOccurancesOfLetter.ContainsKey(letter.Key))
					newWordClues.numOccurancesOfLetter.Add
					(
						letter.Key,
						(
							Math.Max(letter.Value.Amount, b.numOccurancesOfLetter[letter.Key].Amount),
							letter.Value.Definitive || b.numOccurancesOfLetter[letter.Key].Definitive
						)
					);
				else
					newWordClues.numOccurancesOfLetter.Add(letter.Key, letter.Value);
			}
			foreach(var letter in b.numOccurancesOfLetter)
				if(!a.numOccurancesOfLetter.ContainsKey(letter.Key))
					newWordClues.numOccurancesOfLetter.Add(letter.Key, letter.Value);

			return newWordClues;
		}


		public bool Match(string word)
		{
			
			foreach(var green in greens)
				if(word[green.Key] != green.Value)
					return false;

			for(int i = 0; i < word.Length; ++i) {
				char letter = word[i];
				if(greys.Contains(letter))
					return false;
				if(yellows.Contains((i, letter)))
					return false;
			}

			HashSet<char> wordHashSet = word.ToHashSet();

			foreach(var yellow in yellows)
				if(!wordHashSet.Contains(yellow.Letter))
					return false;

			Dictionary<char, int> numOccurancesOfLetter = new Dictionary<char, int>();

			foreach(char letter in word)
			{
				numOccurancesOfLetter.EnsureKeyExists(letter, 0);
				++numOccurancesOfLetter[letter];
			}

			foreach(var letter in this.numOccurancesOfLetter)
				if
				(
					(!numOccurancesOfLetter.ContainsKey(letter.Key))
					||
					(
						letter.Value.Definitive
						&& letter.Value.Amount != numOccurancesOfLetter[letter.Key]
					)
					||
					(
						!letter.Value.Definitive
						&& letter.Value.Amount > numOccurancesOfLetter[letter.Key]
					)
				)
					return false;

			return true;
		}


		private void IncreaseNumOccurancesOfLetter(char letter)
		{
			numOccurancesOfLetter.EnsureKeyExists(letter, (0, false));
			numOccurancesOfLetter[letter] =
				(numOccurancesOfLetter[letter].Amount + 1,
				numOccurancesOfLetter[letter].Definitive);
		}


		private void MakeNumOccuranceOfLetterDefinitive(char letter)
		{
			numOccurancesOfLetter.EnsureKeyExists(letter, (0, false));
			numOccurancesOfLetter[letter] =
				(numOccurancesOfLetter[letter].Amount, true);
		}
	}
}
