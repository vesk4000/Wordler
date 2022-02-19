using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordler
{
	// credit: https://stackoverflow.com/a/21886340
	public class DuplicateKeyComparerDescending<TKey> : IComparer<TKey> where TKey : IComparable
	{
		public int Compare(TKey x, TKey y)
		{
			int result = x.CompareTo(y);

			if (result == 0)
				return 1; // Handle equality as being greater. Note: this will break Remove(key) or
			else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
				return result;
		}
	}
}
