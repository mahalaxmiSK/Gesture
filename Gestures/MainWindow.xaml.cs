using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Animation;
using Microsoft.Ink;
using System;

namespace Gestures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            InkCanvas.DefaultDrawingAttributes.Width = 15;
            InkCanvas.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Ellipse;
            InkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString("White");
            btnshow.Foreground = Brushes.Black;
            btnshow.Content = "\u21C9";
            btnshow.Click += Btnshow_Click;
        }
        private bool isShow;

        private void Btnshow_Click(object sender, RoutedEventArgs e)
        {
            if (!isShow)
            {
                Storyboard sb = Resources["OpenMenu"] as Storyboard;
                sb.Begin(LeftMenu);
            }
            else
            {
                Storyboard sb = Resources["CloseMenu"] as Storyboard;
                sb.Begin(LeftMenu);
            }
            isShow = !isShow;
        }

        private void InkCanvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            HandleGesture();
        }
        private void InkCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleGesture();
        }
        private void HandleGesture()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                InkCanvas.Strokes.Save(ms);
                var ink = new Ink();
                ink.Load(ms.ToArray());

                using (RecognizerContext context = new RecognizerContext())
                {
                    if (ink.Strokes.Count > 0)
                    {
                        context.Strokes = ink.Strokes;
                        RecognitionStatus status;

                        var result = context.Recognize(out status);

                        if (status == RecognitionStatus.NoError)
                        {
                            string letter = result.TopString;
                            File.AppendAllText(@"D:\Trace.txt", $"Letter wriiten is {letter}" + Environment.NewLine);

                        }
                    }
                }
            }
            InkCanvas.Strokes.Clear();
        }
    }
}
