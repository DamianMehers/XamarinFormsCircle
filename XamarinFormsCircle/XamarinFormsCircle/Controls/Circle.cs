using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace XamarinFormsCircle.Controls
{
    public class Circle : ContentView
    {
        public static readonly BindableProperty SegmentsProperty = BindableProperty.Create(nameof(Segments), typeof(IEnumerable<Segment>), typeof(Circle), defaultValueCreator: DefaultSegmentsCreator, propertyChanged: OnSegmentsPropertyChanged);

        private static object DefaultSegmentsCreator(BindableObject bindable)
        {
            var circle = (Circle)bindable;
            var observableCollection = new ObservableCollection<Segment>();
            observableCollection.CollectionChanged += circle.OnSegmentsCollectionChanged;
            return observableCollection;
        }


        private static void OnSegmentsPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var circle = (Circle)bindable;

            var observableCollection = newvalue as ObservableCollection<Segment>;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged += circle.OnSegmentsCollectionChanged;
            }
            observableCollection = oldvalue as ObservableCollection<Segment>;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged -= circle.OnSegmentsCollectionChanged;
            }
            if (newvalue != null)
            {
                foreach (var segment in (IEnumerable<Segment>)newvalue)
                {
                    segment.Parent = circle;
                }
            }
            ((SKCanvasView)circle.Content).InvalidateSurface();
        }


        private void OnSegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Segment segment in e.NewItems)
                {
                    segment.Parent = this;
                }
            }
            ((SKCanvasView)Content).InvalidateSurface();
        }

        public IList<Segment> Segments {
            get => (IList<Segment>)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public Circle()
        {
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            var center = new SKPoint(info.Width / 2F, info.Height / 2F);

            var startAngle = 0F;
            foreach (var segment in Segments)
            {
                var radius = Math.Min(info.Width / 2, info.Height / 2) * segment.Radius;
                var rect = new SKRect(center.X - radius, center.Y - radius,
                    center.X + radius, center.Y + radius);

                using (var path = new SKPath())
                using (var fillPaint = new SKPaint())
                {
                    fillPaint.Style = SKPaintStyle.Fill;
                    fillPaint.Color = segment.Color.ToSKColor();

                    path.MoveTo(center);
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    path.ArcTo(rect, startAngle, segment.SweepAngle == 360F ? 359.99F : segment.SweepAngle, false);
                    startAngle += segment.SweepAngle;
                    path.Close();
                    canvas.DrawPath(path, fillPaint);
                }
            }

        }
    }

    public class Segment : BindableObject
    {
        public Circle Parent { get; set; }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(Segment), Color.ForestGreen, propertyChanged: OnSegmentPropertyChanged);
        public static readonly BindableProperty SweepAngleProperty = BindableProperty.Create(nameof(SweepAngle), typeof(float), typeof(Segment), 90F, propertyChanged: OnSegmentPropertyChanged);
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(float), typeof(Segment), 1F, propertyChanged: OnSegmentPropertyChanged);

        private static void OnSegmentPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var segment = (Segment)bindable;
            ((SKCanvasView)segment?.Parent?.Content)?.InvalidateSurface();
        }


        public Color Color {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public float SweepAngle {
            get => (float)GetValue(SweepAngleProperty);
            set => SetValue(SweepAngleProperty, value);
        }
        public float Radius {
            get => (float)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

    }
}
