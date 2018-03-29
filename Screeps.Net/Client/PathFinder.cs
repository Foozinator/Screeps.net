using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class PathFinder
	{
		public Route Search( RoomPosition origin, GameObject goal, SearchOptions options = null )
		{
			return null;
		}
	}

	public class CostMatrix
	{
	}

	public class SearchOptions
	{
		public delegate CostMatrix SearchRoomHandler( string roomname );
		//public event SearchRoomHandler RoomCallback;

		public int PlainCost { get; set; }
		public int SwampCost { get; set; }
		public bool Flee { get; set; }
		//public int MaxOps { get; set; }
		public int MaxRooms { get; set; }
		public int MaxCost { get; set; }
		//public float HeuristicWeight { get; set; }

		public SearchOptions()
		{
			PlainCost = 1;
			SwampCost = 5;
			Flee = false;
			MaxRooms = 16;
			MaxCost = int.MaxValue;
		}
	}
}
