using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace Wordler {
	static class Cacher {
		private static string path;
		private static XDocument doc = new XDocument();
		private static Object docLock = new Object();
		private static string section = "";

		public static void LoadFromWordListPath(string wordListPath) {
			path = Path.ChangeExtension(wordListPath, ".cache.xml");
			if(File.Exists(path))
				doc = XDocument.Load(path);
		}

		public static void SetSection(bool hard, Type solution) {
			section = (hard ? "hard" : "nothard") + solution.Name;
		}

		public static List<(TKey Key, TValue Value)> GetCache<TKey, TValue>(string section = "") {
			if(section == "")
				section = Cacher.section;

			List<(TKey Key, TValue Value)> ans = new List<(TKey Key, TValue Value)>();

			lock(docLock) {
				if(doc.Element(section) is not null && doc.Element(section).HasElements)
					ans = doc.Element(section).Elements().Select(e => (
						(TKey)Convert.ChangeType(e.Name.ToString(), typeof(TKey)),
						(TValue)Convert.ChangeType(e.Value.ToString(), typeof(TValue))
					)).ToList();
			}

			return ans;
		}

		public static void AddCache<TKey, TValue>(List<(TKey Key, TValue Value)> elements, string section = "") {
			if(section == "")
				section = Cacher.section;
			lock(docLock) {
				if(doc.Element(section) == null)
					doc.Add(new XElement(section));
				foreach(var element in elements)
					doc.Element(section).Add(new XElement(
						element.Key.ToString(),
						element.Value.ToString()
					));
				doc.Save(path);
			}
		}
	}
}
