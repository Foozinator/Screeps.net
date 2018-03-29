using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class Map
	{
		private List<Room> _rooms = new List<Room>();
		public IReadOnlyList<Room> Rooms { get { return _rooms; } }

		public Route FindRoute( string fromRoom, string toRoom )
		{
			return FindRoute( _rooms.FirstOrDefault( r => r.Name == fromRoom ), _rooms.FirstOrDefault( r => r.Name == toRoom ) );
		}
		public Route FindRoute( Room fromRoom, Room toRoom )
		{
			Route route = null;

			if ( fromRoom != null && toRoom != null )
			{
				throw new NotImplementedException();
			}

			return route;
		}

		//public TerrainTypes GetTerrainAt( int x, int y, string roomName )
		//{
		//	TerrainTypes result = TerrainTypes.Plain;
		//	Room room = _rooms.FirstOrDefault( r => r.Name == roomName );

		//	if ( room != null )
		//	{
		//		result = GetTerrainAt( room.GetPositionAt( x, y ) );
		//	}

		//	return result;
		//}
		//public TerrainTypes GetTerrainAt( RoomPosition pos )
		//{
		//	TerrainTypes result = TerrainTypes.Plain;

		//	if ( pos != null && pos.Room != null )
		//	{
		//		result = pos.Room.GetTerrainAt( pos );
		//	}

		//	return result;
		//}

		public int GetWorldSize()
		{
			throw new NotImplementedException();
		}

		// MOVED TO: Room.IsAvailable
		//public bool IsRoomAvailable( string roomName )
		//{
		//	return IsRoomAvailable( _rooms.FirstOrDefault( r => r.Name == roomName ) );
		//}
		//public bool IsRoomAvailable( Room room )
		//{
		//	bool isAvailable = false;

		//	if ( room != null )
		//	{
		//		throw new NotImplementedException();
		//	}

		//	return isAvailable;
		//}
	}

	public class Route
	{
		internal List<PathStep> _path = new List<PathStep>();
		public IReadOnlyList<PathStep> Path { get { return _path; } }

		public int Ops { get; internal set; }
		public int Cost { get; internal set; }
		public bool Incomplete { get; internal set; }
	}

	public class PathStep : RoomPosition
	{
		public int Cost { get; set; }

		public override string ToString()
		{
			string str = base.ToString();

			if ( Cost > 0 )
			{
				str += string.Format( ", ({0})", Cost );
			}

			return str;
		}
	}

	public class RoomPosition
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Room Room { get; set; }

		public override bool Equals( object obj )
		{
			return ( obj != null )
				&& ( obj is RoomPosition )
				&& ( ( (RoomPosition)obj ).X == X )
				&& ( ( (RoomPosition)obj ).Y == Y )
				&& ( ( (RoomPosition)obj ).Room.Equals( Room ) );
		}

		internal int GetRangeTo( RoomPosition pos )
		{
			int range = int.MaxValue;
			var navigator = Room.GetNavigator();

			if ( navigator != null )
			{
				var route = navigator.GetPath( this, pos );

				if ( route != null && route.Path != null && route.Path.Count > 0 )
				{
					range = route.Path.Count;
				}
			}

			return range;
		}

		public override int GetHashCode()
		{
			return string.Format( "{0},{1},{2}", X, Y, ( Room != null ) ? Room.ID : string.Empty ).GetHashCode();
		}

		public override string ToString()
		{
			string str = string.Format( "{0}, {1}", X, Y );

			if ( Room != null )
			{
				str = string.Format( "{0}, {1}, {2}", X, Y, Room.Name );
			}

			return str;
		}
	}

	public enum Directions : byte
	{
		Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft
	}
}
