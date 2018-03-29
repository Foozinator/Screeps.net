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
	public partial class StringDialog : Form
	{
		public StringDialog()
		{
			InitializeComponent();
		}

		public string StringResult
		{
			get { return InputString.Text; }
			set { InputString.Text = value; }
		}

		private void Cancel_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OK_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
