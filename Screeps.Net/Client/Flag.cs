using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class Flag : RoomObject
	{
		public Color PrimaryColor { get; internal set; }
		public Color SecondaryColor { get; internal set; }
		public string Name { get; internal set; }

		public Dictionary<string, object> Memory { get; set; }

		public Result Remove()
		{
			throw new NotImplementedException();
		}

		public Result SetColor( Color primary )
		{
			throw new NotImplementedException();
		}
		public Result SetColor( Color primary, Color secondary )
		{
			throw new NotImplementedException();
		}

		public Result SetPosition( int x, int y )
		{
			throw new NotImplementedException();
		}
		public Result SetPosition( RoomPosition pos )
		{
			throw new NotImplementedException();
		}
	}
}
