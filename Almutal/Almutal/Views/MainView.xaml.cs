using AlmutalCore;
using AlmutalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Almutal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : BasePage, INotifyPropertyChanged
    {
        private Algorithm Algorithm;
        private double containerWidth = 280;
        private double containerHeight = 130;
        private double Kerf = 0.3;

        private List<Box> noPositionBoxes = new List<Box>
        {
            new Box(49.5,74.5),
new Box(49.5,74.5),
new Box(49.5,74.5),
new Box(49.5,74.5),
new Box(49.5,44.5),
new Box(29.5,74.5),
new Box(29.5,74.5),
new Box(29.5,64.5),
new Box(29.5,64.5),
new Box(29.5,84.5),
new Box(29.5,84.5),
new Box(29.5,84.5),
new Box(29.5,84.5),
new Box(29.5,65.5),
new Box(29.5,39.5),
new Box(29.5,29.5),
new Box(29.5,29.5),
new Box(24.5,29.5),
new Box(24.5,29.5),
new Box(29.5,64.5),
new Box(69.5,64.5),
new Box(48.5,78.5),
new Box(34.5,78.5),
new Box(38.5,78.5),
new Box(38.5,133.5),
new Box(29.5,129.5),
new Box(29.5,129.5),
new Box(49.5,30.5),
new Box(88.5,68.5),
new Box(69.5,68.5),
new Box(43.5,68.5),
new Box(88.5,28.5),
new Box(33.5,68.5),
new Box(33.5,73.5),
new Box(46.6,76.6),
new Box(42.1,74.6),
new Box(42.1,74.6),
new Box(32.6,76.6),
new Box(46.6,16.6),
new Box(46.6,16.6),
new Box(16.6,76.6),
new Box(36.6,76.6),
new Box(31.6,66.6),
new Box(39.4,69.4),
new Box(34.9,69.4),
new Box(32.6,69.4),
new Box(39.4,34.4),
new Box(39.4,9.4 ),
new Box(39.4,9.4 ),
new Box(29.4,69.4),
new Box(29.4,69.4),
new Box(24.4,59.4),
new Box(31.8,59.4),
new Box(28.5,29.5),
new Box(28.5,29.5),
new Box(28.5,29.5),
new Box(28.5,29.5),
new Box(28.5,79.5),
new Box(28.5,34.5),
new Box(28.5,24.5),
new Box(28.5,24.5),
new Box(69.4,25.4),
new Box(43.3,66.6),
new Box(43.3,66.6),
new Box(33.8,66.6),
new Box(33.8,66.6),
new Box(46.1,66.6),
new Box(31.6,66.6),
new Box(26.6,82.6),
new Box(71.6,31.6),
new Box(39.1,66.6),
new Box(36.1,59.4),
new Box(26.6,59.4),
new Box(26.6,59.4),
new Box(36.1,59.4),
new Box(26.6,59.4),
new Box(34.4,59.4),
new Box(26.6,59.4),
new Box(24.4,59.4),
new Box(19.4,79.4),
new Box(64.4,24.4),
new Box(31.8,29.4),
new Box(46.5,41.5),
new Box(39.4,34.4),
new Box(28.5,29.5),
new Box(68.4,25.4),
new Box(68.4,46.6),
new Box(46.6,76.6),
new Box(59.5,59.5),
new Box(59.5,59.5),
new Box(59.5,59.5),
new Box(59.5,59.5),
new Box(63.5,63.5),
new Box(63.5,63.5),
new Box(74.5,64.5),
new Box(74.5,64.5),
new Box(78.5,68.5),
new Box(78.5,68.5),
new Box(78.5,68.5),
new Box(78.5,68.5),
new Box(74.5,54.5),
new Box(74.5,54.5),
new Box(69.5,59.5),
new Box(69.5,59.5),
new Box(59.5,59.5),
new Box(59.5,59.5),
new Box(60,70),
new Box(55,55)
        };

        public string SheetsNumber { get; set; }
        public string BoxDetails { get; set; }
        public bool PopupVisible { get; set; }
        public ObservableCollection<StockSheet> Items { get; set; }
        public ObservableCollection<View> SkiItems { get; set; }

        public Command DetailsCommand => new Command<Box>(x => {

            if (x == null)
                return;

            PopupVisible = true;
            BoxDetails = $"Width: {x.Width}\nLength: {x.Length}";
        });

        public Command DismissCommand => new Command(() => PopupVisible = false);
        public MainView()
        {
            InitializeComponent();
            Items = new ObservableCollection<StockSheet>();
            SkiItems = new ObservableCollection<View>();
            BindingContext = this;

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
                double boxesTotalArea = 0;
                foreach (var sheet in boxes)
                {
                    foreach (var box in sheet.CuttedPanels)
                    {
                        boxesTotalArea += box.Area;
                    }
                }
                //boxes.ForEach(x =>
                //{
                //    boxesTotalArea = x.CuttedPanels.Sum(y => y.Area);
                //});
                //var allboxes = noPositionBoxes.Sum(x => x.Length * x.Width);
                var totalSheetsArea = containerHeight * containerWidth * boxes.Count;
                var used = Math.Round(100 * boxesTotalArea / totalSheetsArea, 2);
                var wast = Math.Round(100 * (1 - boxesTotalArea / totalSheetsArea), 2);
                SheetsNumber = $"Sheets Count: {boxes.Count} \nUsed: {used}%  Wast: {wast}%";
                foreach (var item in boxes)
                {
                    Items.Add(item);
                }

                //var results = boxes.GroupBy(p => p.ParentId);
                //paint(boxes);

            }

            //GenerateUI();
        }

        private void CreateBoxes()
        {
            //var boxesList = new List<Box>();
            //Random rand = new Random();

            //for (var i = 0; i < BoxNumber; i++)
            //{
            //    var box = new Box
            //    {
            //        Width = rand.Next(10, MaxWidth),
            //        Length = rand.Next(10, MaxHeight)
            //    };
            //    boxesList.Add(box);
            //    //Console.WriteLine(box.width.ToString() + "x" + box.height.ToString());
            //}

            Algorithm = new Algorithm(containerHeight, containerWidth, noPositionBoxes, Kerf);

        }

        //private void paint(IEnumerable<IGrouping<int, Box>> results)
        private void paint(List<StockSheet> results)
        {
            // Create SKCanvasView to view result
            SheetsNumber = results.Count().ToString();
            //int row = 1;
            foreach (var sheet in results)
            {
                var absolute = new AbsoluteLayout();

                foreach (var panel in sheet.CuttedPanels)
                {
                    if (panel.Position != null)
                    {
                        var frame = new Frame
                        {
                            Padding = 0,
                            BorderColor = Color.Black,
                            BackgroundColor = Color.FromHex(panel.Color),
                            Content = new Label
                            {

                                TextColor = Color.White,
                                Text = $"{panel.Width}*{panel.Length}",
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center
                            },



                        };
                        Rectangle rect = new Rectangle();
                        //if(panel.Width > panel.Length)
                        rect = new Rectangle(panel.Position.X * 1.2, panel.Position.Y * 1.2, panel.Width * 1.2, panel.Length * 1.2);
                        //else
                        //    rect = new Rectangle(panel.Position.X, panel.Position.Y, panel.Position.X + panel.Length, panel.Position.Y + panel.Width);

                        absolute.Children.Add(frame, rect);


                    }
                }
                SkiItems.Add(absolute);
                ////var canvasView = new SKCanvasView();
                //DrawRects(new SKCanvasView(), sheet, row);
                //row++;

            }
            //if (containerHeight > containerWidth)
            //    scroll.HeightRequest = Math.Ceiling(results.Count() * containerWidth);
            //else
            //    scroll.HeightRequest = Math.Ceiling(results.Count() * containerHeight);

            //grid.Children.Add(stack, 0, 1);
            //scroll.Content = grid;

        }

    //    private void DrawRects(SKCanvasView sKCanvasView, IGrouping<int, Box> sheet, int row)
    //    {


    //        sKCanvasView.PaintSurface += (s, e) =>
    //{
    //    SKImageInfo info = e.Info;
    //    SKSurface surface = e.Surface;
    //    SKCanvas canvas = surface.Canvas;
    //    canvas.Clear(SKColors.Black);
    //    //canvas.Clear();
    //    Random rand = new Random();
    //    sKCanvasView.WidthRequest = 300;
    //    sKCanvasView.HeightRequest = 180;
    //    sKCanvasView.InputTransparent = true;
    //    //canvas.Scale((float)(e.Info.Width / Math.Ceiling(sKCanvasView.Width)));

    //    //float canvasMin = Math.Min(e.Info.Width, e.Info.Height);
    //    //// get the size 
    //    //float svgMax = Math.Max(containerWidth, containerHeight);

    //    //// get the scale to fill the screen
    //    //float scale = canvasMin / svgMax;
    //    //canvas.Scale(scale);

    //    foreach (var box in sheet.AsEnumerable())
    //    {
    //        if (box.Position != null)
    //        {
    //            area += box.Area;

    //            var boxrect = new SKRect(box.Position.X, box.Position.Y, box.Position.X + box.Width, box.Position.Y + box.Length);

    //            uint red = (uint)rand.Next(20, 234);
    //            uint grn = (uint)rand.Next(20, 234);
    //            uint blu = (uint)rand.Next(20, 234);
    //            var text = $"{box.Width} * {box.Length} ID:{box.Id}";

    //            using (var rectPaint = new SKPaint()
    //            {
    //                Color = new SKColor((byte)red, (byte)grn, (byte)blu)
    //            })
    //            using (var textPaint = new SKPaint()
    //            {
    //                Color = SKColors.White,
    //                Style = SKPaintStyle.Fill
    //            })
    //            {


    //                //canvas.Scale(scale);


    //                //textPaint.TextSize = 0.9f * textPaint.FontMetrics.Top * textPaint.TextSize / textWidth;
    //                //textPaint.MeasureText(text, ref boxrect);

    //                canvas.DrawRect(boxrect, rectPaint);

    //                //if (box.Width > box.Length)
    //                //{
    //                float textWidth = textPaint.MeasureText(text);

    //                // Adjust TextSize property so text is 90% of screen width
    //                textPaint.TextSize = 0.9f * boxrect.Height * textPaint.TextSize / textWidth;
    //                // Calculate offsets to center the text on the box
    //                float xText = boxrect.MidX - (boxrect.Height * .9f) / 2;
    //                float yText = boxrect.MidY + boxrect.Height / 8;
    //                // And draw the text
    //                canvas.DrawText(text, xText, yText, textPaint);
    //                //}
    //                //else
    //                //{
    //                //    // auto save
    //                //    using (new SKAutoCanvasRestore(canvas))
    //                //    {
    //                //        // do any transformations
    //                //        canvas.RotateDegrees(-90);
    //                //        // do serious work
    //                //        float textWidth = textPaint.MeasureText(text);

    //                //        // Adjust TextSize property so text is 90% of screen width
    //                //        textPaint.TextSize = 0.9f * boxrect.Height * textPaint.TextSize / textWidth;
    //                //        // Calculate offsets to center the text on the box
    //                //        float xText = boxrect.MidX - (boxrect.Width * .9f) / 2;
    //                //        float yText = boxrect.MidY + boxrect.Width / 8;
    //                //        // And draw the text
    //                //        canvas.DrawText(text, xText, yText, textPaint);
    //                //        // auto restore, even on exceptions or errors
    //                //    }
    //                //    //textPaint.TextSize = 0.9f * boxrect.Width * textPaint.TextSize / textWidth;
    //                //    ////// Calculate offsets to center the text on the box
    //                //    //float xText = boxrect.MidY - (boxrect.Width * .9f) / 2;
    //                //    //float yText = boxrect.MidX + boxrect.Height / 8;
    //                //    ////// And draw the text
    //                //    ////canvas.DrawText(text, xText, yText, textPaint);
    //                //    //using (SKPaint skPaint = new SKPaint())
    //                //    //{
    //                //    //    SKPath path = new SKPath();
    //                //    //    skPaint.TextAlign = SKTextAlign.Center;
    //                //    //    path.MoveTo(boxrect.Width / 8, boxrect.MidY + boxrect.Height / 2);
    //                //    //    path.LineTo(boxrect.Width / 8, boxrect.MidY - boxrect.Height / 2);
    //                //    //    skPaint.TextSize = 0.9f * boxrect.Height * textPaint.TextSize / textWidth;
    //                //    //    canvas.DrawTextOnPath(text, path, 0, 0, skPaint);
    //                //    //    path.Dispose();
    //                //    //}
    //                //}


    //            }
    //        }

    //    }
    //};

    //        //sKCanvasView.IgnorePixelSkcaling = true;

    //        //sKCanvasView.InputTransparent = true;
    //        //sKCanvasView.HorizontalOptions = LayoutOptions.FillAndExpand;
    //        //sKCanvasView.VerticalOptions = LayoutOptions.FillAndExpand;
    //        //sKCanvasView.IgnorePixelScaling = true;
    //        //sKCanvasView.WidthRequest = 900;
    //        //sKCanvasView.HeightRequest = 180;
    //        //sKCanvasView.Scale = 1.2;
    //        //var scroll = new ScrollView
    //        //{
    //        //    Content = sKCanvasView,
    //        //    WidthRequest = 180

    //        //};
    //        //if (skiImage != null)
    //        //{
    //        //    SKData encoded = skiImage.Encode();
    //        //    // get a stream over the encoded data
    //        //    var stream = encoded.AsStream();
    //        //    var image = new Image
    //        //    {
    //        //        Source = ImageSource.FromStream(() => stream)
    //        //    };
    //        //    stack.Children.Add(image);
    //        //}
    //        var flex = new FlexLayout
    //        {
    //            Wrap = FlexWrap.Wrap
    //        };
    //        flex.Children.Add(sKCanvasView);
    //        SkiItems.Add(flex);

    //        //stack.Children.Add(new ScrollView { 
    //        //    Content = sKCanvasView
    //        //});

    //        //gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
    //        //gridLayout.Children.Add(sKCanvasView, 0, row);
    //    }

        private void Redo_Clicked(object sender, EventArgs e)
        {
            //CreateBoxes();

            Items.Clear();

            if (Algorithm == null)
                return;

            var boxes = Algorithm.Pack();
            if (boxes.Count > 0)
            {
                foreach (var item in boxes)
                {
                    Items.Add(item);
                }

            }
            //CreateBoxes();

            //if (Algorithm == null)
            //    return;
            //var sheets = Algorithm.Pack();
            //if (sheets.Count > 0)
            //{
            //    //if (grid.Children.Count > 1)
            //    //{
            //    //    for (int i = 1; i < grid.Children.Count; i++)
            //    //    {
            //    //        grid.Children.RemoveAt(i);
            //    //    }
            //    //}

            //    //gridLayout.Children.Clear();
            //    Items.Clear();
            //    //var results = boxes.GroupBy(p => p.ParentId);
            //    //var results = boxes.GroupBy(p => p.ParentId);
            //    //paint(sheets);

            //}

            //GenerateUI();
        }
    }
}