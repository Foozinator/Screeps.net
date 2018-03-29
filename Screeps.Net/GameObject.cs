using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net
{
	public class GameObject
	{
		public Client.Game Game { get; set; }
		public string ID { get; internal set; }

		internal virtual void UpdateTick() { }
	}

	public enum GameEventTypes : byte
	{
		None = 0,
		RoomChanged,
		ChooseRoom,
		ChooseSpawnPosition,
		GameStep
	}

	public class GameEventArgs : EventArgs
	{
		public GameEventTypes Type { get; set; }
		public string Message { get; set; }
	}
}
