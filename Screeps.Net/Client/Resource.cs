using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public enum ResourceTypes : byte
	{
		Energy
	}

	public class Resource : RoomObject
	{
		public ResourceTypes Type { get; internal set; }
		public int Amount { get; internal set; }
	}
}
