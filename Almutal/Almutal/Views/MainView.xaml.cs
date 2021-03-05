using AlmutalCore;
using AlmutalCore.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Shapes;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rectangle = Xamarin.Forms.Shapes.Rectangle;

namespace Almutal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        private Algorithm Algorithm;
        private const uint BoxNumber = 20;
        private const int MaxWidth = 180;
        private const int MaxHeight = 180;
        private float containerWidth = 300f;
        private float containerHeight = 180f;
        private List<Box> noPositionBoxes = new List<Box>();
        private string label = string.Empty;
        private float area;

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CreateBoxes();

            if (Algorithm == null)
                return;
            var boxes = Algorithm.Pack();
            if (boxes.Count > 0)
            {
                var results = boxes.GroupBy(p => p.ParentId);
                paint(results);

            }
        }

        private void CreateBoxes()
        {
            var boxesList = new List<Box>();
            Random rand = new Random();

            for (var i = 0; i < BoxNumber; i++)
            {
                var box = new Box
                {
                    Width = rand.Next(10, MaxWidth),
                    Length = rand.Next(10, MaxHeight)
                };
                boxesList.Add(box);
                //Console.WriteLine(box.width.ToString() + "x" + box.height.ToString());
            }

            Algorithm = new Algorithm(containerHeight, containerWidth,  boxesList);

        }

        private void paint(IEnumerable<IGrouping<int, Box>> results)
        {
            // Create SKCanvasView to view result
            int row = 1;
            foreach (var sheet in results)
            {

                //var canvasView = new SKCanvasView();
                DrawRects(new SKCanvasView(), sheet, row);
                row++;

            }

        }

        private void DrawRects(SKCanvasView sKCanvasView, IGrouping<int, Box> sheet, int row)
        {
            sKCanvasView.PaintSurface += (s, e) =>
            {
                SKImageInfo info = e.Info;
                SKSurface surface = e.Surface;
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(SKColors.Black);
                //canvas.Clear();
                Random rand = new Random();
                var margin = 10;

                foreach (var box in sheet.AsEnumerable())
                {
                    if (box.Position != null)
                    {
                        area += box.Area;

                        var boxrect = new SKRect(box.Position.X, box.Position.Y, box.Position.X + box.Width, box.Position.Y + box.Length);

                        uint red = (uint)rand.Next(20,234);
                        uint grn = (uint)rand.Next(20,234);
                        uint blu = (uint)rand.Next(20,234);
                        var text = $"{box.Width} * {box.Length}";

                        using (var rectPaint = new SKPaint()
                        {
                            Color = new SKColor((byte)red, (byte)grn, (byte)blu)
                        })
                        using (var textPaint = new SKPaint()
                        {
                            Color = SKColors.White,
                            Style = SKPaintStyle.Fill
                        })
                        {


                            //canvas.Scale(scale);

                            // Adjust TextSize property so text is 90% of screen width
                            float textWidth = textPaint.MeasureText(text);
                            textPaint.TextSize = 0.9f * boxrect.Height * textPaint.TextSize / textWidth;
                            //textPaint.TextSize = 0.9f * textPaint.FontMetrics.Top * textPaint.TextSize / textWidth;
                            //textPaint.MeasureText(text, ref boxrect);

                            canvas.DrawRect(boxrect, rectPaint);
                            // Calculate offsets to center the text on the box
                            float xText = boxrect.MidX - (boxrect.Height * .9f) / 2;
                            float yText = boxrect.MidY + boxrect.Height/8;
                            // And draw the text
                            canvas.DrawText(text, xText, yText, textPaint);
                        }
                    }

                }

                float canvasMin = Math.Min(e.Info.Width, e.Info.Height);
                // get the size 
                float svgMax = Math.Max(containerWidth, containerHeight);

                // get the scale to fill the screen
                float scale = canvasMin / svgMax;
                //canvas.Scale(50);


            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            sKCanvasView.InputTransparent = true;
            sKCanvasView.IgnorePixelScaling = true;
            sKCanvasView.WidthRequest = 300;
            sKCanvasView.HeightRequest = 180;
            //sKCanvasView.Scale = 1.2;

            grid.Children.Add(sKCanvasView, 0, row);
        }

        private void Redo_Clicked(object sender, EventArgs e)
        {
            CreateBoxes();

            if (Algorithm == null)
                return;
            var boxes = Algorithm.Pack();
            if (boxes.Count > 0)
            {
                //if (grid.Children.Count > 1)
                //{
                //    for (int i = 1; i < grid.Children.Count; i++)
                //    {
                //        grid.Children.RemoveAt(i);
                //    }
                //}

                grid.Children.Clear();
                var results = boxes.GroupBy(p => p.ParentId);
                paint(results);

            }
        }
    }
}