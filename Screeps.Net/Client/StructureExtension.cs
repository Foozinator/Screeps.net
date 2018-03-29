using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public interface IEnergySource
	{
		int Energy { get; }
		int EnergyCapacity { get; }
		RoomPosition Pos { get; }
		string ID { get; }
	}

	public class StructureWithEnergy : OwnedStructure, IEnergySource
	{
		public int Energy { get; internal set; }
		public int EnergyCapacity { get; internal set; }
	}

	public class StructureExtension : StructureWithEnergy
	{
	}
}
