using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.Logic
{
	internal abstract class Navigator
	{
		public abstract Route GetPath( RoomPosition start, RoomPosition end, MoveOptions options = null );

		protected int GetTerrainCost( TerrainTypes type )
		{
			switch ( type )
			{
			case TerrainTypes.Exit:
			case TerrainTypes.Plain: return 1;

			case TerrainTypes.Swamp: return 5;

			default: return int.MaxValue;
			}
		}
	}

	// TODO: Hex map
	// REF: https://www.redblobgames.com/grids/hexagons/implementation.html
	// REF: https://www.codeproject.com/Articles/14948/Hexagonal-grid-for-games-and-other-projects-Part
	// REF: https://hexgridutilities.codeplex.com/
	// REF: https://github.com/DigitalMachinist/HexGrid
	// REF: https://news.ycombinator.com/item?id=5809724

	internal class SquareMapNavigator : Navigator
	{
		private string Map( List<PathStep> list, int maxX, int maxY )
		{
			StringBuilder map = new StringBuilder();

			for ( int y = 1; y <= maxY; y++ )
			{
				for ( int x = 1; x <= maxX; x++ )
				{
					var visited = list.FirstOrDefault( p => p.X == x && p.Y == y );

					if ( visited != null )
					{
						map.AppendFormat( "{0:d02} ", visited.Cost );
					}
					else
					{
						map.Append( "00 " );
					}
				}

				map.AppendLine();
			}

			return map.ToString();
		}
		public override Route GetPath( RoomPosition start, RoomPosition end, MoveOptions options )
		{
			// TODO: use options
			// TODO: Handle different rooms
			return DijkstraPathTo( start, end );
		}
		private Route DijkstraPathTo( RoomPosition start, RoomPosition dest )
		{
			Route result = null;

			if ( start.Room.ID == dest.Room.ID )
			{
				// build the cost graph of the room
				var vertices = new Dictionary<RoomPosition, Dictionary<RoomPosition, int>>();

				for ( int x = 1; x <= start.Room.MaxX; x++ )
				{
					for ( int y = 1; y <= start.Room.MaxY; y++ )
					{
						var pos = start.Room.GetPositionAt( x, y );
						var adjacent = new Dictionary<RoomPosition, int>();
						vertices.Add( pos, adjacent );

						// get adjacent items
						for ( int dx = -1; dx <= 1; dx++ )
						{
							for ( int dy = -1; dy <= 1; dy++ )
							{
								// origin is not adjacent
								if ( !( dx == 0 && dy == 0 ) )
								{
									var adj = start.Room.GetPositionAt( x + dx, y + dy );

									// make sure we have a valid point
									if ( adj != null )
									{
										var obstructionCount = start.Room.Objects.Count( o => o.Pos.Equals( adj ) && !( o is StructureRoad ) && !( o is StructureRampart ) );

										if ( obstructionCount == 0 || adj.Equals( dest ) )
										{
											double cost = ( dx == 0 || dy == 0 ) ? 1 : 1.4;

											switch ( start.Room.GetTerrainAt( adj ) )
											{
											case TerrainTypes.Plain:
											case TerrainTypes.Exit:
												adjacent.Add( adj, (int)cost );
												break;

											case TerrainTypes.Swamp:
												adjacent.Add( adj, (int)( 5 * cost ) );
												break;

												// walls are NOT adjacent vertices
											}
										}
									}
								}
							}
						}
					}
				}

				result = DijkstraShortestPath( start, dest, vertices );
			}
			// TODO: else start to exit in the right direction

			return result;
		}
		private Route DijkstraShortestPath( RoomPosition start, RoomPosition finish, Dictionary<RoomPosition, Dictionary<RoomPosition, int>> vertices )
		{
			Route result = new Route() { _path = new List<PathStep>() };
			var previous = new Dictionary<RoomPosition, RoomPosition>();
			var distances = new Dictionary<RoomPosition, int>();
			var nodes = new List<RoomPosition>();

			//List<char> path = null;

			foreach ( var vertex in vertices )
			{
				if ( vertex.Key.Equals( start ) )
				{
					distances[ vertex.Key ] = 0;
				}
				else
				{
					distances[ vertex.Key ] = int.MaxValue;
				}

				nodes.Add( vertex.Key );
			}

			while ( nodes.Count != 0 )
			{
				nodes.Sort( ( x, y ) => distances[ x ] - distances[ y ] );

				var smallest = nodes[ 0 ];
				nodes.Remove( smallest );

				if ( smallest.Equals( finish ) )
				{
					//path = new List<RoomPosition>();
					while ( previous.ContainsKey( smallest ) )
					{
						result._path.Add( new PathStep()
						{
							X = smallest.X,
							Y = smallest.Y,
							Room = smallest.Room,
						} );
						smallest = previous[ smallest ];
					}

					break;
				}

				if ( distances[ smallest ] == int.MaxValue )
				{
					break;
				}

				foreach ( var neighbor in vertices[ smallest ] )
				{
					var alt = distances[ smallest ] + neighbor.Value;
					if ( alt < distances[ neighbor.Key ] )
					{
						distances[ neighbor.Key ] = alt;
						previous[ neighbor.Key ] = smallest;
					}
				}
			}

			return result;
		}
		//private Route DijkstraRecurse( RoomPosition start, List<PathStep> visited, RoomPosition dest, int level = 0 )
		//{
		//	Route result = null;

		//	// check to see if an adjacent position is the destination
		//	for ( int dx = -1; dx <= 1; dx++ )
		//	{
		//		for ( int dy = -1; dy <= 1; dy++ )
		//		{
		//			// self is not a valid place
		//			if ( dx != 0 || dy != 0 )
		//			{
		//				var current = start.Room.GetPositionAt( start.X + dx, start.Y + dy );

		//				// make sure we have a valid place on the map
		//				if ( current != null  )
		//				{
		//					// make sure we're not on a wall
		//					if ( start.Room.GetTerrainAt( current ) != TerrainTypes.Wall )
		//					{
		//						// make sure this is unvisited
		//						var test = visited.FirstOrDefault( p => p.Equals( current ) );

		//						if ( test == null || test.Cost > ( level + 1 ) )
		//						{
		//							// this is now visited
		//							if ( test == null )
		//							{
		//								visited.Add( new PathStep()
		//								{
		//									Room = current.Room,
		//									X = current.X,
		//									Y = current.Y,
		//									Cost = level + 1,
		//								} );
		//							}
		//							else
		//							{
		//								test.Cost = level + 1;
		//							}

		//							// check to see if we're done
		//							if ( current.Equals( dest ) )
		//							{
		//								// let's rock!
		//								result = new Route();
		//								result._path.Add( new PathStep()
		//								{
		//									Room = current.Room,
		//									X = current.X,
		//									Y = current.Y,
		//									Cost = GetTerrainCost( current.Room.GetTerrainAt( current ) )
		//								} );
		//							}
		//						}
		//						// else already tested, ignore
		//					}
		//					else
		//					{
		//						visited.Add( new PathStep()
		//						{
		//							Room = current.Room,
		//							X = current.X,
		//							Y = current.Y,
		//							Cost = -1,
		//						} );
		//					}
		//				}
		//				// else out of bounds
		//			}

		//			// if we found a way, stop looking
		//			if ( result != null )
		//			{
		//				break;
		//			}
		//		}

		//		// if we found a way, stop looking
		//		if ( result != null )
		//		{
		//			break;
		//		}
		//	}

		//	// if we haven't found the spot, yet, recurse to look around
		//	if ( result == null )
		//	{
		//		for ( int dx = -2; dx <= 2; dx++ )
		//		{
		//			for ( int dy = -2; dy <= 2; dy++ )
		//			{
		//				// self is not a valid place
		//				if ( dx != 0 || dy != 0 )
		//				{
		//					var current = start.Room.GetPositionAt( start.X + dx, start.Y + dy );

		//					// make sure we have a valid place on the map
		//					if ( current != null && start.Room.GetTerrainAt( current ) != TerrainTypes.Wall )
		//					{
		//						// make sure this is unvisited
		//						var test = visited.FirstOrDefault( p => p.Equals( current ) );

		//						if ( test == null || test.Cost > ( level + 1 ) )
		//						{
		//							// this is now visited
		//							if ( test == null )
		//							{
		//								visited.Add( new PathStep()
		//								{
		//									Room = current.Room,
		//									X = current.X,
		//									Y = current.Y,
		//									Cost = level + 1,
		//								} );
		//							}
		//							else
		//							{
		//								test.Cost = level + 1;
		//							}

		//							// if we're not out of places to try, recurse
		//							result = DijkstraRecurse( current, visited, dest, level + 1 );

		//							// if we found something, we need to retrace back
		//							if ( result != null )
		//							{
		//								result._path.Add( new PathStep()
		//								{
		//									Room = current.Room,
		//									X = current.X,
		//									Y = current.Y,
		//									Cost = GetTerrainCost( current.Room.GetTerrainAt( current ) )
		//								} );
		//							}
		//						}
		//						// else already tested, ignored
		//					}
		//					// else out of bounds
		//				}
		//			}
		//		}
		//	}

		//	return result;
		//}
	}
}
