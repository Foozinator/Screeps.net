using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Action;

namespace Screeps.Net
{
	public abstract class PlayerController
	{
		public Client.Game Game { get; internal set; }

		internal List<GameAction> DoTick()
		{
			try
			{
				DoGameTick();
			}
			catch ( Exception ex )
			{
				// TODO: Report all the things
				string foo = ex.ToString();
			}

			var list = Game._actions;
			Game._actions = new List<GameAction>();

			return list;
		}

		protected abstract void DoGameTick();
	}
}
