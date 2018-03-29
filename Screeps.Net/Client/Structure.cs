using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public abstract class Structure : RoomObject
	{
		public int HitPoints { get; internal set; }
		public int HitPointsMax { get; internal set; }
		public virtual StructureTypes Type { get; internal set; }

		public virtual Result Destroy()
		{
			throw new NotImplementedException();
		}

		public bool IsActive { get; internal set; }

		// TODO: public bool NotifyWhenAttacked { get; internal set; }
	}
}
