using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Action
{
	internal class CreepTransfer : GameAction
	{
		public Creep Creep { get; set; }
		public IEnergySource Source { get; set; }

		public override int GroupID { get { return -1; } }
		public override int SortNum { get { return -1; } }

		public CreepTransfer( Creep crp, IEnergySource src )
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
				int transferAmount = 0;

				if ( Creep._carrying != null && Creep._carrying.ContainsKey( ResourceTypes.Energy ) )
				{
					transferAmount = Creep._carrying[ ResourceTypes.Energy ];
				}

				if ( transferAmount > 0 )
				{
					// we can't have this public, and the iterface won't let us have something internal
					// so we have to cheat, a little
					var src = Source as Source;
					var strct = Source as StructureWithEnergy;

					if ( src != null )
					{
						if ( src.Energy + transferAmount > src.EnergyCapacity )
						{
							transferAmount = src.EnergyCapacity - src.Energy;
						}

						src.Energy += transferAmount;

						if ( Creep._carrying[ ResourceTypes.Energy ] == transferAmount )
						{
							Creep._carrying.Remove( ResourceTypes.Energy );
						}
						else
						{
							Creep._carrying[ ResourceTypes.Energy ] -= transferAmount;
						}

						result = new Result() { TypeID = ResultTypes.Success };
					}
					else if ( strct != null )
					{
						if ( strct.Energy + transferAmount > strct.EnergyCapacity )
						{
							transferAmount = strct.EnergyCapacity - strct.Energy;
						}

						strct.Energy += transferAmount;

						if ( Creep._carrying[ ResourceTypes.Energy ] == transferAmount )
						{
							Creep._carrying.Remove( ResourceTypes.Energy );
						}
						else
						{
							Creep._carrying[ ResourceTypes.Energy ] -= transferAmount;
						}

						result = new Result() { TypeID = ResultTypes.Success };
					}
				}
				else
				{
					result = new Result() { TypeID = ResultTypes.CreepEmpty };
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
