using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class Source : RoomObject, IEnergySource
	{
		public int Energy { get; internal set; }
		public int EnergyCapacity { get; internal set; }
		public int TicksToRegenerate { get; internal set; }

		internal override void UpdateTick()
		{
			base.UpdateTick();

			if ( TicksToRegenerate > 0 )
			{
				TicksToRegenerate--;
			}
			else
			{
				TicksToRegenerate = 2000;
				Energy = EnergyCapacity;
			}
		}
	}
}
