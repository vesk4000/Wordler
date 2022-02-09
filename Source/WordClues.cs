using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wordler
{
	public class WordClues
	{
		public Dictionary<int, char> Greens;
		public Dictionary<int, char> Yellows;
		public List<char> Greys;

		public WordClues(Dictionary<int, char> inputGreens,
						 Dictionary<int, char> inputYellows,
						 List<char> inputGreys)
		{
			this.Greens = new Dictionary<int, char>(inputGreens);
			this.Yellows = new Dictionary<int, char>(inputYellows);
			this.Greys = new List<char>(inputGreys);
		}

		public WordClues() {
			this.Greens = new Dictionary<int, char>();
			this.Yellows = new Dictionary<int, char>();
			this.Greys = new List<char>();
		}

		public bool Match(string word)
		{
			foreach (char letter in word)
			{
				if (Greys.Contains(letter)) return false;

				if (Yellows.ContainsKey(word.IndexOf(letter)) && 
					Yellows[word.IndexOf(letter)] == letter) return false;

				if (Greens.ContainsKey(word.IndexOf(letter)) &&
					Greens[word.IndexOf(letter)] != letter) return false;
			}

			return true;
		}

		public string NextWord()
		{

			throw new NotImplementedException();
		}
	}
}
