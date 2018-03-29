namespace Screeps.Net.Windows
{
	partial class ViewRoom
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
			this.RoomImage = new System.Windows.Forms.PictureBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.ViewStatus = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.RoomImage)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// RoomImage
			// 
			this.RoomImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RoomImage.BackColor = System.Drawing.Color.Black;
			this.RoomImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.RoomImage.Location = new System.Drawing.Point(0, 1);
			this.RoomImage.Name = "RoomImage";
			this.RoomImage.Size = new System.Drawing.Size(723, 367);
			this.RoomImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.RoomImage.TabIndex = 0;
			this.RoomImage.TabStop = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 369);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(722, 24);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// ViewStatus
			// 
			this.ViewStatus.Name = "ViewStatus";
			this.ViewStatus.Size = new System.Drawing.Size(0, 19);
			// 
			// ViewRoom
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722, 393);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.RoomImage);
			this.Name = "ViewRoom";
			this.Text = "Room";
			((System.ComponentModel.ISupportInitialize)(this.RoomImage)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox RoomImage;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel ViewStatus;
	}
}