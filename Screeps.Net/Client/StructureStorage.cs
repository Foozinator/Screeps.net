using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class StructureStorage : OwnedStructure
	{
		public List<Resource> Store { get; internal set; }
		public int StoreCapacity { get; internal set; }
	}
}
