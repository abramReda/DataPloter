namespace DataPloter;

using DataPloter.Constans;
using SkiaSharp;
using System.Linq;
using static Helper;
public class SingleAxis
{
    /// <summary>
    /// Get how many pixel to represent a single unit
    /// </summary>
    public Double PixelPerUnit => Length / TotalUnites;
    /// <summary>
    /// get or set Width of  Axis in pixel
    /// </summary>
    public double Length { get; set; }

    /// <summary>
    /// get or set Coordinate Range For the Axis
    /// </summary>
    public (double Start,double End) Rang { get; set; } = (0f, 10f);

    /// <summary>
    /// get totlal Distance unit 
    /// </summary>
    public double TotalUnites => Rang.End - Rang.Start;

    /// <summary>
    /// Get Step
    /// </summary>
    /// <remarks>
    /// Distanse Between Visable perpendicular line on the Axis
    /// </remarks>
    public double Step => TotalUnites / 10f;


    /// <summary>
    /// Transform coordinate(distanse) to Pixel World
    /// </summary>
    /// <param name="coordinate">x coordinate </param>
    /// <returns></returns>
    public int Coordinate2Pixel(double coordinate)
# warning need to use scale transform insted of subtract
    => (int)((coordinate - Rang.Start) * PixelPerUnit);

    public double Pixel2XCoordinate(double Pixel)
    => (Pixel / PixelPerUnit);
}
public class Axis
{

    #region private Field
    private bool _FirstGraphRander = true;
    private readonly SingleAxis _xAxis = new();
    private readonly SingleAxis _yAxis = new();

    private readonly SKPaint _axisPaint = new()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColor.Parse("#767676")
    };
    private readonly SKPaint _mainAxisPaint = new()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Black,
        StrokeWidth = 2
    };
    private readonly SKPaint _pointPaint = new()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Yellow
    };
    private readonly SKPaint _textPaint = new()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Yellow,
        TextSize = 32,
    };
    private SKCanvas? _canvas = null;
    #endregion


    #region Constractors
    public Axis()
    {

    }
    public Axis(SKCanvas canvas, SKImageInfo info)
    {
        Init(canvas, info);

    }
    #endregion

    public void Init(SKCanvas canvas, SKImageInfo info)
    {
        _canvas = canvas;
        _xAxis.Length = info.Width * 0.8;
        _yAxis.Length = info.Height * 0.8;

        _canvas.Translate((int)(info.Width * 0.1), (int)(info.Height * 0.1));
    }
    

    public void ShowGrid(DateTime? Startdate = null)
    {
        // Draw Vertical Axis
        for (double XLoc = _xAxis.Rang.Start; XLoc <= _xAxis.Rang.End; XLoc += _xAxis.Step)
        {
            DrawLine(XLoc, _yAxis.Rang.Start, XLoc, _yAxis.Rang.End,XLoc==_xAxis.Rang.Start?LineType.MainAxisline:LineType.GuideAxisline);
            WritText(XLoc.ToString("0.0").PadRight(4), XLoc, _yAxis.Rang.Start - 0.5f * _yAxis.Step);
        }

        // Draw Herozontal Axis
        for (double Yloc = _yAxis.Rang.Start; Yloc <= _yAxis.Rang.End; Yloc += _yAxis.Step)
        {
            DrawLine(_xAxis.Rang.Start, Yloc, _xAxis.Rang.End, Yloc, Yloc == _yAxis.Rang.Start ? LineType.MainAxisline : LineType.GuideAxisline);
            string label = Startdate == null ? Yloc.ToString("0.0").PadRight(4) :
                Startdate?.AddDays(Yloc).ToString("d");
            WritText(label, _xAxis.Rang.Start - 0.7f *_xAxis.Step, Yloc);
        }
    }

    public void Translate(double totalX, double totalY)
    {
        Console.WriteLine($"X : {totalX}");
        Console.WriteLine($"y : {totalY}");
        var x = _xAxis.Pixel2XCoordinate(totalX);
        var y = _yAxis.Pixel2XCoordinate(totalY);
        Console.WriteLine($"Converted x : {x}");
        _xAxis.Rang = (_xAxis.Rang.Start - x, _xAxis.Rang.End - x);
        _yAxis.Rang = (_yAxis.Rang.Start - y, _yAxis.Rang.End - y);
    }

    /// <summary>
    /// Draw line Between 2 point
    /// </summary>
    /// <param name="p1">First Point</param>
    /// <param name="p2">Second Point</param>
    public void DrawLine(GraphPoint p1,GraphPoint p2)=>
        DrawLine(p1.XCoordinate,p1.YCoordinate,p2.XCoordinate,p2.YCoordinate);
    /// <summary>
    /// Draw line Between 2 point 
    /// </summary>
    /// <param name="x0">x coordinate of first point</param>
    /// <param name="y0">y coordinate of first point</param>
    /// <param name="x1">x coordinate of second point</param>
    /// <param name="y1">y coordinate of second point</param>
    /// <param name="lineType">Not implemented yet</param>
    public void DrawLine(double x0, double y0, double x1, double y1,LineType lineType=LineType.StraightLine)
    {
        // ToDo implement the line Type
        var _x0 = _xAxis.Coordinate2Pixel(x0);
        var _x1 = _xAxis.Coordinate2Pixel(x1);
        var _y0 = _yAxis.Coordinate2Pixel(y0);
        var _y1 = _yAxis.Coordinate2Pixel(y1);
        var paint = lineType switch
        {
            LineType.MainAxisline => _mainAxisPaint,
            _ => _axisPaint,
        };
        _canvas?.DrawLine(_x0, _y0, _x1, _y1, paint);
    }

    public void DrawPoint(GraphPoint p, float r = 16f)
    {
        DrawPoint(p.XCoordinate, p.YCoordinate,r);
    }
    /// <summary>
    /// Drow a circule point
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="r">redious of point</param>
    public void DrawPoint(double x, double y, float r =16f)
    {
        if (!IsInRange(x, _xAxis.Rang) || !IsInRange(y,_yAxis.Rang)) return;
        var _x = _xAxis.Coordinate2Pixel(x);
        var _y = _yAxis.Coordinate2Pixel(y);
        _canvas?.DrawCircle(_x, _y, r, _pointPaint);
    }

    /// <summary>
    /// writ text in cordenat
    /// </summary>
    /// <param name="str"> string you want to add in axis</param>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public void WritText(string str, double x, double y)
    {
        var _x = _xAxis.Coordinate2Pixel(x);
        var _y = _yAxis.Coordinate2Pixel(y);
        _canvas?.DrawText(str, _x, _y, _textPaint);

    }

    public void ZoomInOut(double scale)
    {
        double newWidth = _xAxis.TotalUnites * scale;
        double newHeight =_yAxis.TotalUnites * scale;
        //var dw = (newWidth - TotalXUnite) / 2;
        //var dh = (newHeight - TotalYUnite) / 2;

        //XRang = (XRang.Start - dw , XRang.End + dw);
        //YRang = (YRang.Start - dh, YRang.End + dh);

        _xAxis.Rang = (_xAxis.Rang.Start , _xAxis.Rang.Start + newWidth);
        _yAxis.Rang = (_yAxis.Rang.Start , _yAxis.Rang.Start + newHeight);

    }

    public void DrawGraph(GraphInfo graph , bool ForceFoucase = false)
    {
        if(graph.GraphType == GraphType.TimeGraph)
        {
            DateTime startedDate = (DateTime)graph.StartedDate;
            // Generate location based on Time
            foreach (var point in graph.Points)
                point.XCoordinate = ((DateTime)point.Date - startedDate).Days;
           
        }
        if (ForceFoucase || _FirstGraphRander)
        {
            _FirstGraphRander = false;
            var xMax = graph.Points.Max(p => p.XCoordinate);
            var xmin = graph.Points.Min(p => p.XCoordinate);

            var yMax = graph.Points.Max(p => p.YCoordinate);
            var ymin = graph.Points.Min(p => p.YCoordinate);

            _xAxis.Rang = (xmin, xMax);
            _yAxis.Rang = (ymin, yMax);
        }
        _canvas?.Clear();
        ShowGrid(graph.StartedDate);
        // Display Records
        foreach (var point in graph.Points)
        {
            DrawPoint(point.XCoordinate, point.YCoordinate);
        }
    }

    
}
