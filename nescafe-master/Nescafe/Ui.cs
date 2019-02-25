using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Kinect;
using System.IO;

namespace Nescafe
{
    public class Ui : Form
    {
        Bitmap _frame;
        Console _console;

        Thread _nesThread;

        Graphics g;
        private TCPClient client;
        private TCPSetting tcpSetting;
        private int myId, SId, Port;
        private string IpAddress;
        private ButtonImg[] buttonsVirtual;
        private VirtualHand rightHand;
        private VirtualHand leftHand;
        //A B Start Select Up Down Left Right
        private int numButtonImage = 8;

        private KeyStates keyStates;

        private KinectSensor sensor;
        public Ui()
        {
            Text = "NEScafé";
            Size = new Size(512, 480);
            FormBorderStyle = FormBorderStyle.FixedSingle;

            g = CreateGraphics();
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            initTCP();
            CenterToScreen();
            InitMenus();
            initButtonVirtual();
            initVirtualHand();
            this._console = new Console();
            _console.DrawAction = Draw;

            _frame = new Bitmap(256, 240, PixelFormat.Format8bppIndexed);
            InitPalette();
            initKinect();

            keyStates = new KeyStates();

            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);

            _nesThread = new Thread(new ThreadStart(StartNes));
            _nesThread.IsBackground = true;

            
        }


        void initKinect()
        {
            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                MessageBox.Show("No kinect");
            }


            //Set to Seated Mode
            this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
        }


        void SensorSkeletonFrameReady(object sensor, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];
            using(SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            if(skeletons.Length != 0)
            {
                foreach(Skeleton skel in skeletons)
                {
                    if(skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        foreach(Joint joint in skel.Joints)
                        {
                            if(joint.JointType == JointType.HandRight)
                            {
                                if(joint.TrackingState == JointTrackingState.Tracked)
                                {
                                    DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, DepthImageFormat.Resolution320x240Fps30);
                                    //Transform point 320*240 to fit the screen
                                    this.rightHand.UpdatePos(new Point((int)depthPoint.X * Size.Width / 320, (int)depthPoint.Y * Size.Height / 240));
                                    //this.rightHandPos = new Point(depthPoint.X, depthPoint.Y);
                                }
                                else
                                {
                                    this.rightHand.UpdatePos(new Point(-1, -1));
                                }
                            }
                            if(joint.JointType == JointType.HandLeft)
                            {
                                if(joint.TrackingState == JointTrackingState.Tracked)
                                {
                                    DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, DepthImageFormat.Resolution320x240Fps30);
                                    //Transform point 320*240 to 256*240
                                    this.leftHand.UpdatePos(new Point((int)depthPoint.X * Size.Width / 320, (int)depthPoint.Y * Size.Height / 240));
                                    //this.leftHandPos = new Point(depthPoint.X, depthPoint.Y);
                                }
                                else
                                {
                                    this.leftHand.UpdatePos(new Point(-1, -1));
                                }
                            }
                            {

                            }
                        }
                    }
                }
            }
            

            
        }

        void initButtonVirtual()
        {
            this.buttonsVirtual = new ButtonImg[numButtonImage];
            buttonsVirtual[0] = new ButtonImg(Controller.Button.Start, new Point((int)(Size.Width/10+Size.Width/3*0.5), (int)(Size.Height/10+Size.Height/4)), (int)Size.Height/15, Color.Black);
            buttonsVirtual[1] = new ButtonImg(Controller.Button.Select, new Point((int)(Size.Width/10+Size.Width/3*0.8), (int)(Size.Height/10+Size.Height/4*0.35)), (int)Size.Height/15, Color.Red);
            buttonsVirtual[2] = new ButtonImg(Controller.Button.Up, new Point((int)(Size.Width/10+Size.Width/3*1.80), (int)(Size.Height/10+Size.Height/4*0.35)), (int)Size.Height/15, Color.White);
            buttonsVirtual[3] = new ButtonImg(Controller.Button.A, new Point((int)(Size.Width/10+Size.Width/3*0.8), (int)(Size.Height/10+Size.Height/4*1.5)), (int)Size.Height/15, Color.Orange);
            buttonsVirtual[4] = new ButtonImg(Controller.Button.B, new Point((int)(Size.Width/10+Size.Width/3), (int)(Size.Height/10+Size.Height/4)), (int)Size.Height/15, Color.Blue);
            buttonsVirtual[5] = new ButtonImg(Controller.Button.Left, new Point((int)(Size.Width/10+Size.Width/3*1.5), (int)(Size.Height/10+Size.Height/4)), (int)Size.Height/15, Color.White);
            buttonsVirtual[6] = new ButtonImg(Controller.Button.Right, new Point((int)(Size.Width/10+Size.Width/3*2.1), (int)(Size.Height/10+Size.Height/4)), (int)Size.Height/15, Color.White);
            buttonsVirtual[7] = new ButtonImg(Controller.Button.Down, new Point((int)(Size.Width/10+Size.Width/3*1.80), (int)(Size.Height/10+Size.Height/4*1.5)), (int)Size.Height/15, Color.White);
        }

        void initVirtualHand()
        {
            this.rightHand = new VirtualHand();
            this.leftHand = new VirtualHand();
        }
        void initTCP()
        {
            myId = 2;
            SId = 1;
            Port = 50003;
            IpAddress = "loopback";
        }

        void drawHandVirtuel()
        {
            SolidBrush brush;
            Rectangle rect;
            if(rightHand.GetPos().x == -1 && rightHand.GetPos().y == -1)
            {

            }
            else
            {
                brush = new SolidBrush(Color.Yellow);
                rect = new Rectangle(rightHand.GetPos().x,rightHand.GetPos().y,40,40);
                g.FillEllipse(brush, rect);
            }

            if (leftHand.GetPos().x == -1 && leftHand.GetPos().y == -1)
            {

            }
            else
            {
                brush = new SolidBrush(Color.YellowGreen);
                rect = new Rectangle(leftHand.GetPos().x,leftHand.GetPos().y,40,40);
                g.FillEllipse(brush, rect);
            }
        }

        void drawButtonVirtual()
        {
            foreach(ButtonImg button in buttonsVirtual)
            {
                SolidBrush brush = new SolidBrush(button.color);
                Rectangle rect = new Rectangle(button.center.x - button.radius, button.center.y - button.radius, button.radius * 2, button.radius * 2);
                g.FillEllipse(brush, rect);
            }
        }
        void StopConsole()
        {
            _console.Stop = true;

            if (_nesThread.ThreadState == ThreadState.Running)
            {
                _nesThread.Join();
            }
        }

        void StartConsole()
        {
            _nesThread = new Thread(new ThreadStart(StartNes));
            _nesThread.IsBackground = true;
            _nesThread.Start();
        }

        void LoadCartridge(object sender, EventArgs e)
        {
            StopConsole();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "NES ROMs | *.nes";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_console.LoadCartridge(openFileDialog.FileName))
                {
                    Text = "NEScafé - " + openFileDialog.SafeFileName;
                    StartConsole();
                }
                else
                {
                    MessageBox.Show("Could not load ROM, see standard output for details");
                }
            }
        }

        void LaunchGitHubLink(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.github.com/GunshipPenguin/nescafe");
        }

        void InitMenus()
        {
            MenuStrip ms = new MenuStrip();

            // File menu
            var fileMenu = new ToolStripMenuItem("File");

            var fileLoadMenu = new ToolStripMenuItem("Load ROM", null, new EventHandler(LoadCartridge));
            fileMenu.DropDownItems.Add(fileLoadMenu);

            var screenshotMenu = new ToolStripMenuItem("Take Screenshot", null, new EventHandler(TakeScreenshot));
            fileMenu.DropDownItems.Add(screenshotMenu);

            ms.Items.Add(fileMenu);

            // Help menu
            var helpMenu = new ToolStripMenuItem("Help");

            var helpGithubMenu = new ToolStripMenuItem("GitHub", null, new EventHandler(LaunchGitHubLink));
            helpMenu.DropDownItems.Add(helpGithubMenu);

            ms.Items.Add(helpMenu);

            //server TCP 
            var serverMenu = new ToolStripMenuItem("TCP");
            var startClient = new ToolStripMenuItem("Start", null, new EventHandler(StartTCP));
            var stopClient = new ToolStripMenuItem("Stop", null, new EventHandler(StopTCP));
            var setClient = new ToolStripMenuItem("setting", null, new EventHandler(SetTCP));
            serverMenu.DropDownItems.Add(startClient);
            serverMenu.DropDownItems.Add(stopClient);
            serverMenu.DropDownItems.Add(setClient);
            ms.Items.Add(serverMenu);

            Controls.Add(ms);
        }

        void TakeScreenshot(object sender, EventArgs e)
        {
            String filename = "screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            _frame.Save(filename);
        }

        void StartNes()
        {
            _console.Start();
        }

        void StartTCP(object sender, EventArgs e)
        {
            client = new TCPClient(_console,myId,SId,IpAddress,Port);
        }
        void StopTCP(object sender, EventArgs e)
        {
            if(client != null)
            {
                client.CloseClient();
            }
        }
        void SetTCP(object sender, EventArgs e)
        {
            tcpSetting = new TCPSetting(this,myId, SId, IpAddress, Port);
            tcpSetting.ShowDialog(this);
        }
        public void setTcpParam(int Mid, int slaveId,string ip, int p)
        {
            myId = Mid;
            SId = slaveId;
            IpAddress = ip;
            Port = p;
        }
        void InitPalette()
        {
            ColorPalette palette = _frame.Palette;
            palette.Entries[0x0] = Color.FromArgb(84, 84, 84);
            palette.Entries[0x1] = Color.FromArgb(0, 30, 116);
            palette.Entries[0x2] = Color.FromArgb(8, 16, 144);
            palette.Entries[0x3] = Color.FromArgb(48, 0, 136);
            palette.Entries[0x4] = Color.FromArgb(68, 0, 100);
            palette.Entries[0x5] = Color.FromArgb(92, 0, 48);
            palette.Entries[0x6] = Color.FromArgb(84, 4, 0);
            palette.Entries[0x7] = Color.FromArgb(60, 24, 0);
            palette.Entries[0x8] = Color.FromArgb(32, 42, 0);
            palette.Entries[0x9] = Color.FromArgb(8, 58, 0);
            palette.Entries[0xa] = Color.FromArgb(0, 64, 0);
            palette.Entries[0xb] = Color.FromArgb(0, 60, 0);
            palette.Entries[0xc] = Color.FromArgb(0, 50, 60);
            palette.Entries[0xd] = Color.FromArgb(0, 0, 0);
            palette.Entries[0xe] = Color.FromArgb(0, 0, 0);
            palette.Entries[0xf] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x10] = Color.FromArgb(152, 150, 152);
            palette.Entries[0x11] = Color.FromArgb(8, 76, 196);
            palette.Entries[0x12] = Color.FromArgb(48, 50, 236);
            palette.Entries[0x13] = Color.FromArgb(92, 30, 228);
            palette.Entries[0x14] = Color.FromArgb(136, 20, 176);
            palette.Entries[0x15] = Color.FromArgb(160, 20, 100);
            palette.Entries[0x16] = Color.FromArgb(152, 34, 32);
            palette.Entries[0x17] = Color.FromArgb(120, 60, 0);
            palette.Entries[0x18] = Color.FromArgb(84, 90, 0);
            palette.Entries[0x19] = Color.FromArgb(40, 114, 0);
            palette.Entries[0x1a] = Color.FromArgb(8, 124, 0);
            palette.Entries[0x1b] = Color.FromArgb(0, 118, 40);
            palette.Entries[0x1c] = Color.FromArgb(0, 102, 120);
            palette.Entries[0x1d] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x1e] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x1f] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x20] = Color.FromArgb(236, 238, 236);
            palette.Entries[0x21] = Color.FromArgb(76, 154, 236);
            palette.Entries[0x22] = Color.FromArgb(120, 124, 236);
            palette.Entries[0x23] = Color.FromArgb(176, 98, 236);
            palette.Entries[0x24] = Color.FromArgb(228, 84, 236);
            palette.Entries[0x25] = Color.FromArgb(236, 88, 180);
            palette.Entries[0x26] = Color.FromArgb(236, 106, 100);
            palette.Entries[0x27] = Color.FromArgb(212, 136, 32);
            palette.Entries[0x28] = Color.FromArgb(160, 170, 0);
            palette.Entries[0x29] = Color.FromArgb(116, 196, 0);
            palette.Entries[0x2a] = Color.FromArgb(76, 208, 32);
            palette.Entries[0x2b] = Color.FromArgb(56, 204, 108);
            palette.Entries[0x2c] = Color.FromArgb(56, 180, 204);
            palette.Entries[0x2d] = Color.FromArgb(60, 60, 60);
            palette.Entries[0x2e] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x2f] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x30] = Color.FromArgb(236, 238, 236);
            palette.Entries[0x31] = Color.FromArgb(168, 204, 236);
            palette.Entries[0x32] = Color.FromArgb(188, 188, 236);
            palette.Entries[0x33] = Color.FromArgb(212, 178, 236);
            palette.Entries[0x34] = Color.FromArgb(236, 174, 236);
            palette.Entries[0x35] = Color.FromArgb(236, 174, 212);
            palette.Entries[0x36] = Color.FromArgb(236, 180, 176);
            palette.Entries[0x37] = Color.FromArgb(228, 196, 144);
            palette.Entries[0x38] = Color.FromArgb(204, 210, 120);
            palette.Entries[0x39] = Color.FromArgb(180, 222, 120);
            palette.Entries[0x3a] = Color.FromArgb(168, 226, 144);
            palette.Entries[0x3b] = Color.FromArgb(152, 226, 180);
            palette.Entries[0x3c] = Color.FromArgb(160, 214, 228);
            palette.Entries[0x3d] = Color.FromArgb(160, 162, 160);
            palette.Entries[0x3e] = Color.FromArgb(0, 0, 0);
            palette.Entries[0x3f] = Color.FromArgb(0, 0, 0);

            _frame.Palette = palette;
        }

        unsafe void Draw(byte[] screen)
        {
            BitmapData _frameData = _frame.LockBits(new Rectangle(0, 0, 256, 240), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            byte* ptr = (byte*)_frameData.Scan0;
            for (int i = 0; i < 256 * 240; i++)
            {
                ptr[i] = screen[i];
            }
            _frame.UnlockBits(_frameData);

            g.DrawImage(_frame, 0, 0, Size.Width, Size.Height);
            drawButtonVirtual();
            drawHandVirtuel();



            //judge if the virtual buttons are touched
            VirtualHandControl();

            // send BitMap _frame to Unity
            if(client != null) client.SendBitmap(_frame);



            //System.Console.WriteLine("left hand x : " + leftHand.GetPos().x + ", y : " + leftHand.GetPos().y);
            //System.Console.WriteLine("right hand x : " + rightHand.GetPos().x + ", y : " + rightHand.GetPos().y);
        }


        void VirtualHandControl()
        {
            Controller.Button rightTouched = this.rightHand.TouchButton(this.buttonsVirtual);
            Controller.Button leftTouched = this.leftHand.TouchButton(this.buttonsVirtual);
            if(keyStates.A == false)
            {
                if(rightTouched == Controller.Button.A || leftTouched == Controller.Button.A)
                {
                    _console.Controller.setButtonState(Controller.Button.A, true);
                    keyStates.A = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.A && leftTouched != Controller.Button.A)
                {
                    _console.Controller.setButtonState(Controller.Button.A, false);
                    keyStates.A = false;
                }
            }

            if(keyStates.B == false)
            {
                if(rightTouched == Controller.Button.B || leftTouched == Controller.Button.B)
                {
                    _console.Controller.setButtonState(Controller.Button.B, true);
                    keyStates.B = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.B && leftTouched != Controller.Button.B)
                {
                    _console.Controller.setButtonState(Controller.Button.B, false);
                    keyStates.B = false;
                }
            }


            if(keyStates.Start == false)
            {
                if(rightTouched == Controller.Button.Start || leftTouched == Controller.Button.Start)
                {
                    _console.Controller.setButtonState(Controller.Button.Start, true);
                    keyStates.Start = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Start && leftTouched != Controller.Button.Start)
                {
                    _console.Controller.setButtonState(Controller.Button.Start, false);
                    keyStates.Start = false;
                }
            }


            if(keyStates.Selete == false)
            {
                if(rightTouched == Controller.Button.Select || leftTouched == Controller.Button.Select)
                {
                    _console.Controller.setButtonState(Controller.Button.Select, true);
                    keyStates.Selete = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Select && leftTouched != Controller.Button.Select)
                {
                    _console.Controller.setButtonState(Controller.Button.Select, false);
                    keyStates.Selete = false;
                }
            }


            if(keyStates.Up == false)
            {
                if(rightTouched == Controller.Button.Up || leftTouched == Controller.Button.Up)
                {
                    _console.Controller.setButtonState(Controller.Button.Up, true);
                    keyStates.Up = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Up && leftTouched != Controller.Button.Up)
                {
                    _console.Controller.setButtonState(Controller.Button.Up, false);
                    keyStates.Up = false;
                }
            }


            if(keyStates.Down == false)
            {
                if(rightTouched == Controller.Button.Down || leftTouched == Controller.Button.Down)
                {
                    _console.Controller.setButtonState(Controller.Button.Down, true);
                    keyStates.Down = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Down && leftTouched != Controller.Button.Down)
                {
                    _console.Controller.setButtonState(Controller.Button.Down, false);
                    keyStates.Down = false;
                }
            }



            if(keyStates.Right == false)
            {
                if(rightTouched == Controller.Button.Right || leftTouched == Controller.Button.Right)
                {
                    _console.Controller.setButtonState(Controller.Button.Right, true);
                    keyStates.Right = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Right && leftTouched != Controller.Button.Right)
                {
                    _console.Controller.setButtonState(Controller.Button.Right, false);
                    keyStates.Right = false;
                }
            }



            if(keyStates.Left == false)
            {
                if(rightTouched == Controller.Button.Left || leftTouched == Controller.Button.Left)
                {
                    _console.Controller.setButtonState(Controller.Button.Left, true);
                    keyStates.Left = true;
                }
            }
            else
            {
                if(rightTouched != Controller.Button.Left && leftTouched != Controller.Button.Left)
                {
                    _console.Controller.setButtonState(Controller.Button.Left, false);
                    keyStates.Left = false;
                }
            }
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Receive Key From Unity
            SetControllerButton(true, e);
        }

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Receive Key From Unity
            SetControllerButton(false, e);
        }

        void SetControllerButton(bool state, KeyEventArgs e)
        {
            System.Console.WriteLine(e.KeyCode+ "is" + state);
            switch (e.KeyCode)
            {
                case Keys.A:
                    _console.Controller.setButtonState(Controller.Button.A, state);
                    break;
                case Keys.Z:
                    _console.Controller.setButtonState(Controller.Button.B, state);
                    break;
                case Keys.Left:
                    _console.Controller.setButtonState(Controller.Button.Left, state);
                    break;
                case Keys.Right:
                    _console.Controller.setButtonState(Controller.Button.Right, state);
                    break;
                case Keys.Up:
                    _console.Controller.setButtonState(Controller.Button.Up, state);
                    break;
                case Keys.Down:
                    _console.Controller.setButtonState(Controller.Button.Down, state);
                    break;
                case Keys.Q:
                    _console.Controller.setButtonState(Controller.Button.Start, state);
                    break;
                case Keys.S:
                    _console.Controller.setButtonState(Controller.Button.Select, state);
                    break;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Ui
            // 
            this.ClientSize = new System.Drawing.Size(1116, 395);
            this.Name = "Ui";
            this.Load += new System.EventHandler(this.Ui_Load);
            this.ResumeLayout(false);

        }

        private void Ui_Load(object sender, EventArgs e)
        {

        }
    }
}
