using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Action
{
	internal class SpawnCreep : GameAction
	{
		public CreepInfo CreepInfo { get; set; }
		public StructureSpawn Spawn { get; set; }

		public SpawnCreep( StructureSpawn spawn, CreepInfo info )
		{
			CreepInfo = info;
			Spawn = spawn;
		}

		public override int GroupID { get { return 1; } }
		public override int SortNum { get { return 1; } }

		public override Result Execute( Game game, Player player )
		{
			var result = new Result() { TypeID = ResultTypes.UnknownError };

			// make sure we get the root copy and not a local copy
			var localSpawn = game.Objects.FirstOrDefault( s => s.ID == Spawn.ID ) as StructureSpawn;

			if ( localSpawn != null )
			{
				if ( localSpawn.Spawning == null )
				{
					// set a local reference so the info object can calculate
					CreepInfo.Game = game;

					localSpawn.Spawning = CreepInfo;
					localSpawn.TimeToFinish = CreepInfo.TimeCost;
					localSpawn.Energy -= CreepInfo.EnergyCost;

					result = new Result() { TypeID = ResultTypes.Success };
				}
				else
				{
					result = new Result() { TypeID = ResultTypes.SpawnAlreadyBusy };
				}
			}
			else
			{
				result = new Result() { TypeID = ResultTypes.BadRequestID };
			}

			return result;
		}
	}
}
