using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Action
{
	internal class CreepHarvest	: GameAction
	{
		public Creep Creep { get; set; }
		public IEnergySource Source { get; set; }

		public override int GroupID { get { return -1; } }
		public override int SortNum { get { return -1; } }

		public CreepHarvest( Creep crp, IEnergySource src )
		{
			Creep = crp;
			Source = src;
		}

		public override Result Execute( Game game, Player player )
		{
			var result = new Result() { TypeID = ResultTypes.UnknownError };

			// switch to root copies before processing
			Creep = game.Creeps.FirstOrDefault( c => c.ID == Creep.ID );
			Source = game.Objects.FirstOrDefault( s => s.ID == Source.ID ) as IEnergySource;

			int range = Creep.Pos.GetRangeTo( Source.Pos );

			if ( range <= 1 )
			{
				if ( Creep.CarryLoad < Creep.CarryCapacity )
				{
					int transferAmount = 1; // TODO: Creep.CanTransfer;

					if ( Source.Energy < transferAmount )
					{
						transferAmount = Source.Energy;
					}

					// we can't have this public, and the iterface won't let us have something internal
					// so we have to cheat, a little
					var src = Source as Source;
					var strct = Source as StructureWithEnergy;

					if ( src != null )
					{
						src.Energy -= transferAmount;
					}
					else if ( strct != null )
					{
						strct.Energy -= transferAmount;
					}
					else
					{
						transferAmount = 0;
					}

					if ( transferAmount > 0 )
					{
						if ( Creep._carrying.ContainsKey( ResourceTypes.Energy ) )
						{
							int carrying = Creep._carrying[ ResourceTypes.Energy ] + transferAmount;

							Creep._carrying[ ResourceTypes.Energy ] = carrying;

							// TODO: Add to events for UI to shows
						}
						else
						{
							Creep._carrying.Add( ResourceTypes.Energy, transferAmount );
						}

						result = new Result() { TypeID = ResultTypes.Success };
					}
				}
				else
				{
					result = new Result() { TypeID = ResultTypes.CreepFull };
				}
			}
			else
			{
				result = new Result() { TypeID = ResultTypes.TooFar };
			}

			return result;
		}
	}
}
