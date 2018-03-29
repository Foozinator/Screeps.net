using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net
{
	public enum TerrainTypes : byte
	{
		Plain = 0,
		Swamp = 1,
		Wall = 2,
		Exit = 3
	}

	public class Terrain
	{
		public RoomPosition Pos { get; internal set; }
		public TerrainTypes Type { get; internal set; }
	}
}
