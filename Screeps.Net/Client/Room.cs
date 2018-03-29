using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class Room : GameObject
	{
		#region Cannon Properties
		public StructureController Controller
		{
			get
			{
				return Objects.FirstOrDefault( o => o is StructureController ) as StructureController;
			}
		}
		public StructureStorage Storage
		{
			get
			{
				return Objects.FirstOrDefault( o => o is StructureStorage ) as StructureStorage;
			}
		}
		//public StructureTerminal Terminal
		//{
		//	get
		//	{
		//		return Objects.FirstOrDefault( o => o is StructureTerminal ) as StructureTerminal;
		//	}
		//}

		public int EnergyAvailable
		{
			get
			{
				return Objects
					.Where( o => typeof( StructureWithEnergy ).IsAssignableFrom( o.GetType() ) )
					.Select( o => (StructureWithEnergy)o )
					.Sum( s => s.Energy );
			}
		}

		public int EnergyCapacity
		{
			get
			{
				return Objects
					.Where( o => typeof( StructureWithEnergy ).IsAssignableFrom( o.GetType() ) )
					.Select( o => (StructureWithEnergy)o )
					.Sum( s => s.EnergyCapacity );
			}
		}

		public Dictionary<string, object> Memory { get; set; }
		public string Name { get; internal set; }
		#endregion

		#region Cannon Methods
		public Result CreateConstructionSite( int x, int y, StructureTypes type )
		{
			return CreateConstructionSite( GetPositionAt( x, y ), type );
		}
		public Result CreateConstructionSite( RoomPosition pos, StructureTypes type )
		{
			throw new NotImplementedException();
		}

		public Result CreateFlag( int x, int y, string name = null, Color primary = default( Color ), Color secondary = default( Color ) )
		{
			return CreateFlag( GetPositionAt( x, y ), name, primary, secondary );
		}
		public Result CreateFlag( RoomPosition pos, string name = null, Color primary = default( Color ), Color secondary = default( Color ) )
		{
			throw new NotImplementedException();
		}

		// TODO: public List<RoomObject> Find( ? search )
		// why?: public Directions FindExitTo( Room target )
		// not encapsulated: public Route FindPath( RoomPosition from, RoomPosition to, object options = null )

		public RoomPosition GetPositionAt( int x, int y )
		{
			RoomPosition pos = null;

			if ( x >= _terrain.Min( t => t.Pos.X )
				&& x <= _terrain.Max( t => t.Pos.X )
				&& y >= _terrain.Min( t => t.Pos.Y )
				&& y <= _terrain.Max( t => t.Pos.Y ) )
			{
				pos = new RoomPosition() { X = x, Y = y, Room = this };
			}

			return pos;
		}
		public RoomPosition GetPositionAt( Point pt )
		{
			return GetPositionAt( pt.X, pt.Y );
		}

		public IReadOnlyList<RoomObject> LookAt( int x, int y )
		{
			return LookAt( GetPositionAt( x, y ) );
		}
		public IReadOnlyList<RoomObject> LookAt( RoomPosition position )
		{
			return Objects
				.Where( o => o.Pos.Equals( position ) )
				.ToList();
		}

		// TODO: public List<RoomObject> LookAtArea( top, left, bottom, right )
		#endregion

		public IReadOnlyList<RoomObject> Objects
		{
			get
			{
				return Game.Objects
					// filter
					.Where( o => typeof( RoomObject ).IsAssignableFrom( o.GetType() ) && ( (RoomObject)o ).Pos.Room.ID == ID )
					// cast
					.Select( o => (RoomObject)o )
					.ToList();
			}
		}

		internal List<Terrain> _terrain = new List<Net.Terrain>();
		public IReadOnlyList<Terrain> Terrain { get { return _terrain; } }

		public IReadOnlyList<Creep> Creeps
		{
			get
			{
				return Game.Creeps
					// filter
					.Where( c => c.Pos.Room.ID == ID )
					// cast
					.ToList();
			}
		}

		public bool IsAvailable { get; internal set; }

		//public Room()
		//{
		//}

		public override bool Equals( object obj )
		{
			return ( obj != null )
				&& ( obj is Room )
				&& ( ( (Room)obj ).ID == ID );
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public TerrainTypes GetTerrainAt( int x, int y )
		{
			return GetTerrainAt( GetPositionAt( x, y ) );
		}
		public TerrainTypes GetTerrainAt( RoomPosition pos )
		{
			TerrainTypes result = TerrainTypes.Plain;
			var terrain = _terrain.FirstOrDefault( t => t.Pos.Equals( pos ) );

			if ( terrain != null )
			{
				result = terrain.Type;
			}

			return result;
		}

		public int MaxX
		{
			get
			{
				return _terrain.Max( t => t.Pos.X );
			}
		}

		public int MaxY
		{
			get
			{
				return _terrain.Max( t => t.Pos.Y );
				//int max = 1;

				//if ( TerrainStorage != null && TerrainStorage.Count > 0 )
				//{
				//	max = TerrainStorage.Max( p => p.Key.Y );
				//}

				//return max;
			}
		}

		internal Logic.Navigator GetNavigator()
		{
			Logic.Navigator navi = null;

			switch ( Game.Server.Configuration.MapType )
			{
			case Logic.MapTypes.Square:
				navi = new Logic.SquareMapNavigator();
				break;
			}

			return navi;
		}
	}
}
