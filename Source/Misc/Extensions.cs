using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Wordler {
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

		// credit: https://stackoverflow.com/a/607216/6431494
		public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() {
			return 
				(from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
				from t in a.GetTypes()
				let attributes = t.GetCustomAttributes(typeof(TAttribute), true)
				where attributes != null && attributes.Length > 0
				select  t);
		}

		public static Type GetSolutionType(string solName) {
			Type ans = null;
			IEnumerable<Type> solutions = Extensions.GetTypesWithAttribute<SolutionNamesAttribute>();
			foreach(Type solution in solutions) {
				var solutionNamesAttributes = (SolutionNamesAttribute[])solution.GetCustomAttributes(typeof(SolutionNamesAttribute), true);
				var solutionNamesAttribute = solutionNamesAttributes[0];
				foreach(string name in solutionNamesAttribute.Names.Split('|')) {
					if(name == solName) {
						ans = solution;
						break;
					}
				}
			}
			return ans;
		}

		public static (int Part, int TotalParts)? GetDivision(this string div) {
			(int Part, int TotalParts) ans = (0, 0);
			if(!Regex.IsMatch(div, @"^\d+\/\d+$"))
				return null;
			ans.Part = int.Parse(Regex.Match(div, @"\d+(?=\/)").Value);
			ans.TotalParts = int.Parse(Regex.Match(div, @"(?<=\/)\d+").Value);

			if(ans.Part <= 0 || ans.TotalParts <= 0 || ans.Part > ans.TotalParts)
				return null;

			return ans;
		}
	}
}
