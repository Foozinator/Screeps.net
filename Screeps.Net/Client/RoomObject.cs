﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class RoomObject : GameObject
	{
		public RoomPosition Pos { get; internal set; }
		public Room Room { get; internal set; }
	}
}
