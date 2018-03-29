using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net
{
	public class Player	: GameObject
	{
		public string Name { get; internal set; }
		public Type ControllerType { get; internal set; }
	}
}
