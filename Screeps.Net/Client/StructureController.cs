using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class StructureController : OwnedStructure
	{
		public int Level { get; set; }
		public int Progress { get; set; }
		public int ProgressTotal { get; set; }
		public int TicksToDowngrade { get; set; }

		public int GetProgressTotal( int level )
		{
			switch ( level )
			{
			case 1: return 200;
			case 2: return 5000; // 45000;
			case 3: return 135000;
			case 4: return 405000;
			case 5: return 1215000;
			case 6: return 3645000;
			case 7: return 10935000;
			default: return -1;
			}
		}

		public int GetTicksToDowngrade( int level )
		{
			switch ( level )
			{
			case 1: return 20000;
			case 2: return 5000;
			case 3: return 10000;
			case 4: return 20000;
			case 5: return 40000;
			case 6: return 60000;
			case 7: return 100000;
			default: return 150000;
			}
		}

		internal override void UpdateTick()
		{
			base.UpdateTick();

			if ( TicksToDowngrade > 0 )
			{
				TicksToDowngrade--;
			}
			else
			{
				Level = Math.Max( 0, Level - 1 );

				if ( Level < 1 )
				{
					Owner = null;
				}
				else
				{
					ProgressTotal = GetProgressTotal( Level + 1 );
					TicksToDowngrade = GetTicksToDowngrade( Level + 1 );
				}
			}
		}
	}
}
