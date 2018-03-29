using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class StructureSpawn : StructureWithEnergy
	{
		// Create a logger for use in this class
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public string Name { get; internal set; }
		public CreepInfo Spawning { get; internal set; }

		public int TimeToFinish { get; internal set; }

		public Result SpawnCreep( CreepInfo info )
		{
			Result result = new Result() { TypeID = ResultTypes.UnknownError };

			if ( Spawning == null )
			{
				// TODO: Check room controller level
				result = Game.AddAction( new Action.SpawnCreep( this, info ) );
			}
			else
			{
				result = new Result() { TypeID = ResultTypes.SpawnAlreadyBusy };
			}

			return result;
		}

		internal override void UpdateTick()
		{
			base.UpdateTick();

			if ( TimeToFinish > 0 )
			{
				TimeToFinish--;
			}
			else if ( Spawning != null )
			{
				log.Info( string.Format( "Spawn of {0} complete", Spawning ) );
				// find an adjacent position that's not a wall
				RoomPosition pos = null;
				bool spotIsSwamp = false;

				for ( int dx = -1; dx <= 1; dx++ )
				{
					for ( int dy = -1; dy <= 1; dy++ )
					{
						if ( !( dx == 0 && dy == 0 ) )
						{
							var test = Room.GetPositionAt( Pos.X + dx, Pos.Y + dy );

							switch ( Room.GetTerrainAt( test ) )
							{
							case TerrainTypes.Swamp:
								// double check that the spot is empty
								if ( !Room.Objects.Any( o => o.Pos.Equals( test ) ) )
								{
									pos = test;
									spotIsSwamp = true;
								}
								break;

							case TerrainTypes.Plain:
								if ( !Room.Objects.Any( o => o.Pos.Equals( test ) ) )
								{
									pos = test;
									spotIsSwamp = false;
								}
								break;
							}

							if ( pos != null && !spotIsSwamp )
							{
								break;
							}
						}
					}

					if ( pos != null && !spotIsSwamp )
					{
						break;
					}
				}

				Creep baby = new Creep()
				{
					Body = Spawning.Body,
					ID = Game.Server.CreateID(),
					Name = Spawning.Name,
					Owner = this.Owner,
					TicksToLive = Game.Server.Configuration.CreepInfo.CreepLifeTime,
					HitPoints = 100,
					HitPointsMax = 100,
					Fatigue = 0,
					Game = Game,
					Pos = pos ?? Pos,
					Room = Room,
					Memory = Spawning.Memory,
				};

				baby.Memory[ "#" ] = 1;

				Game._objects.Add( baby );
				Game.Memory.Add( baby.ID, baby.Memory );

				Spawning = null;
			}

			if ( Energy < EnergyCapacity )
			{
				Energy++;
			}
		}

		public Result RecycleCreep( Creep creep )
		{
			throw new NotImplementedException();
		}

		public Result RenewCreep( Creep creep )
		{
			throw new NotImplementedException();
		}
	}
}
