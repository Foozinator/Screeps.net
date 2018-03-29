using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Action;

namespace Screeps.Net.Client
{
	public class Game : GameObject
	{
		internal List<ConstructionSite> _constructionSites = new List<ConstructionSite>();
		public IReadOnlyList<ConstructionSite> ConstructionSites { get { return _constructionSites; } }

		//public int Cpu { get { return 0; } }
		internal List<GameObject> _objects = new List<GameObject>();
		public IReadOnlyList<GameObject> Objects { get { return _objects; } }

		public IReadOnlyList<Creep> Creeps
		{
			get
			{
				return _objects
					// filter
					.Where( o => o.GetType().Name == "Creep" )
					// cast
					.Select( o => (Creep)o )
					.ToList();
			}
		}

		public IReadOnlyList<Flag> Flags
		{
			get
			{
				return _objects
					// filter
					.Where( o => o.GetType().Name == "Flag" )
					// cast
					.Select( o => (Flag)o )
					.ToList();
			}
		}


		internal List<Player> _players = new List<Player>();
		public IReadOnlyList<Player> Players { get { return _players; } }

		// TODO: public GlobalControlLevel Gcl { get; internal set; }
		public Map Map { get; internal set; }

		// private ? _resources;
		// public ? Resources { get { return _resources; } }

		internal Dictionary<string, Dictionary<string, object>> Memory { get; set; }

		internal List<Room> _rooms = new List<Room>();
		public IReadOnlyList<Room> Rooms { get { return _rooms; } }

		public Shard Shard { get; internal set; }

		public IReadOnlyList<Structure> Structures
		{
			get
			{
				return _objects
					// filter
					.Where( o => o.GetType().Name == "Structure" )
					// cast
					.Select( o => (Structure)o )
					.ToList();
			}
		}


		public UInt64 Time { get; internal set; }

		public GameObject GetObjectByID( string id )
		{
			return _objects.FirstOrDefault( o => o.ID == id );
		}

		//public int Notify( string msg, int groupInterval )
		//{
		//	return -1;
		//}

		internal Logic.Shard Server { get; set; }
		internal Game( Logic.Shard server )
		{
			Server = server;
			Time = server.Time;
			Memory = new Dictionary<string, Dictionary<string, object>>();
		}

		public Player AddPlayer( string name, Type controllerType )
		{
			var player = new Player() { Name = name, ControllerType = controllerType, ID = Server.CreateID(), Game = this };

			_players.Add( player );

			return player;
		}

		#region Game Actions
		public Result ChooseSpawn( RoomPosition pos, Player owner, string name )
		{
			return Server.ChooseSpawn( pos, owner, name );
		}

		internal List<GameAction> _actions = new List<GameAction>();
		internal Result AddAction( GameAction action )
		{
			_actions.Add( action );
			return new Result() { TypeID = ResultTypes.Success };
		}
		#endregion
	}
}
