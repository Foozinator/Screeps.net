using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screeps.Net.Windows
{
	public partial class ViewRoom : Form
	{
		public Client.Room Room { get; set; }
		public Player Player { get; set; }
		private GameEventTypes _mode = GameEventTypes.None;

		public ViewRoom()
		{
			InitializeComponent();

			RoomImage.MouseMove += RoomImage_MouseMove;
			RoomImage.MouseClick += RoomImage_MouseClick;
		}

		private void RoomImage_MouseClick( object sender, MouseEventArgs e )
		{
			switch ( _mode )
			{
			case GameEventTypes.ChooseSpawnPosition:
				if ( Room != null && Room.Game != null )
				{
					var pt = WindowPositionToMapLocation( e.Location );
					var pos = Room.GetPositionAt( pt );

					string name = RequestStringDialog( "Spawn Name", "My Spawn 1" );

					if ( !string.IsNullOrWhiteSpace( name ) )
					{
						var result = Room.Game.ChooseSpawn( pos, Player, name );

						if ( result.TypeID == ResultTypes.Success )
						{
							Text = Room.Name;
							_mode = GameEventTypes.None;
						}
					}
				}
				break;
			}
		}

		private string RequestStringDialog( string prompt, string initial = null )
		{
			string result = string.Empty;
			var dlg = new StringDialog() { Text = prompt, StringResult = initial, StartPosition = FormStartPosition.CenterParent };

			if ( dlg.ShowDialog( this ) == DialogResult.OK )
			{
				result = dlg.StringResult;
			}

			return result;
		}

		private Point _lastPoint = Point.Empty;
		private void RoomImage_MouseMove( object sender, MouseEventArgs e )
		{
			_lastPoint = WindowPositionToMapLocation( e.Location );

			var viewer = UI.RoomViewer.Create( Room );

			if ( viewer != null )
			{
				RoomImage.Image = viewer.RenderImage( RoomImage.ClientSize, _lastPoint );
			}

			// also, check for objects
			var pos = Room.GetPositionAt( _lastPoint );
			var objects = Room.Objects.Where( o => o.Pos.Equals( pos ) );

			if ( objects.Count() > 0 )
			{
				string list = objects
					.Select( o => o.ToString() )
					.Aggregate( ( a, b ) => string.Format( "{0}, {1}", a, b ) );

				ViewStatus.Text = list;
			}
			else
			{
				ViewStatus.Text = string.Empty;
			}
		}

		protected override void OnResizeEnd( EventArgs e )
		{
			base.OnResizeEnd( e );

			if ( Room != null )
			{
				var viewer = UI.RoomViewer.Create( Room );

				if ( viewer != null )
				{
					RoomImage.BackgroundImage = viewer.RenderBackground( RoomImage.ClientSize );
					RoomImage.Image = viewer.RenderImage( RoomImage.ClientSize, _lastPoint );
				}
			}
		}

		private Point WindowPositionToMapLocation( Point windowPos )
		{
			Point result = Point.Empty;
			// try to figure out the current game position
			//var clientPt = GameImage.PointToClient( e.Location );
			//Console.WriteLine( string.Format( "C({0},{1})", clientPt.X, clientPt.Y ) );
			// test to make sure our image is not null
			if ( RoomImage.BackgroundImage != null )
			{
				// Make sure our control width and height are not 0
				if ( RoomImage.Width != 0 && RoomImage.Height != 0 )
				{
					// This is the one that gets a little tricky. Essentially, need to check 
					// the aspect ratio of the image to the aspect ratio of the control
					// to determine how it is being rendered
					float imageAspect = (float)RoomImage.BackgroundImage.Width / RoomImage.BackgroundImage.Height;
					float controlAspect = (float)RoomImage.Width / RoomImage.Height;
					float newX = windowPos.X;
					float newY = windowPos.Y;
					if ( imageAspect > controlAspect )
					{
						// This means that we are limited by width, 
						// meaning the image fills up the entire control from left to right
						float ratioWidth = (float)RoomImage.BackgroundImage.Width / RoomImage.Width;
						newX *= ratioWidth;
						float scale = (float)RoomImage.Width / RoomImage.BackgroundImage.Width;
						float displayHeight = scale * RoomImage.BackgroundImage.Height;
						float diffHeight = RoomImage.Height - displayHeight;
						diffHeight /= 2;
						newY -= diffHeight;
						newY /= scale;
					}
					else
					{
						// This means that we are limited by height, 
						// meaning the image fills up the entire control from top to bottom
						float ratioHeight = (float)RoomImage.BackgroundImage.Height / RoomImage.Height;
						newY *= ratioHeight;
						float scale = (float)RoomImage.Height / RoomImage.BackgroundImage.Height;
						float displayWidth = scale * RoomImage.BackgroundImage.Width;
						float diffWidth = RoomImage.Width - displayWidth;
						diffWidth /= 2;
						newX -= diffWidth;
						newX /= scale;
					}

					//Console.WriteLine( string.Format( "M({0},{1})", newX, newY ) );

					if ( newX >= 0 && newX < RoomImage.BackgroundImage.Width && newY >= 0 && newY < RoomImage.BackgroundImage.Height )
					{
						// get the room extents to convert from scaled view coordinates to room grid
						float scaleX = RoomImage.BackgroundImage.Width / (float)Room.MaxX;
						float scaleY = RoomImage.BackgroundImage.Height / (float)Room.MaxY;

						int xpos = (int)( newX / scaleX ) + 1;
						int ypos = (int)( newY / scaleY ) + 1;

						result = new Point( xpos, ypos );
					}
				}
			}

			return result;
		}

		protected override void OnShown( EventArgs e )
		{
			base.OnShown( e );

			if ( Room != null && _mode == GameEventTypes.None )
			{
				Text = Room.Name;
			}
		}

		protected override void OnClosing( CancelEventArgs e )
		{
			var gameForm = Owner as GameForm;

			if ( gameForm != null )
			{
				gameForm.CloseRoomView( this, true );
			}

			base.OnClosing( e );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );

			// if we don't have an initial image, make one
			if ( Room != null && RoomImage.BackgroundImage == null )
			{
				var viewer = UI.RoomViewer.Create( Room );

				if ( viewer != null )
				{
					RoomImage.BackgroundImage = viewer.RenderBackground( RoomImage.ClientSize );
					RoomImage.Image = viewer.RenderImage( RoomImage.ClientSize, _lastPoint );
				}
			}
		}

		internal void GameEvent( GameObject source, GameEventArgs args )
		{
			switch ( args.Type )
			{
			case GameEventTypes.ChooseSpawnPosition:
				Text = "Choose your spawn position";
				_mode = args.Type;
				break;

			case GameEventTypes.GameStep:
			case GameEventTypes.RoomChanged:
				if ( Room != null )
				{
					var viewer = UI.RoomViewer.Create( Room );

					if ( viewer != null )
					{
						//RoomImage.BackgroundImage = viewer.RenderBackground( RoomImage.ClientSize );
						RoomImage.Image = viewer.RenderImage( RoomImage.ClientSize, _lastPoint );
					}
				}
				break;
			}
		}
	}
}
