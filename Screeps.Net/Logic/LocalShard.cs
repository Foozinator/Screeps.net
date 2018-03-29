using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Logic
{
	public class LocalShard : Shard
	{
		internal LocalShard( ShardConfiguration config )
			: base( config )
		{
			var game = Games.FirstOrDefault();

			if ( game != null )
			{
				var room = new Client.Room() { ID = CreateID(), Name = "Local", IsAvailable = true, Game = game, Memory = new Dictionary<string, object>() };

				room.Memory[ "#" ] = 1;
				game.Memory.Add( room.ID, room.Memory );

				int x, y;

				for ( x = 1; x <= 12; x++ )
				{
					room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = x, Y = 1, Room = room }, Type = TerrainTypes.Wall } );
					room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = x, Y = 12, Room = room }, Type = TerrainTypes.Wall } );
				}

				for ( y = 1; y <= 12; y++ )
				{
					room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 1, Y = y, Room = room }, Type = TerrainTypes.Wall } );
					room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 12, Y = y, Room = room }, Type = TerrainTypes.Wall } );
				}

				room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 4, Y = 3, Room = room }, Type = TerrainTypes.Wall } );
				room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 5, Y = 3, Room = room }, Type = TerrainTypes.Wall } );
				room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 4, Y = 4, Room = room }, Type = TerrainTypes.Wall } );
				room._terrain.Add( new Terrain() { Pos = new Client.RoomPosition() { X = 4, Y = 5, Room = room }, Type = TerrainTypes.Wall } );

				game._objects.Add( new Client.StructureController()
				{
					ID = CreateID(),
					Room = room,
					Pos = room.GetPositionAt( 6, 3 ),
					HitPoints = 200,
					HitPointsMax = 200,
					Type = Client.StructureTypes.Controller,
					ProgressTotal = 200,
					TicksToDowngrade = 20000,
				} );

				game._objects.Add( new Client.Source()
				{
					ID = CreateID(),
					Room = room,
					Pos = room.GetPositionAt( 8, 8 ),
					Energy = 2500,
					EnergyCapacity = 3000,
					TicksToRegenerate = 2000,
				} );

				game._rooms.Add( room );

				//var worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
				//worker.DoWork += Worker_DoWork;

				//worker.RunWorkerAsync( room );
			}
		}

		public override void FinishInit( Client.Game game )
		{
			base.FinishInit( game );

			if ( game.Rooms.Count > 1 )
			{
				FireEvent( game, new GameEventArgs() { Type = GameEventTypes.ChooseRoom } );
			}
			else
			{
				FireEvent( game.Rooms.FirstOrDefault(), new GameEventArgs() { Type = GameEventTypes.ChooseSpawnPosition } );
			}
		}

		//private void Worker_DoWork( object sender, DoWorkEventArgs e )
		//{
		//	System.Threading.Thread.Sleep( TimeSpan.FromSeconds( 2 ) );
		//}
	}
}
