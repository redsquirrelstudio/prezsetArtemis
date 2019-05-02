using System;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Preztrack
{
    class FaceDetector
    {
        public Image<Bgr, byte> imgInput;
        Bitmap original;

        public Image<Bgr, byte> DetectFace(ref bool faceDetected, ref int count, int quality, ref Image<Bgr, byte> tracked, Bitmap overlay)
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../data/haarcascade_frontalface_default.xml");
                CascadeClassifier classifier = new CascadeClassifier(facePath);
                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                Rectangle[] detectedFaces = classifier.DetectMultiScale(imgGray, 1.1, quality);
                original = imgInput.ToBitmap();
                Bitmap finalImage = new Bitmap(original.Width, original.Height);
                var graphics = Graphics.FromImage(finalImage);
                graphics.DrawImage(original, 0, 0);
                foreach (var face in detectedFaces)
                {
                    graphics.DrawImage(overlay, face);
                    tracked.Draw(face, new Bgr(0, 0, 255), 2);
                    faceDetected = true;
                    count++;
                }
                imgInput = new Image<Bgr, byte>(finalImage);
                return imgInput;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         }
    }
}