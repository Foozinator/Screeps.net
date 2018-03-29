using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class OwnedStructure : Structure
	{
		public bool My { get; internal set; }
		public Player Owner { get; internal set; }

		public Dictionary<string, object> Memory { get; set; }
	}
}
