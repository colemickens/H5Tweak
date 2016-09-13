using System;
using System.Windows.Forms;
using System.Linq;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Helpers;
using System.Diagnostics;

namespace H5Tweak
{
    public partial class TweakUI : Form
    {
        int fov;
        int fps;

        public TweakUI()
        {
            this.fov = 78;
            this.fps = 60;
            InitializeComponent();
            this.tbFPS.Value = this.fps;
            this.tbFOV.Value = this.fov;
            this.lblFOV.Text = "FOV: " + this.tbFOV.Value.ToString();
            this.lblFPS.Text = "FPS: " + this.tbFPS.Value.ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.lblFOV.Text = "FOV: " + this.tbFOV.Value.ToString();
            this.fov = tbFOV.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.lblFPS.Text = "FPS: " + this.tbFPS.Value.ToString();
            int fps = 1000000 / Convert.ToInt16(this.tbFPS.Value);

            this.fps = fps;
        }

        private void TweakUI_Load(object sender, EventArgs e)
        {
            Timer refreshTimer = new System.Windows.Forms.Timer();

            refreshTimer.Interval = 1000;
            refreshTimer.Tick += new EventHandler(this.Refresh);
            refreshTimer.Start();
        }

        private void Refresh(Object myObject, EventArgs myEventArgs)
        {
            Poker poker;
            if (!Poker.TryGetHaloPoker(out poker))
            {
                Console.WriteLine("No valid Halo process.");
                return;
            }

            poker.SetFOV(this.fov);
            poker.SetFPS(this.fps);
        }
    }
}