using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gestures
{
    public class CanvasBehavior : Behavior<Canvas>
    {
        protected Point SwipeStart;
        protected List<TouchPoints> touchPointCllection = new List<TouchPoints>();
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TouchUp += AssociatedObject_TouchUp;
            AssociatedObject.TouchDown += AssociatedObject_TouchDown;
            AssociatedObject.ManipulationCompleted += AssociatedObject_ManipulationCompleted;
        }

        private void AssociatedObject_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (touchPointCllection.Any() && touchPointCllection.All(_ => _.swipeType == touchPointCllection.First().swipeType))
            {
                swipeGesture gesture = touchPointCllection.First().swipeType;
                if (touchPointCllection.Count == 1 && gesture != swipeGesture.Down)
                {
                    if (gesture == swipeGesture.Left)
                    {
                        File.AppendAllText(@"D:\Trace.txt", $"Swiped Left" + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(@"D:\Trace.txt", $"Swiped Right" + Environment.NewLine);
                    }
                }
                else if (touchPointCllection.Count == 2 && gesture == swipeGesture.Down)
                {
                    File.AppendAllText(@"D:\Trace.txt", $"2 finger Swiped Down" + Environment.NewLine);
                }
            }
            touchPointCllection.Clear();
        }

        private void AssociatedObject_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            SwipeStart = e.GetTouchPoint(AssociatedObject).Position;
        }

        private void AssociatedObject_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            var Swipe = e.GetTouchPoint(AssociatedObject).Position;

            //Swipe Right
            if (Swipe.X > (SwipeStart.X + 200))
            {
                touchPointCllection.Add(new TouchPoints { DownPoint = SwipeStart, UpPoint = Swipe, swipeType = swipeGesture.Right });
            }
            //Swipe Left
            else if (Swipe.X < (SwipeStart.X - 200))
            {
                touchPointCllection.Add(new TouchPoints { DownPoint = SwipeStart, UpPoint = Swipe, swipeType = swipeGesture.Left });
            }
            //Swipe Down
            else if (Swipe.Y > (SwipeStart.Y + 200))
            {
                touchPointCllection.Add(new TouchPoints { DownPoint = SwipeStart, UpPoint = Swipe, swipeType = swipeGesture.Down });
            }
            e.Handled = true;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.TouchUp -= AssociatedObject_TouchUp;
            AssociatedObject.TouchDown -= AssociatedObject_TouchDown;
            AssociatedObject.ManipulationCompleted -= AssociatedObject_ManipulationCompleted;
        }

    }
    public class TouchPoints
    {
        public Point UpPoint { get; set; }
        public Point DownPoint { get; set; }
        public swipeGesture swipeType { get; set; }
    }
    public enum swipeGesture
    {
        Right,
        Left,
        Down
    }
}
