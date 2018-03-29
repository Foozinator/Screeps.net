using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public enum StructureTypes : byte
	{
		Controller,
		Spawn
	}

	public class ConstructionSite : RoomObject
	{
		public bool My { get; internal set; }
		public Player Owner { get; internal set; }
		public int Progress { get; internal set; }
		public int ProgressTotal { get; internal set; }
		public StructureTypes StructureType { get; internal set; }

		public void Remove()
		{
			throw new NotImplementedException();
		}
	}
}
