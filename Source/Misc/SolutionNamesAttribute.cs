using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordler {
	class SolutionNamesAttribute : Attribute {
		public string Names { get; set; }

		public SolutionNamesAttribute(string Names) {
			this.Names = Names;
		}
	}
}
