using AlmutalCore;
using AlmutalCore.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
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
    public partial class MainView : ContentPage, INotifyPropertyChanged
    {
        private Algorithm Algorithm;
        private const uint BoxNumber = 155;
        private const int MaxWidth = 180;
        private const int MaxHeight = 180;
        private double containerWidth = 280;
        private double containerHeight = 130;
        private string label = string.Empty;
        private float area;
        //private StackLayout stack;
        private Grid grid;

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
            //new Box(20, 172),
            //new Box(75, 122),
            //new Box(50, 49),
            //new Box(120, 50),
            //new Box(10, 175),
            //new Box(169, 172),
            //new Box(128, 158),
            //new Box(32 ,47  ),
            //new Box(16 ,90  ),
            //new Box(21 ,63  ),
            //new Box(10 ,113 ),
            //new Box(22 ,34  ),
            //new Box(14 ,41  ),
            //new Box(16 ,31  ),
            //new Box(14 ,32  ),
            //new Box(164, 167),
            //new Box(132, 176),
            //new Box(13 ,161 ),
            //new Box(26 ,41  ),
            //new Box(15 ,65  ),
            //new Box(23 ,25  ),
            //new Box(10 ,38  ),
            //new Box(152, 171),
            //new Box(129, 171),
            //new Box(24 ,156 ),
            //new Box(150, 171),
            //new Box(123, 156),
            //new Box(29 ,156 ),
            //new Box(37 ,56  ),
            //new Box(26 ,76  ),
            //new Box(30 ,65  ),
            //new Box(146, 171),
            //new Box(127, 147),
            //new Box(34 ,170 ),
            //new Box(34 ,53  ),
            //new Box(18 ,94  ),
            //new Box(30 ,54  ),
            //new Box(131, 171),
            //new Box(121, 150),
            //new Box(49 ,151 ),
            //new Box(57 ,58  ),
            //new Box(40 ,82  ),
            //new Box(10 ,77  ),
            //new Box(148, 151),
            //new Box(143, 149),
            //new Box(30 ,122 ),
            //new Box(137, 155),
            //new Box(135, 137),
            //new Box(43 ,124 ),
            //new Box(130, 140),
            //new Box(39 ,130 ),
            //new Box(117, 155),
            //new Box(112, 160),
            //new Box(60 ,144 ),
            //new Box(61 ,62  ),
            //new Box(50 ,75  ),
            //new Box(103, 169),
            //new Box(110, 158),
            //new Box(74 ,165 ),
            //new Box(46 ,60  ),
            //new Box(28 ,98  ),
            //new Box(25 ,93  ),
            //new Box(120, 141),
            //new Box(126, 134),
            //new Box(60 ,112 ),
            //new Box(121, 139),
            //new Box(93 ,175 ),
            //new Box(54 ,118 ),
            //new Box(65 ,87  ),
            //new Box(66 ,72  ),
            //new Box(98 ,160 ),
            //new Box(105, 148),
            //new Box(80 ,151 ),
            //new Box(46 ,71  ),
            //new Box(36 ,59  ),
            //new Box(123, 124),
            //new Box(91 ,166 ),
            //new Box(57 ,111 ),
            //new Box(60 ,79  ),
            //new Box(38 ,79  ),
            //new Box(36 ,79  ),
            //new Box(88 ,170 ),
            //new Box(83 ,178 ),
            //new Box(87 ,161 ),
            //new Box(83 ,91  ),
            //new Box(29 ,78  ),
            //new Box(33 ,64  ),
            //new Box(110, 134),
            //new Box(98 ,149 ),
            //new Box(64 ,114 ),
            //new Box(28 ,79  ),
            //new Box(94 ,153 ),
            //new Box(81 ,177 ),
            //new Box(84 ,135 ),
            //new Box(52 ,92  ),
            //new Box(93 ,154 ),
            //new Box(92 ,151 ),
            //new Box(49 ,21  ),
            //new Box(79 ,168 ),
            //new Box(108, 121),
            //new Box(96 ,135 ),
            //new Box(30 ,101 ),
            //new Box(43 ,124 ),
            //new Box(93 ,131 ),
            //new Box(108, 111),
            //new Box(86 ,127 ),
            //new Box(72 ,166 ),
            //new Box(95 ,120 ),
            //new Box(83 ,91  ),
            //new Box(55 ,104 ),
            //new Box(99 ,115 ),
            //new Box(85 ,132 ),
            //new Box(53 ,102 ),
            //new Box(31 ,95  ),
            //new Box(12 ,98  ),
            //new Box(64 ,172 ),
            //new Box(64 ,168 ),
            //new Box(76 ,140 ),
            //new Box(51 ,162 ),
            //new Box(40 ,102 ),
            //new Box(79 ,134 ),
            //new Box(89 ,117 ),
            //new Box(95 ,108 ),
            //new Box(73 ,136 ),
            //new Box(57 ,173 ),
            //new Box(85 ,114 ),
            //new Box(77 ,119 ),
            //new Box(22 ,109 ),
            //new Box(65 ,150 ),
            //new Box(68 ,142 ),
            //new Box(56 ,172 ),
            //new Box(39 ,130 ),
            //new Box(45 ,111 ),
            //new Box(57 ,163 ),
            //new Box(65 ,138 ),
            //new Box(54 ,162 ),
            //new Box(45 ,106 ),
            //new Box(38 ,113 ),
            //new Box(55 ,153 ),
            //new Box(56 ,142 ),
            //new Box(50 ,157 ),
            //new Box(34 ,130 ),
            //new Box(39 ,109 ),
            //new Box(25 ,129 ),
            //new Box(51 ,148 ),
            //new Box(41 ,175 ),
            //new Box(48 ,147 ),
            //new Box(33 ,132 ),
            //new Box(26 ,133 ),
            //new Box(25 ,129 ),
            //new Box(29 ,100 ),
            //new Box(43 ,157 ),
            //new Box(34 ,160 ),
            //new Box(12 ,173 ),
            //new Box(22 ,140 ),
            //new Box(17 ,133 ),
            //new Box(26 ,175 ),
            //new Box(15 ,170 ),
            //new Box(12 ,173 )
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
            BoxDetails = $"Width: {x.Width}  Length: {x.Length}";
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
                SheetsNumber = $"No {boxes.Count}";
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

            Algorithm = new Algorithm(containerHeight, containerWidth, noPositionBoxes);

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