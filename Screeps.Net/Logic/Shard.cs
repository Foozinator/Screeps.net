using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Action;
using Screeps.Net.Client;

namespace Screeps.Net.Logic
{
	/// <summary>
	/// Server-side controller for game logic, handles the primary game loop
	/// </summary>
	public class Shard
	{
		// Create a logger for use in this class
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public ShardConfiguration Configuration { get; private set; }

		private List<Game> _games = new List<Game>();
		public IReadOnlyList<Game> Games { get { return _games; } }

		public UInt64 Time { get; internal set; }

		/// <summary>
		/// Factory method for starting up a game instance
		/// </summary>
		/// <param name="config">Configuration for the game instance</param>
		/// <returns></returns>
		public static Shard Create( ShardConfiguration config )
		{
			Shard result = null;

			if ( config != null )
			{
				switch ( config.ShardType )
				{
				case ShardTypes.Local: result = new LocalShard( config ); break;
				}
			}

			return result;
		}

		public virtual Result Login( string name, string pwd = null )
		{
			throw new NotImplementedException();
		}

		public virtual Result Start()
		{
			BackgroundWorker bw = new BackgroundWorker()
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true,
			};

			bw.DoWork += GameLoopThread;

			bw.RunWorkerAsync();

			return new Result() { TypeID = ResultTypes.Success };
		}

		public virtual Result Stop()
		{
			_run = false;
			return new Result() { TypeID = ResultTypes.Success };
		}

		private bool _run = true;
		private void GameLoopThread( object sender, DoWorkEventArgs e )
		{
			while ( _run )
			{
				DateTime start = DateTime.Now;

				foreach ( var game in _games )
				{
					ProcessGameTick( game );
				}

				// loop is complete, save progress (every so many ticks...)
				//if ( _game.Tick % 20 == 0 )
				//{
				//	_storage.SavePoint();
				//}

				var nextTickStart = start.Add( Configuration.TickRate );
				var delay = nextTickStart.Subtract( DateTime.Now );

				if ( delay.TotalMilliseconds > 0 )
				{
					System.Threading.Thread.Sleep( delay );
				}
			}
		}

		private void ProcessGameTick( Game game )
		{
			game.Time++;

			log.Info( string.Format( "Tick: {0}", game.Time ) );

			// Handle all game-level updates (fatigue, refresh, etc)
			UpdateObjects( game );

			foreach ( var player in game.Players )
			{
				log.Info( string.Format( "Player: {0}", player.Name ) );

				DoPlayerTick( game, player );

				FireEvent( game, new GameEventArgs() { Type = GameEventTypes.GameStep, Message = "Completed Tick " + game.Time.ToString() } );
			}

			//game.Reporter.Indent--;

			//if ( game.Tick >= 10 )
			//{
			//	Stop();
			//}
		}

		private void DoPlayerTick( Game game, Player player )
		{
			var model = PlayerFactory.GetModel( game, player );
			var controller = PlayerFactory.GetController( model, player );
			var actions = controller.DoTick();

			ProcessActions( game, player, actions );

			CopyMemory( game, player, model );
		}

		private void CopyMemory( Game game, Player player, Game model )
		{
			foreach ( string playerMemKey in model.Memory.Keys )
			{
				var playerMem = model.Memory[ playerMemKey ];

				// Don't create a new instance, as that orphans all the game object references
				game.Memory[ playerMemKey ].Clear();

				foreach ( string objMemKey in playerMem.Keys )
				{
					if ( objMemKey != "#" )
					{
						game.Memory[ playerMemKey ].Add( objMemKey, playerMem[ objMemKey ] );
					}
					else
					{
						game.Memory[ playerMemKey ].Add( "#", 1 );
					}
				}
			}
		}

		private void UpdateObjects( Game game )
		{
			foreach ( var room in game.Rooms )
			{
				foreach ( var gameObj in room.Objects )
				{
					gameObj.UpdateTick();
				}
			}
		}

		private void ProcessActions( Game game, Player player, List<GameAction> actions )
		{
			// TODO: Grouping only for actions from the same object
			//// 1 Sort by group
			//var usedGroups = actions
			//	.Select( a => a.GroupID )
			//	.Distinct()
			//	.OrderBy( g => g );

			//// 2 Select first group (discard the others)
			//var firstGroupID = usedGroups.FirstOrDefault();

			//if ( firstGroupID != 0 )
			//{
			//	log.Info( string.Format( "Using Actions Group: {0}", firstGroupID ) );

			//	// 3 Sort by execute order in group
			//	var actionsInGroup = actions
			//		.Where( a => a.GroupID == firstGroupID )
			//		.OrderBy( a => a.SortNum );

			//	foreach ( var action in actionsInGroup )
				foreach ( var action in actions )
				{
					// 4 Execute in order
					log.Info( string.Format( "Executing: {0}", action ) );
					var actionResult = action.Execute( game, player );
					log.Info( string.Format( "- Result: {0}", actionResult ) );
				}
			//}
			// else possible there were no actions
		}

		public string CreateID()
		{
			return Guid.NewGuid().ToString().Replace( "-", string.Empty );
		}

		protected Shard( ShardConfiguration config )
		{
			Configuration = config;

			_games.Add( new Game( this )
			{
				Shard = new Client.Shard( this ),
			} );
		}

		public delegate void GameEventHandler( GameObject source, GameEventArgs args );
		public event GameEventHandler GameEvent;
		protected void FireEvent( GameObject source, GameEventArgs args )
		{
			if ( GameEvent != null )
			{
				GameEvent( source, args );
			}
		}

		public virtual void FinishInit( Client.Game game )
		{
		}

		internal Result ChooseSpawn( RoomPosition pos, Player owner, string name )
		{
			var result = new Result() { TypeID = ResultTypes.UnknownError };

			// must be a real position
			if ( pos != null && pos.Room != null )
			{
				// must be on a plain
				if ( pos.Room.GetTerrainAt( pos ) == TerrainTypes.Plain )
				{
					var spawn = new Client.StructureSpawn()
					{
						Owner = owner,
						Pos = pos,
						Room = pos.Room,
						Energy = Configuration.SpawnInfo.EnergyCapacity,
						EnergyCapacity = Configuration.SpawnInfo.EnergyCapacity,
						ID = CreateID(),
						Game = pos.Room.Game,
						HitPoints = Configuration.SpawnInfo.HitPoints,
						HitPointsMax = Configuration.SpawnInfo.HitPoints,
						Name = name,
						Type = StructureTypes.Spawn,

						Memory = new Dictionary<string, object>(),
					};

					pos.Room.Game._objects.Add( spawn );
					pos.Room.Game.Memory.Add( spawn.ID, spawn.Memory );
					result = new Result() { TypeID = ResultTypes.Success };

					FireEvent( pos.Room, new GameEventArgs() { Type = GameEventTypes.RoomChanged, Message = string.Format( "Spawn {0} Created", name ) } );

					// TODO: Check for any other requirements before game start
					Start();
				}
				else
				{
					result = new Result() { TypeID = ResultTypes.SpawnMustBeOnAPlain };
				}
			}
			else
			{
				result = new Result() { TypeID = ResultTypes.InvalidPoint };
			}

			return result;
		}
	}

	public enum MapTypes : byte
	{
		Square //, HorizontalHex, VerticalHex
	}

	/// <summary>
	/// Initial settings for creating a shard
	/// </summary>
	public class ShardConfiguration
	{
		public ShardTypes ShardType { get; set; }
		public MapTypes MapType { get; set; }
		public string Name { get; set; }

		public TimeSpan TickRate { get; set; }

		public CreepConfig CreepInfo { get; set; }
		public SpawnConfig SpawnInfo { get; set; }

		public ShardConfiguration()
		{
			// set defaults
			ShardType = ShardTypes.Local;
			MapType = MapTypes.Square;
			TickRate = TimeSpan.FromMilliseconds( 200 );

			// from Screeps site: http://docs.screeps.com/api/#
			CreepInfo = new CreepConfig()
			{
				BodyPartCosts = new Dictionary<BodyPartTypes, int>(),

				CreepLifeTime = 1500,
				CreepClaimLifeTime = 500,
			};

			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Move, 50 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Work, 100 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Attack, 80 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Carry, 50 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Heal, 250 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.RangedAttack, 150 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Tough, 10 );
			CreepInfo.BodyPartCosts.Add( BodyPartTypes.Claim, 600 );

			SpawnInfo = new SpawnConfig()
			{
				HitPoints = 5000,
				EnergyCapacity = 300,
				CreepSpawnTime = 3,
			};
		}
	}

	public class CreepConfig
	{
		public Dictionary<BodyPartTypes, int> BodyPartCosts { get; set; }

		public int CreepLifeTime { get; set; }
		public int CreepClaimLifeTime { get; set; }
	}

	public class SpawnConfig
	{
		public int HitPoints { get; set; }
		public int EnergyCapacity { get; set; }
		public int CreepSpawnTime { get; set; }
	}
}
