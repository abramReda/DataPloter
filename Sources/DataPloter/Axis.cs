namespace DataPloter;

using DataPloter.Constans;
using SkiaSharp;
using System.Linq;

public class Axis
{
    public Axis()
    {

    }
    public Axis(SKCanvas canvas, SKImageInfo info)
    {
        Init(canvas, info);

    }

    public void Init(SKCanvas canvas, SKImageInfo info)
    {
        _canvas = canvas;
        AxisWidth = (int)(info.Width * 0.8);
        AxisHeight = (int)(info.Height * 0.8);

        _canvas.Translate((int)(info.Width * 0.1), (int)(info.Height * 0.1));
    }
    private float PixelPerUintX => AxisWidth / TotalXUnite;
    private float PixelPerUintY => AxisHeight / TotalYUnite;
    /// <summary>
    /// Width of  Axis in pixel
    /// </summary>
    private int AxisWidth { get; set; }
    /// <summary>
    /// Height of Axis in pixel
    /// </summary>
    private int AxisHeight { get; set; }

    /// <summary>
    /// start point to end poind in Phsycal unit on X Axiss thet need to show
    /// </summary>
    public (float Start, float End) XRang { get; set; } = (0f, 10f);

    /// <summary>
    /// totlal phsyical unit needed in x axis
    /// </summary>
    public float TotalXUnite => XRang.End - XRang.Start;

    /// <summary>
    /// start point to end poind in Phsycal unit on Y Axiss thet need to show
    /// </summary>
    public (float Start, float End) YRang { get; set; } = (0f, 20f);
    /// <summary>
    /// totlal phsyical unit needed in y axis
    /// </summary>
    public float TotalYUnite => YRang.End - YRang.Start;

    /// <summary>
    /// Get XStep
    /// </summary>
    /// <remarks>
    /// Distanse Between Vertical line on the Grid
    /// </remarks>
    public float XStep => TotalXUnite / 10f;

    

    /// <summary>
    /// Get YStep
    /// </summary>
    /// <remarks>
    /// Distanse Between Herozontal line on the Grid
    /// </remarks>
    public float YStep => TotalYUnite / 10f;

    readonly SKPaint _axisPaint = new ()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColor.Parse("#767676")
    };
    readonly SKPaint _mainAxisPaint = new()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Black,
        StrokeWidth = 2
    };
    readonly SKPaint _pointPaint = new ()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Yellow
    };
    readonly SKPaint _textPaint = new ()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColors.Yellow,
        TextSize = 32,
    };
    private  SKCanvas? _canvas = null;

    public void ShowGrid()
    {
        // Draw Vertical Axis
        for (float XLoc = XRang.Start; XLoc <= XRang.End; XLoc += XStep)
        {
            DrawLine(XLoc, YRang.Start, XLoc, YRang.End,XLoc==XRang.Start?LineType.MainAxisline:LineType.GuideAxisline);
            WritText(XLoc.ToString("0.0").PadRight(4), XLoc, YRang.Start - 0.5f * YStep);
        }

        // Draw Herozontal Axis
        for (float Yloc = YRang.Start; Yloc <= YRang.End; Yloc += YStep)
        {
            DrawLine(XRang.Start, Yloc, XRang.End, Yloc, Yloc == YRang.Start ? LineType.MainAxisline : LineType.GuideAxisline);
            WritText(Yloc.ToString("0.0").PadRight(4), XRang.Start - 0.7f *XStep, Yloc);
        }
    }

    public void Translate(double totalX, double totalY)
    {
        Console.WriteLine($"X : {totalX}");
        Console.WriteLine($"y : {totalY}");
        var x = Pixel2XCoordinate((float)totalX);
        var y = Pixel2YCoordinate((float)totalY);
        Console.WriteLine($"Converted x : {x}");
        XRang = (XRang.Start - x, XRang.End - x);
        YRang = (YRang.Start - y, YRang.End - y);
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
    public void DrawLine(float x0, float y0, float x1, float y1,LineType lineType=LineType.StraightLine)
    {
        // ToDo implement the line Type
        var _x0 = XCordinate2Pixel(x0);
        var _x1 = XCordinate2Pixel(x1);
        var _y0 = YCordinate2Pixel(y0);
        var _y1 = YCordinate2Pixel(y1);
        var paint = lineType switch
        {
            LineType.MainAxisline => _mainAxisPaint,
            _ => _axisPaint,
        };
        _canvas?.DrawLine(_x0, _y0, _x1, _y1, paint);
    }

    /// <summary>
    /// Drow a circule point
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="r">redious of point</param>
    public void DrawPoint(float x, float y,float r=16f)
    {
        if (!isInRange(x, XRang) || !isInRange(y,YRang)) return;
        var _x = XCordinate2Pixel(x);
        var _y = YCordinate2Pixel(y);
        _canvas?.DrawCircle(_x, _y, r, _pointPaint);
    }

    /// <summary>
    /// writ text in cordenat
    /// </summary>
    /// <param name="str"> string you want to add in axis</param>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public void WritText(string str, float x, float y)
    {
        var _x = XCordinate2Pixel(x);
        var _y = YCordinate2Pixel(y);
        _canvas?.DrawText(str, _x, _y, _textPaint);

    }

    public void ZoomInOut(double scale)
    {
        float newWidth = (float)(TotalXUnite * scale);
        float newHeight = (float)(TotalYUnite * scale);
        //var dw = (newWidth - TotalXUnite) / 2;
        //var dh = (newHeight - TotalYUnite) / 2;

        //XRang = (XRang.Start - dw , XRang.End + dw);
        //YRang = (YRang.Start - dh, YRang.End + dh);

        XRang = (XRang.Start , XRang.Start + newWidth);
        YRang = (YRang.Start , YRang.Start + newHeight);

    }

    public void DrawGraph(GraphInfo graph)
    {
        var xMax = graph.Points.Max(p => p.XCoordinate);
        var xmin = graph.Points.Min(p => p.XCoordinate);

        var yMax = graph.Points.Max(p => p.YCoordinate);
        var ymin = graph.Points.Min(p => p.YCoordinate);

        //XRang = (xmin, xMax);
        //YRang = (ymin, yMax);

        ShowGrid();
        foreach (var point in graph.Points)
        {
            DrawPoint(point.XCoordinate, point.YCoordinate);
        }
    }

    /// <summary>
    /// Transform X coordenate(distanse) to Pixel World
    /// </summary>
    /// <param name="cordinate">x Coordinate </param>
    /// <returns></returns>
    private int XCordinate2Pixel(float cordinate)
# warning need to use scale transform insted of subtract
    => (int)((cordinate - XRang.Start) * PixelPerUintX);

    private float Pixel2XCoordinate(float Pixel)
    => (Pixel/ PixelPerUintX);
    private float Pixel2YCoordinate(float Pixel)
    => (Pixel / PixelPerUintY);
    private int YCordinate2Pixel(float cordinate)
# warning need to use scale transform insted of subtract
    => (int)((cordinate - YRang.Start) * PixelPerUintY);

    private bool isInRange(float value,(float start,float end) Range)=> value >=Range.start && value <= Range.end;

    private float GetInRange(float value,(float start,float end) Range)
    {
        return Math.Min(Range.end, Math.Max(value, Range.start));
    }
    
}
