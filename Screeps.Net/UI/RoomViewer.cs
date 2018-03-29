using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screeps.Net.Client;

namespace Screeps.Net.UI
{
	public abstract class RoomViewer
	{
		protected Client.Room _room;
		protected RoomViewer() { }

		public static RoomViewer Create( Screeps.Net.Client.Room room )
		{
			return new SquareGridRoomViewer() { _room = room };
		}

		public abstract Image RenderBackground( Size size );
		public abstract Image RenderImage( Size size, Point cursor );
	}

	public class SquareGridRoomViewer : RoomViewer
	{
		public override Image RenderBackground( Size size )
		{
			int minLen = Math.Min( size.Width, size.Height );
			Image img = new Bitmap( minLen, minLen );
			var g = Graphics.FromImage( img );
			GraphicsUnit gu = g.PageUnit;

			// default is plains, so start with that
			g.FillRectangle( Brushes.DarkGray, img.GetBounds( ref gu ) );

			if ( _room != null && _room.Terrain.Count > 0 )
			{
				float dx = img.Width / (float)_room.MaxX;
				float dy = img.Height / (float)_room.MaxY;

				// fill in all non-plains parts
				foreach ( var item in _room.Terrain )
				{
					Brush br = Brushes.Purple;

					switch ( item.Type )
					{
					case TerrainTypes.Wall:
						br = Brushes.Black;
						break;

					case TerrainTypes.Swamp:
						br = Brushes.DarkSeaGreen;
						break;

					case TerrainTypes.Exit:
						br = Brushes.DimGray;
						break;

					case TerrainTypes.Plain:
					default:
						br = Brushes.DarkGray;
						break;
					}

					var rect = new RectangleF( ( item.Pos.X - 1 ) * dx, ( item.Pos.Y - 1 ) * dy, dx, dy );
					g.FillRectangle( br, rect );
				}
			}
			else
			{
				g.DrawRectangle( Pens.Red, new Rectangle( new Point( 0, 0 ), new Size( img.Size.Width - 1, img.Size.Height - 1 ) ) );
			}

			return img;
		}

		public override Image RenderImage( Size size, Point cursor )
		{
			int minLen = Math.Min( size.Width, size.Height );
			Image img = new Bitmap( minLen, minLen );
			var g = Graphics.FromImage( img );
			GraphicsUnit gu = g.PageUnit;

			if ( _room != null && _room.Terrain.Count > 0 )
			{
				float dx = img.Width / (float)_room.MaxX;
				float dy = img.Height / (float)_room.MaxY;

				foreach ( var obj in _room.Objects )
				{
					var boundry = new RectangleF( ( obj.Pos.X - 1 ) * dx,
						( obj.Pos.Y - 1 ) * dy,
						dx,
						dy );

					switch ( obj.GetType().Name )
					{
					case "Source":
						RenderSource( g, (Client.Source)obj, boundry );
						break;

					case "StructureController":
						RenderController( g, (Client.StructureController)obj, boundry );
						break;

					case "StructureSpawn":
						RenderSpawn( g, (Client.StructureSpawn)obj, boundry );
						break;

					case "Creep":
						RenderCreep( g, (Client.Creep)obj, boundry );
						break;
					}
				}

				if ( cursor != Point.Empty )
				{
					using ( var hlBr = new SolidBrush( Color.FromArgb( 128, Color.White ) ) )
					{
						var rect = new RectangleF( ( cursor.X - 1 ) * dx, ( cursor.Y - 1 ) * dy, dx, dy );
						g.FillRectangle( hlBr, rect );
					}
				}
			}
			//else
			//{
			//	g.DrawRectangle( Pens.Black, new Rectangle( new Point( 1, 1 ), new Size( img.Size.Width - 2, img.Size.Height - 2 ) ) );
			//	//g.DrawString( "Foreground", new Font( FontFamily.GenericSansSerif, 10 ), Brushes.White, new Point( 1, 150 ) );
			//}

			return img;

		}

		private void RenderCreep( Graphics g, Creep creep, RectangleF boundry )
		{
			boundry.Inflate( -3, -3 );
			g.DrawEllipse( Pens.White, boundry );

			// energy storage
			boundry.Inflate( -3, -3 );
			g.FillEllipse( Brushes.DarkBlue, boundry );

			if ( creep.CarryCapacity != 0 && creep.CarryLoad > 0 )
			{
				float nrgPercent = 1 - ( creep.CarryLoad / (float)creep.CarryCapacity );
				float inflatex = ( boundry.Width * ( nrgPercent / 2 ) * -1 );
				float inflatey = ( boundry.Height * ( nrgPercent / 2 ) * -1 );

				if ( inflatex != 0 && inflatey != 0 )
				{
					boundry.Inflate( inflatex, inflatey );
				}

				g.FillEllipse( Brushes.Yellow, boundry );
			}
		}

		private static void RenderController( Graphics g, Client.StructureController rc, RectangleF boundry )
		{
			g.DrawEllipse( Pens.White, boundry );

			boundry.Inflate( -2, -2 );

			// level
		//	g.DrawArc( Pens.LightSalmon, boundry, 0, 45 * rc.Level );

			// progress
			boundry.Inflate( -4, -4 );
			g.FillEllipse( Brushes.DarkBlue, boundry );

			if ( rc.Level > 0 )
			{
				g.DrawString( rc.Level.ToString(), new Font( FontFamily.GenericSansSerif, 10 ), Brushes.White, new Point( (int)boundry.X, (int)boundry.Y ) );
			}

			if ( rc.ProgressTotal != 0 && rc.Progress > 0 )
			{
				float nrgPercent = ( rc.Progress / (float)rc.ProgressTotal );
				float inflatex = ( boundry.Width * ( nrgPercent ) ) - boundry.Width;
				float inflatey = ( boundry.Height * ( nrgPercent ) ) - boundry.Height;

				if ( inflatex != 0 && inflatey != 0 )
				{
					boundry.Inflate( inflatex / 2, inflatey / 2 );
				}

				g.FillEllipse( Brushes.Yellow, boundry );
			}
		}

		private static void RenderSpawn( Graphics g, Client.StructureSpawn spn, RectangleF boundry )
		{
			g.DrawEllipse( Pens.White, boundry );

			// energy storage
			boundry.Inflate( -3, -3 );
			g.FillEllipse( Brushes.DarkBlue, boundry );

			if ( spn.EnergyCapacity != 0 && spn.Energy > 0 )
			{
				float nrgPercent = 1 - ( spn.Energy / (float)spn.EnergyCapacity );
				float inflatex = ( boundry.Width * ( nrgPercent / 2 ) * -1 );
				float inflatey = ( boundry.Height * ( nrgPercent / 2 ) * -1 );

				if ( inflatex != 0 && inflatey != 0 )
				{
					boundry.Inflate( inflatex, inflatey );
				}

				g.FillEllipse( Brushes.Yellow, boundry );
			}
		}

		private static void RenderSource( Graphics g, Client.Source src, RectangleF boundry )
		{
			g.DrawEllipse( Pens.White, boundry );

			// energy storage
			boundry.Inflate( -3, -3 );
			g.FillEllipse( Brushes.DarkBlue, boundry );

			if ( src.EnergyCapacity != 0 && src.Energy > 0 )
			{
				float nrgPercent = 1 - ( src.Energy / (float)src.EnergyCapacity );
				float inflatex = ( boundry.Width * ( nrgPercent / 2 ) * -1 );
				float inflatey = ( boundry.Height * ( nrgPercent / 2 ) * -1 );

				if ( inflatex != 0 && inflatey != 0 )
				{
					boundry.Inflate( inflatex, inflatey );
				}

				g.FillEllipse( Brushes.Yellow, boundry );
			}
		}
	}
}
