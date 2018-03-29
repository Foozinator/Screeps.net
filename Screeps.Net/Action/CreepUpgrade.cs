using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Action
{
	internal class CreepUpgrade : GameAction
	{
		public Creep Creep { get; set; }
		public StructureController Controller { get; set; }

		public override int GroupID { get { return -1; } }
		public override int SortNum { get { return -1; } }

		public CreepUpgrade( Creep crp, StructureController ctrl )
		{
			Creep = crp;
			Controller = ctrl;
		}

		public override Result Execute( Game game, Player player )
		{
			var result = new Result() { TypeID = ResultTypes.UnknownError };

			// switch to root copies before processing
			Creep = game.Creeps.FirstOrDefault( c => c.ID == Creep.ID );
			Controller = game.Objects.FirstOrDefault( s => s.ID == Controller.ID ) as StructureController;

			int range = Creep.Pos.GetRangeTo( Controller.Pos );

			if ( range <= 1 )
			{
				int transferAmount = 0;

				if ( Creep._carrying != null && Creep._carrying.ContainsKey( ResourceTypes.Energy ) )
				{
					transferAmount = Creep._carrying[ ResourceTypes.Energy ];
				}

				if ( transferAmount > 0 )
				{
					// no owner, take control
					if ( Controller.Owner == null || Controller.Owner.ID == player.ID )
					{
						Controller.Owner = player;
						int used = Upgrade( Controller, transferAmount );
						Creep._carrying.Remove( ResourceTypes.Energy );

						result = new Result() { TypeID = ResultTypes.Success };
					}
					else if ( Controller.Owner.ID != player.ID )
					{
						result = new Result() { TypeID = ResultTypes.NotOwner };
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

		private int Upgrade( StructureController controller, int amount )
		{
			int used = amount;

			if ( controller.ProgressTotal <= ( amount + controller.Progress ) )
			{
				controller.Level++;
				controller.ProgressTotal = controller.GetProgressTotal( controller.Level + 1 );
				controller.TicksToDowngrade = controller.GetTicksToDowngrade( controller.Level + 1 );
				controller.Progress = 0;
			}
			else
			{
				controller.Progress += amount;
				controller.TicksToDowngrade = controller.GetTicksToDowngrade( controller.Level + 1 );
			}

			return used;
		}
	}
}
