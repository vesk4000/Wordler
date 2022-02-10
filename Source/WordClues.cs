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
		public HashSet<Tuple<int, char>> Yellows;
		public HashSet<char> Greys;

		public WordClues(Dictionary<int, char> inputGreens,
						 HashSet<Tuple<int, char>> inputYellows,
						 List<char> inputGreys)
		{
			this.Greens = new Dictionary<int, char>(inputGreens);
			this.Yellows = new HashSet<Tuple<int, char>>(inputYellows);
			this.Greys = new HashSet<char>(inputGreys);
		}

		public WordClues() {
			this.Greens = new Dictionary<int, char>();
			this.Yellows = new HashSet<Tuple<int, char>>();
			this.Greys = new HashSet<char>();
		}

		public bool Match(string word)
		{
			if(word == "cigar")
            {
				;
            }
			int index = 0;
			foreach (char letter in word)
			{
				if (Greys.Contains(letter)) return false;

				if (Yellows.Any(e => e.Item1 == index && e.Item2 == letter)) return false;

				if (Greens.Any(e => e.Key == index && e.Value != letter)) return false;
				index++;
			}
			
			return true;
		}

		public string NextWord()
		{

			throw new NotImplementedException();
		}
	}
}
