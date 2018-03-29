using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Logic
{
	internal class PlayerFactory
	{
		internal static Game GetModel( Game game, Player user )
		{
			var model = new Game( game.Server )
			{
				ID = game.ID,
				Time = game.Time,
				Shard = new Client.Shard( game.Server )
				{
					Name = game.Shard.Name,
					Type = game.Shard.Type,
				}
			};

			foreach ( var player in game.Players )
			{
				// TODO: IF user can see the player
				model._players.Add( new Player()
				{
					Name = player.Name,
				} );
			}

			foreach ( var room in game.Rooms )
			{
				var modelRoom = new Room()
				{
					Game = model,
					ID = room.ID,
					Name = room.Name,
					IsAvailable = room.IsAvailable,
					_terrain = room._terrain,
					Memory = new Dictionary<string, object>(),
				};

				model.Memory.Add( modelRoom.ID, modelRoom.Memory );

				if ( room.Memory != null )
				{
					foreach ( string key in room.Memory.Keys )
					{
						modelRoom.Memory.Add( key, room.Memory[ key ] );
					}

					modelRoom.Memory[ "#" ] = 2;
				}

				foreach ( var rmObj in room.Objects )
				{
					model._objects.Add( DeepCopyRoomObject( rmObj, modelRoom, model, user ) );
				}

				model._rooms.Add( modelRoom );
			}

			return model;
		}

		private static RoomObject DeepCopyRoomObject( RoomObject obj, Room room, Game game, Player user )
		{
			RoomObject copy = null;

			if ( obj != null )
			{
				switch ( obj.GetType().Name )
				{
				case "StructureSpawn":
					var spawn = (StructureSpawn)obj;
					copy = new StructureSpawn()
					{
						Game = game,
						My = spawn.Owner != null && spawn.Owner.ID == user.ID,
						Pos = room.GetPositionAt( spawn.Pos.X, spawn.Pos.Y ),
						Room = room,

						Energy = spawn.Energy,
						EnergyCapacity = spawn.EnergyCapacity,
						HitPoints = spawn.HitPoints,
						HitPointsMax = spawn.HitPointsMax,
						ID = spawn.ID,
						IsActive = spawn.IsActive,
						Name = spawn.Name,
						Owner = spawn.Owner,
						Spawning = spawn.Spawning,
						Type = spawn.Type,
						Memory = new Dictionary<string, object>(),
					};

					game.Memory.Add( copy.ID, ( (StructureSpawn)copy ).Memory );

					if ( spawn.Memory != null )
					{
						foreach ( string key in spawn.Memory.Keys )
						{
							( (StructureSpawn)copy ).Memory.Add( key, room.Memory[ key ] );
						}

						( (StructureSpawn)copy ).Memory[ "#" ] = 2;
					}
					break;

				case "StructureController":
					var controller = (StructureController)obj;
					copy = new StructureController()
					{
						Game = game,
						Room = room,

						Owner = controller.Owner,
						My = controller.Owner != null && controller.Owner.ID == user.ID,

						HitPoints = controller.HitPoints,
						HitPointsMax = controller.HitPointsMax,
						ID = controller.ID,
						IsActive = controller.IsActive,
						Pos = controller.Pos,
						Type = controller.Type,

						Progress = controller.Progress,
						ProgressTotal = controller.ProgressTotal,
						Level = controller.Level,
						TicksToDowngrade = controller.TicksToDowngrade,
					};
					break;

				case "Source":
					var source = (Source)obj;
					copy = new Source()
					{
						Game = game,
						Room = room,

						Energy = source.Energy,
						EnergyCapacity = source.EnergyCapacity,
						ID = source.ID,
						Pos = source.Pos,
						TicksToRegenerate = source.TicksToRegenerate,
					};
					break;

				case "Creep":
					var creep = (Creep)obj;

					// TODO: Player-defined child class

					copy = new Creep()
					{
						Game = game,
						My = ( creep.Owner.ID == user.ID ),
						Room = room,

						Body = creep.Body,
						Fatigue = creep.Fatigue,
						HitPoints = creep.HitPoints,
						HitPointsMax = creep.HitPointsMax,
						ID = creep.ID,
						Name = creep.Name,
						NotifyWhenAttacked = creep.NotifyWhenAttacked,
						Owner = creep.Owner,
						Pos = creep.Pos,
						Saying = creep.Saying,
						Spawning = creep.Spawning,
						TicksToLive = creep.TicksToLive,

						Memory = new Dictionary<string, object>(),
						_carrying = new Dictionary<ResourceTypes, int>(),
					};

					game.Memory.Add( copy.ID, ( (Creep)copy ).Memory );

					foreach ( string key in creep.Memory.Keys )
					{
						( (Creep)copy ).Memory.Add( key, creep.Memory[ key ] );
					}

					( (Creep)copy ).Memory[ "#" ] = 2;

					foreach ( var key in creep._carrying.Keys )
					{
						( (Creep)copy )._carrying.Add( key, creep._carrying[ key ] );
					}
					break;

				default:
					string type = obj.GetType().Name;
					break;
				}
			}

			return copy;
		}

		private static Creep DeepCopyCreep( Creep creep, Room modelRoom, Game model, Player user )
		{
			var copy = new Creep()
			{
				Game = model,
				My = ( creep.Owner.ID == user.ID ),
				Room = modelRoom,
				Pos = creep.Pos.Room.GetPositionAt( creep.Pos.X, creep.Pos.Y ),

				Body = creep.Body,
				Fatigue = creep.Fatigue,
				HitPoints = creep.HitPoints,
				HitPointsMax = creep.HitPointsMax,
				ID = creep.ID,
				Name = creep.Name,
				NotifyWhenAttacked = creep.NotifyWhenAttacked,
				Owner = creep.Owner,
				Saying = creep.Saying,
				Spawning = creep.Spawning,
				TicksToLive = creep.TicksToLive,
			};

			return copy;
		}

		internal static PlayerController GetController( Game model, Player user )
		{
			PlayerController controller = null;

			if ( user != null && user.ControllerType != null )
			{
				controller = user.ControllerType.Assembly.CreateInstance( user.ControllerType.FullName ) as PlayerController;

				if ( controller != null )
				{
					controller.Game = model;
				}
			}

			return controller;
		}
	}
}
