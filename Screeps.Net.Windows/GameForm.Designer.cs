namespace Screeps.Net.Windows
{
	partial class GameForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.GameSplit = new System.Windows.Forms.SplitContainer();
			this.RoomList = new System.Windows.Forms.ListView();
			this.GameStatus = new System.Windows.Forms.StatusStrip();
			this.StatusText = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.GameSplit)).BeginInit();
			this.GameSplit.Panel1.SuspendLayout();
			this.GameSplit.Panel2.SuspendLayout();
			this.GameSplit.SuspendLayout();
			this.GameStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(690, 123);
			this.textBox1.TabIndex = 0;
			// 
			// GameSplit
			// 
			this.GameSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GameSplit.Location = new System.Drawing.Point(0, 0);
			this.GameSplit.Name = "GameSplit";
			this.GameSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// GameSplit.Panel1
			// 
			this.GameSplit.Panel1.Controls.Add(this.RoomList);
			// 
			// GameSplit.Panel2
			// 
			this.GameSplit.Panel2.Controls.Add(this.textBox1);
			this.GameSplit.Size = new System.Drawing.Size(690, 379);
			this.GameSplit.SplitterDistance = 252;
			this.GameSplit.TabIndex = 1;
			// 
			// RoomList
			// 
			this.RoomList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.RoomList.Location = new System.Drawing.Point(12, 12);
			this.RoomList.Name = "RoomList";
			this.RoomList.Size = new System.Drawing.Size(182, 226);
			this.RoomList.TabIndex = 0;
			this.RoomList.UseCompatibleStateImageBehavior = false;
			this.RoomList.View = System.Windows.Forms.View.List;
			// 
			// GameStatus
			// 
			this.GameStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.GameStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusText});
			this.GameStatus.Location = new System.Drawing.Point(0, 355);
			this.GameStatus.Name = "GameStatus";
			this.GameStatus.Size = new System.Drawing.Size(690, 24);
			this.GameStatus.TabIndex = 2;
			this.GameStatus.Text = "statusStrip1";
			// 
			// StatusText
			// 
			this.StatusText.Name = "StatusText";
			this.StatusText.Size = new System.Drawing.Size(0, 19);
			// 
			// GameForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(690, 379);
			this.Controls.Add(this.GameStatus);
			this.Controls.Add(this.GameSplit);
			this.Name = "GameForm";
			this.Text = "Screeps.Net";
			this.GameSplit.Panel1.ResumeLayout(false);
			this.GameSplit.Panel2.ResumeLayout(false);
			this.GameSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.GameSplit)).EndInit();
			this.GameSplit.ResumeLayout(false);
			this.GameStatus.ResumeLayout(false);
			this.GameStatus.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.SplitContainer GameSplit;
		private System.Windows.Forms.StatusStrip GameStatus;
		private System.Windows.Forms.ListView RoomList;
		private System.Windows.Forms.ToolStripStatusLabel StatusText;
	}
}