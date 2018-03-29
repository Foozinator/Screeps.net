using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Screeps.Net.Client;

namespace Screeps.Net.Windows
{
	public partial class GameForm : Form
	{
		public Logic.Shard _game = null;
		public Player _player;
		private List<ViewRoom> _roomViews = new List<ViewRoom>();
		private List<Client.Room> _knownRooms = new List<Client.Room>();

		public GameForm()
		{
			InitializeComponent();

			RoomList.ItemSelectionChanged += RoomList_ItemSelectionChanged;

			_game = Logic.Shard.Create( new Logic.ShardConfiguration()
			{
				ShardType = Client.ShardTypes.Local,
				MapType = Logic.MapTypes.Square,
				Name = "Test",
			} );

			_game.GameEvent += GameEvent;

			Text = string.Format( "Screeps Game: {0}", _game.Configuration.Name );

			var clientGame = _game.Games.FirstOrDefault();

			if ( clientGame != null )
			{
				var rooms = clientGame.Rooms.Where( r => r.IsAvailable );

				if ( rooms != null && rooms.Count() > 0 )
				{
					// populate the list of rooms in the ui
					RoomList.Items.Clear();
					List<ListViewItem> currentRooms = new List<ListViewItem>();

					foreach ( var room in rooms )
					{
						var item = new ListViewItem() { Name = room.Name, Tag = room, Text = room.Name, ToolTipText = room.ID };
						RoomList.Items.Add( item );
						currentRooms.Add( item );
					}

					// Select any new rooms
					foreach ( var newRoom in currentRooms.Where( cr => !_knownRooms.Any( kr => kr.ID == ((Client.Room)cr.Tag).ID ) ) )
					{
						newRoom.Selected = true;
						_knownRooms.Add( (Client.Room)newRoom.Tag );
					}
				}

				_player = clientGame.AddPlayer( "user", typeof( Player1.Controller ) );
			}

			_game.FinishInit( clientGame );
		}

		private void GameEvent( GameObject source, GameEventArgs args )
		{
			if ( InvokeRequired )
			{
				BeginInvoke( new Logic.Shard.GameEventHandler( GameEvent ), new object[] { source, args } );
			}
			else
			{
				GameEventArgs gea = args as GameEventArgs;

				if ( gea != null )
				{
					switch ( gea.Type )
					{
					case GameEventTypes.ChooseSpawnPosition:
						{
							// make the room visible, if it isn't already
							var roomView = ShowRoom( source as Client.Room );
							// make the request
							roomView.GameEvent( source, gea );
						}
						break;

					case GameEventTypes.RoomChanged:
						{
							if ( !string.IsNullOrWhiteSpace( args.Message ) )
							{
								LogMessage( args.Message );
							}

							var view = _roomViews.FirstOrDefault( rv => rv.Room.ID == source.ID );

							if ( view != null )
							{
								view.GameEvent( source, args );
							}
						}
						break;

					case GameEventTypes.GameStep:
						foreach ( var view in _roomViews )
						{
							view.GameEvent( source, args );
						}
						break;
					}
				}
			}
		}

		private void LogMessage( string message )
		{
			if ( InvokeRequired )
			{
				BeginInvoke( Delegate.CreateDelegate( typeof( GameForm ), this, "LogMessage" ), new object[] { message } );
			}
			else
			{
				StatusText.Text = message;
			}
		}

		private ViewRoom ShowRoom( Room room )
		{
			ViewRoom view = null;

			if ( room != null )
			{
				// first, check to see if it's already open
				view = _roomViews.FirstOrDefault( r => r.Room.ID == room.ID );

				// if it isn't, open it up
				if ( view == null )
				{
					view = new ViewRoom() { Room = room, Player = _player };
					_roomViews.Add( view );
					view.Show( this );
				}
			}

			return view;
		}

		private void RoomList_ItemSelectionChanged( object sender, ListViewItemSelectionChangedEventArgs e )
		{
			Client.Room room = e.Item.Tag as Client.Room;

			if ( room != null )
			{
				if ( e.Item.Selected )
				{
					ShowRoom( room );
				}
				else
				{
					var view = _roomViews.FirstOrDefault( r => r.Room.ID == room.ID );

					if ( view != null )
					{
						CloseRoomView( view );
						view.Close();
					}
				}
			}
		}

		internal void CloseRoomView( ViewRoom view, bool deselect = false )
		{
			_roomViews.Remove( view );

			if ( deselect )
			{
				foreach ( ListViewItem item in RoomList.Items )
				{
					if ( ( (Client.Room)item.Tag ).ID == view.Room.ID )
					{
						item.Selected = false;

						break;
					}
				}
			}
		}
	}
}
