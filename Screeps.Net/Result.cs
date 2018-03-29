using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screeps.Net
{
	public enum ResultTypes : byte
	{
		UnknownError = 0,
		Success,

		// specific errors
		InvalidPoint = 100,
		SpawnMustBeOnAPlain,
		SpawnAlreadyBusy,
		BadRequestID,
		TooFar,
		PathNotFound,
		CreepFull,
		CannotHarvestObject,
		EmptyPath,
		CreepNotFound,
		CreepEmpty,
		InvalidTarget,
		EmptyTarget,
		NotOwner
	}

	public class Result
	{
		public ResultTypes TypeID { get; set; }

		public override string ToString()
		{
			return TypeID.ToString();
		}
	}
}
