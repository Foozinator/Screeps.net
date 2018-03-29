using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net.Client
{
	public enum ShardTypes : byte
	{
		Local //, Sandbox, MultiPlayer
	}

	public class Shard
	{
		private Logic.Shard _server;

		internal Shard( Logic.Shard server )
		{
			_server = server;

			Name = _server.Configuration.Name;
			Type = _server.Configuration.ShardType;
		}

		public string Name { get; internal set; }
		public ShardTypes Type { get; internal set; }
	}
}
