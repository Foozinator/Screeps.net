using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public class GlobalControlLevel
	{
		public int Level { get; internal set; }
		public int Progress { get; internal set; }
		public int ProgressTotal { get; internal set; }
	}
}
