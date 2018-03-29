using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class StructureTower : OwnedStructure
	{
		public int Energy { get; internal set; }
		public int EnergyCapacity { get; internal set; }

		public Result Attack( RoomObject target )
		{
			throw new NotImplementedException();
		}

		public Result Heal( Creep creep )
		{
			throw new NotImplementedException();
		}

		public Result Repair( Structure structure )
		{
			throw new NotImplementedException();
		}
	}
}
