using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordler {
	static class Extensions {
		// credit: youtu.be/sIXKpyhxHR8
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> enumerable, int chunks) {
			int chunckSize = (int)Math.Ceiling((double)enumerable.Count() / chunks);
			return enumerable
				.Select((x, i) => new { Index = i, Value = x })
				.GroupBy(x => x.Index / chunckSize)
				.Select(x => x.Select(v => v.Value));
		}

		public static List<string> Tokenize(this string source) {
			bool hasSpaces = source.Contains(" ");
			string curr = "";
			List<string> ans = new List<string>();
			for(int i = 0; i < source.Length; ++i) {
				if(Char.IsLetter(source[i])) {
					if(curr != "") {
						ans.Add(curr);
						curr = "";
					}
					ans.Add(source[i].ToString());
				}
				else if(Char.IsDigit(source[i])) {
					if(hasSpaces) {
						curr += source[i];
					}
					else {
						ans.Add(source[i].ToString());
					}
				}
				else if(source[i] == ' ') {
					if(curr != "") {
						ans.Add(curr);
						curr = "";
					}
						
				}
				else {
					return null;
				}
			}
			if(curr != "")
				ans.Add(curr);
			return ans;
		}
	}
}
