using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net
{
	public class CreepInfo
	{
		internal Game Game { get; set; }

		public List<BodyPartTypes> Body { get; set; }
		public string Name { get; set; }
		public Dictionary<string, object> Memory { get; set; }
		public bool DryRun { get; set; }

		public int TimeCost
		{
			get
			{
				int cost = Body.Count * 2;

				//if ( Game != null )
				//{
				//	cost = 0;

				//	foreach ( var type in Body )
				//	{
				//		cost += Game.Server.Configuration.CreepInfo.BodyPartCosts[ type ];
				//	}
				//}

				return cost;
			}
		}

		public int EnergyCost
		{
			get
			{
				int cost = Body.Count * 100;

				if ( Game != null )
				{
					cost = 0;

					foreach ( var type in Body )
					{
						cost += Game.Server.Configuration.CreepInfo.BodyPartCosts[ type ];
					}
				}

				return cost;
			}
		}
	}
}
