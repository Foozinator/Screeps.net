using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Action
{
	internal abstract class GameAction
	{
		public abstract int GroupID { get; }
		public abstract int SortNum { get; }

		public abstract Result Execute( Client.Game game, Player player );
	}
}
