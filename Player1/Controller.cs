using System.Collections.Generic;
using System.Linq;
using Screeps.Net;
using Screeps.Net.Client;

namespace Player1
{
	public class Controller : PlayerController
	{
		protected override void DoGameTick()
		{
			CheckInit();

			foreach ( var room in Game.Rooms )
			{
				var spawn = room.Objects
					.FirstOrDefault( o => o is StructureSpawn );

				if ( spawn != null )
				{
					ManageRoom( (StructureSpawn)spawn );
				}
			}
		}

		private void ManageRoom( StructureSpawn spawn )
		{
			if ( spawn.Spawning == null && spawn.Room.EnergyAvailable >= spawn.Room.EnergyCapacity )
			{
				// check creeps population
				int workerCount = spawn.Room.Creeps
					.Where( c => (string)c.Memory[ "Role" ] == "worker" )
					.Count();

				if ( workerCount < ( spawn.Room.Controller.Level * 2 )
					|| ( spawn.Room.Controller.Level == 0 && workerCount == 0 ) )
				{
					var spawnInfo = spawn.SpawnCreep( new CreepInfo()
					{
						Body = new BodyPartTypes[] { BodyPartTypes.Work, BodyPartTypes.Carry, BodyPartTypes.Move }.ToList(),
						Memory = new Dictionary<string, object>() { { "Role", "worker" }, { "task", string.Empty } },
						DryRun = false,
					} );
				}
			}

			foreach ( var creep in spawn.Room.Creeps )
			{
				// TODO: Switch to creep child classes
				switch ( creep.Memory[ "Role" ] )
				{
				case "worker":

					switch ( creep.Memory[ "task" ] )
					{

					case "harvesting":
						if ( creep.CarryLoad < creep.CarryCapacity )
						{
							var source = spawn.Room.Objects.FirstOrDefault( o => o is Source ) as Source;

							if ( creep.Harvest( source ).TypeID == ResultTypes.TooFar )
							{
								creep.MoveTo( source );
							}
						}
						else
						{
							// done w/ harvesting, find something else
							creep.Memory[ "task" ] = "harvest";

							var targets = creep.Room.Objects
								.Where( o => typeof( StructureWithEnergy ).IsAssignableFrom( o.GetType() ) )
								.Where( o => ( (StructureWithEnergy)o ).Energy < ( (StructureWithEnergy)o ).EnergyCapacity )
								.ToList();

							if ( targets != null && targets.Count > 0 )
							{
								var target = targets.First();

								if ( creep.Transfer( target ).TypeID == ResultTypes.TooFar )
								{
									creep.MoveTo( target );
								}
							}

						}
						break;

					default: // harvest
						if ( creep.CarryLoad == 0 )
						{
							var source = spawn.Room.Objects.FirstOrDefault( o => o is Source ) as Source;

							if ( source != null )
							{
								if ( creep.Harvest( source ).TypeID == ResultTypes.TooFar )
								{
									creep.MoveTo( source );
								}
								else
								{
									creep.Memory[ "task" ] = "harvesting";
								}
							}
						}
						else
						{
							var targets = creep.Room.Objects
								.Where( o => typeof( StructureWithEnergy ).IsAssignableFrom( o.GetType() ) )
								.Where( o => ( (StructureWithEnergy)o ).Energy < ( (StructureWithEnergy)o ).EnergyCapacity )
								.ToList();

							if ( targets != null && targets.Count > 0 )
							{
								var target = targets.First();

								if ( creep.Transfer( target ).TypeID == ResultTypes.TooFar )
								{
									creep.MoveTo( target );
								}
							}
							else if ( creep.UpgradeController( creep.Room.Controller ).TypeID == ResultTypes.TooFar )
							{
								creep.MoveTo( creep.Room.Controller );
							}
						}
						break;
					}

					break;
				}
			}
		}

		private void CheckInit()
		{
			//string initKey = "Init";

			//if ( !Memory.ContainsKey( initKey ) )
			//{
			//	Memory[ initKey ] = true;
			//}
		}
	}
}
