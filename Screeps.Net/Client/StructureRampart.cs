using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class StructureRampart : OwnedStructure
	{
		public bool IsPublic { get; internal set; }
		public int TicksToDecay { get; internal set; }

		public Result SetPublic( bool isPublic )
		{
			throw new NotImplementedException();
		}
	}
}
