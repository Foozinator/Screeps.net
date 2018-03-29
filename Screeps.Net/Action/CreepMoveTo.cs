using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Action
{
	internal class CreepMoveTo : GameAction
	{
		public Creep Creep { get; set; }
		public RoomPosition Pos { get; set; }
		public MoveOptions Options { get; set; }

		public CreepMoveTo( Creep crp, RoomPosition pos, MoveOptions options )
		{
			Creep = crp;
			Pos = pos;
			Options = options;
		}

		public override int GroupID { get { return -1; } }
		public override int SortNum { get { return -1; } }

		public override Result Execute( Game game, Player player )
		{
			var result = new Result() { TypeID = ResultTypes.UnknownError };
			var navigator = Creep.Pos.Room.GetNavigator();

			if ( navigator != null )
			{
				var route = navigator.GetPath( Creep.Pos, Pos, Options );

				if ( route != null && route.Path != null )
				{
					// make sure we're working with the official copy
					var creep = game.Creeps.FirstOrDefault( c => c.ID == Creep.ID );

					if ( creep != null )
					{
						result = creep.Move( route );
					}
					else
					{
						result = new Result() { TypeID = ResultTypes.CreepNotFound };
					}
				}
				else
				{
					result = new Result() { TypeID = ResultTypes.PathNotFound };
				}
			}
			else
			{
				result = new Result() { TypeID = ResultTypes.UnknownError };
			}

			return result;
		}
	}
}
