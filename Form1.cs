using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;


namespace Preztrack
{
    public partial class Form1 : Form
    {
        public bool faceDetected = false;
        public int count = 0;
        public bool on = false;
        public int quality;
        public ImageBox viewer;
        public ImageBox viewer2;
        public VideoCapture capture = new VideoCapture();
        public Stopwatch stopwatch = new Stopwatch();
        public Bitmap[] overlayList = new Bitmap[8];
        public Bitmap overlay;
        public DateTime startTime;
        public int slide;

        public Form1()
        {
            InitializeComponent();
            TextBox_Log.Text += "======================================================\n       Artemis Ver:1.2 | Loaded O.K | ©RedSquirrelStudio 2019\n======================================================";
            ComboBox_mode.Text = "Off";
            TextBox_Source.Text = "None";
            Label_Count.Visible = false;
            Button_Quality.Text = "Medium";
            quality = 4;
            BuildOverlayList();
            HideUI();
        }

        private void BuildOverlayList()
        {
            overlayList[0] = (Bitmap)Image.FromFile(@"../../data/overlays/skeleton.jpg");
            overlayList[1] = (Bitmap)Image.FromFile(@"../../data/overlays/person.png");
            overlayList[2] = (Bitmap)Image.FromFile(@"../../data/overlays/dog.png");
            overlayList[3] = (Bitmap)Image.FromFile(@"../../data/overlays/cat.png");
            overlayList[4] = (Bitmap)Image.FromFile(@"../../data/overlays/alien.png");
            overlayList[5] = (Bitmap)Image.FromFile(@"../../data/overlays/empty.png");
            overlayList[6] = (Bitmap)Image.FromFile(@"../../data/overlays/off.png");
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            stopwatch.Start();
            on = true;
            StartCapture();
            ComboBox_mode.Text = "Skeleton";
            ShowUI();
        }

        private void Button_stop_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            on = false;
            StopCapture(capture);
            HideUI();
        }

        private void StartCapture()
        {
            overlay = overlayList[0];
            viewer = imageBox1;
            viewer2 = imageBox2;
            Image<Bgr, byte> tracked;
            TextBox_Log.Text += String.Format("\n{0}: Capture Started", DateTime.Now.ToString("HH:mm:ss"));
            Scrolldown();
            FaceDetector fd = new FaceDetector();
            TextBox_Source.Text = capture.CaptureSource.ToString();
            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {
                try
                {
                    if (on)
                    {
                        fd.imgInput = capture.QueryFrame().ToImage<Bgr, byte>();
                        tracked = capture.QueryFrame().ToImage<Bgr, byte>();
                        viewer.Image = fd.DetectFace(ref faceDetected, ref count, quality, ref tracked, overlay);
                        viewer2.Image = tracked;
                        if (faceDetected)
                        {
                            TextBox_Log.Text += String.Format("\n{0}: Face Detected", DateTime.Now.ToString("HH:mm:ss"));
                            Scrolldown();
                            faceDetected = false;
                            Label_Count.Visible = true;
                            if (count == 1)
                            {
                                Label_Count.Text = String.Format("{0} Face detected.", count);
                            }
                            else
                            {
                                Label_Count.Text = String.Format("{0} Faces detected.", count);
                            }
                            count = 0;
                        }
                        else
                        {
                            count = 0;
                            Label_Count.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextBox_Log.Text += ex.Message;
                    Scrolldown();
                    throw new Exception(ex.Message);
                }

            });
        }

        private void StopCapture(VideoCapture capture)
        {
            capture.Stop();
            imageBox1.Image = null;
            imageBox2.Image = null;
            TextBox_Source.Text = "None";
            ComboBox_mode.Text = "Off";
            Label_Count.Visible = false;
            TextBox_Log.Text += String.Format("\n{0}: Capture Stopped", DateTime.Now.ToString("HH:mm:ss"));
            TextBox_Log.Text += String.Format("\n{0}: Time Elapsed: {1}", DateTime.Now.ToString("HH:mm:ss"), stopwatch.Elapsed);
            Scrolldown();
        }

        private void Scrolldown()
        {
            TextBox_Log.SelectionStart = TextBox_Log.Text.Length;
            TextBox_Log.ScrollToCaret();
        }

        private void Button_Quality_Click(object sender, EventArgs e)
        {
            if (Button_Quality.Text == "Medium")
            {
                Button_Quality.Text = "High";
                quality = 3;
                TextBox_Log.Text += String.Format("\n{0}: Quality switched to high", DateTime.Now.ToString("HH:mm:ss"));
            }
            else
            {
                Button_Quality.Text = "Medium";
                quality = 4;
                TextBox_Log.Text += String.Format("\n{0}: Quality switched to medium", DateTime.Now.ToString("HH:mm:ss"));
            }
            Scrolldown();
        }


        private void ComboBox_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ComboBox_mode.Text)
            {
                case "Skeletons":
                    overlay = overlayList[0];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Skeleton", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "People":
                    overlay = overlayList[1];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: People", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "Dogs":
                    overlay = overlayList[2];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Dogs", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "Cats":
                    overlay = overlayList[3];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Cat", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "Aliens":
                    overlay = overlayList[4];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Alien", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "Empty":
                    overlay = overlayList[5];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Empty", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                case "Off":
                    overlay = overlayList[6];
                    TextBox_Log.Text += String.Format("\n{0}: Overlay switched to option: Off", DateTime.Now.ToString("HH:mm:ss"));
                    Scrolldown();
                    break;
                default:
                    break;
            }
        }

        private void Button_Apply_Click(object sender, EventArgs e)
        {
            TextBox_Notes.Text = TextBox_Input.Text;
            TextBox_Log.Text += String.Format("\n{0}: Notes added.", DateTime.Now.ToString("HH:mm:ss"));
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            TextBox_Notes.Text = "";
            TextBox_Input.Text = "";
            TextBox_Log.Text += String.Format("\n{0}: Notes cleared.", DateTime.Now.ToString("HH:mm:ss"));
        }

        private void Button_StartTime_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            timer1.Start();
            TextBox_Log.Text += String.Format("\n{0}: Stopwatch started.", DateTime.Now.ToString("HH:mm:ss"));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            string text = "";
            int tenths = elapsed.Milliseconds / 100;

            text +=
                elapsed.Hours.ToString("00") + ":" +
                elapsed.Minutes.ToString("00") + ":" +
                elapsed.Seconds.ToString("00") + "." +
                tenths.ToString("0");

            Label_Timer.Text = text;
        }

        private void Button_StopTime_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            TextBox_Log.Text += String.Format("\n{0}: Stopwatch Stopped, Time elapsed: {1}", DateTime.Now.ToString("HH:mm:ss"), Label_Timer.Text);
        }

        private void HideUI()
        {
            StatusImage.Visible = false;
            LeftImage.Visible = false;
            RightImage.Visible = false;
            LogoImage.Visible = false;
            TextBox_Notes.Visible = false;
            Label_Timer.Visible = false;
            Label_Slides.Visible = false;
        }

        private void ShowUI()
        {
            StatusImage.Visible = true;
            LeftImage.Visible = true;
            RightImage.Visible = true;
            LogoImage.Visible = true;
            TextBox_Notes.Visible = true;
            Label_Timer.Visible = true;
            Label_Slides.Visible = true;
            Label_Slides.Text = "Slide 1/10";
        }

        private void RightImage_Click(object sender, EventArgs e)
        {
            if (slide != 10)
            {
                slide++;
                Label_Slides.Text = String.Format("Slide {0}/10", slide);
            }
        }

        private void LeftImage_Click(object sender, EventArgs e)
        {
            if (slide != 1)
            {
                slide--;
                Label_Slides.Text = String.Format("Slide {0}/10", slide);
            }
        }
    }
}
