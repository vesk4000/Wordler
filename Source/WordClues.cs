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
						 HashSet<char> inputGreys)
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
            foreach (var yellow in Yellows)
            {
				if (!word.Contains(yellow.Item2)) return false;
            }
			if(word == "foyer")
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

        public void Add(Dictionary<int, char> greens, HashSet<Tuple<int, char>> yellows, HashSet<char> greys)
        {
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
