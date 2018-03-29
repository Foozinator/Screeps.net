using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Action;

namespace Screeps.Net.Client
{
	//BODYPART_COST: {
 //       "move": 50,
 //       "work": 100,
 //       "attack": 80,
 //       "carry": 50,
 //       "heal": 250,
 //       "ranged_attack": 150,
 //       "tough": 10,
 //       "claim": 600
 //   },

	public enum BodyPartTypes : byte
	{
		Move, Work, Carry, Heal, Tough, Attack, RangedAttack, Claim
	}

	public class Creep : RoomObject
	{
		// Create a logger for use in this class
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public IReadOnlyList<BodyPartTypes> Body { get; internal set; }

		public Dictionary<string, object> Memory { get; set; }

		public int CarryCapacity
		{
			get
			{
				int capacity = 0;

				if ( Body != null )
				{
					capacity = Body.Where( b => b == BodyPartTypes.Carry ).Count() * 50; // TODO: configure
				}

				return capacity;
			}
		}

		public int CarryLoad
		{
			get
			{
				int load = _carrying.Sum( c => c.Value );

				return load;
			}
		}

		internal Dictionary<ResourceTypes, int> _carrying = new Dictionary<ResourceTypes, int>();

		public int Fatigue { get; internal set; }
		public int HitPoints { get; internal set; }
		public int HitPointsMax { get; internal set; }
		public int TicksToLive { get; internal set; }

		public bool My { get; internal set; }
		public bool Spawning { get; internal set; }
		public string Name { get; internal set; }
		public Player Owner { get; internal set; }
		public string Saying { get; internal set; }

		public Result Attack( RoomObject target )
		{
			throw new NotImplementedException();
		}

		public Result Build( ConstructionSite target )
		{
			throw new NotImplementedException();
		}

		public Result CancelOrder( string name )
		{
			throw new NotImplementedException();
		}

		public Result ClaimController( StructureController controller )
		{
			throw new NotImplementedException();
		}

		public Result Dismantle( RoomObject target )
		{
			throw new NotImplementedException();
		}

		public Result Drop( ResourceTypes type, int amount = 0 )
		{
			throw new NotImplementedException();
		}

		public Result GenerateSafeMode( StructureController controller )
		{
			throw new NotImplementedException();
		}

		// use LINQ instead? : public int GetActiveBodyParts( BodyPartTypes type )

		public Result Harvest( IEnergySource target )
		{
			var result = new Result { TypeID = ResultTypes.UnknownError };

			int range = Pos.GetRangeTo( target.Pos );

			if ( range <= 1 )
			{
				result = Game.AddAction( new CreepHarvest( this, target ) );
			}
			else
			{
				result = new Result { TypeID = ResultTypes.TooFar };
			}

			return result;
		}

		public Result Heal( Creep creep )
		{
			throw new NotImplementedException();
		}

		public Result Move( Directions direction )
		{
			throw new NotImplementedException();
		}

		public Result Move( Route route )
		{
			var result = new Result() { TypeID = ResultTypes.EmptyPath };
			var nextPosition = route._path.LastOrDefault();

			if ( nextPosition != null )
			{
				Pos = nextPosition;
				route._path.Remove( nextPosition );
				result = new Result() { TypeID = ResultTypes.Success };
			}

			return result;
		}

		public Result MoveTo( int x, int y, MoveOptions options = null )
		{
			return MoveTo( Room.GetPositionAt( x, y ), options );
		}
		public Result MoveTo( RoomObject target, MoveOptions options = null )
		{
			return MoveTo( target.Pos, options );
		}
		public Result MoveTo( RoomPosition pos, MoveOptions options = null )
		{
			return Game.AddAction( new CreepMoveTo( this, pos, options ) );
		}

		public bool NotifyWhenAttacked { get; set; }

		public Result Pickup( GameObject target )
		{
			throw new NotImplementedException();
		}

		public Result RangedAttack( RoomObject target )
		{
			throw new NotImplementedException();
		}

		public Result RangedMassAttack()
		{
			throw new NotImplementedException();
		}

		public Result RangedHeal( Creep creep )
		{
			throw new NotImplementedException();
		}

		public Result Repair( Structure structure )
		{
			throw new NotImplementedException();
		}

		// what is this for?: public Result ReserveController( StructureController controller )

		public Result Say( string message, bool isPublic = false )
		{
			throw new NotImplementedException();
		}

		public Result SignController( StructureController controller, string msg = null )
		{
			throw new NotImplementedException();
		}

		public Result Suicide()
		{
			throw new NotImplementedException();
		}

		public Result Transfer( RoomObject target, ResourceTypes type = ResourceTypes.Energy, int amount = 0 )
		{
			var result = new Result { TypeID = ResultTypes.UnknownError };

			if ( target != null )
			{
				int range = Pos.GetRangeTo( target.Pos );

				if ( range <= 1 )
				{
					if ( typeof( IEnergySource ).IsAssignableFrom( target.GetType() ) )
					{
						result = Game.AddAction( new CreepTransfer( this, (IEnergySource)target ) );
					}
					else
					{
						result = new Result { TypeID = ResultTypes.InvalidTarget };
					}
				}
				else
				{
					result = new Result { TypeID = ResultTypes.TooFar };
				}
			}
			else
			{
				result = new Result { TypeID = ResultTypes.EmptyTarget };
			}

			return result;
		}

		public Result UpgradeController( StructureController controller )
		{
			var result = new Result { TypeID = ResultTypes.UnknownError };

			if ( controller != null )
			{
				int range = Pos.GetRangeTo( controller.Pos );

				if ( range <= 1 )
				{
					if ( typeof( StructureController ).IsAssignableFrom( controller.GetType() ) )
					{
						result = Game.AddAction( new CreepUpgrade( this, controller ) );
					}
					else
					{
						result = new Result { TypeID = ResultTypes.InvalidTarget };
					}
				}
				else
				{
					result = new Result { TypeID = ResultTypes.TooFar };
				}
			}
			else
			{
				result = new Result { TypeID = ResultTypes.EmptyTarget };
			}

			return result;
		}

		public Result Withdraw( Structure target, ResourceTypes type, int amount = 0 )
		{
			throw new NotImplementedException();
		}

		internal override void UpdateTick()
		{
			base.UpdateTick();

			TicksToLive--;

			if ( TicksToLive <= 0 )
			{
				log.InfoFormat( "Creep {0} ran out of time", this );

				Suicide();
			}
		}
	}

	public class MoveOptions
	{
		public int PathCacheTicks { get; set; }
		public bool SerializeMemory { get; set; }
		public bool NoPathFinding { get; set; }
		// TODO: public LineStyle PathStyle { get; set; }

		public MoveOptions()
		{
			PathCacheTicks = 5;
			SerializeMemory = true;
			NoPathFinding = false;
		}
	}
}
