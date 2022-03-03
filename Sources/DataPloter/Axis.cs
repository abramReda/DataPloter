namespace DataPloter;

using DataPloter.Constans;
using SkiaSharp;
using System.Linq;

public class Axis
{
    public Axis(SKCanvas canvas, SKImageInfo info)
    {
        _canvas = canvas;
        AxisWidth = (int)(info.Width * 0.8);
        AxisHeight = (int)(info.Height * 0.8);

        _canvas.Translate((int)(info.Width * 0.1), (int)(info.Height * 0.1));

    }
    private int PixelPerUintX => (int)(AxisWidth / TotalXUnite);
    private int PixelPerUintY => (int)(AxisHeight / TotalYUnite);
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
    public (float Start, float End) YRang { get; set; } = (0f, 10f);
    /// <summary>
    /// totlal phsyical unit needed in y axis
    /// </summary>
    public float TotalYUnite => YRang.End - YRang.Start;

    /// <summary>
    /// Step in X axis
    /// </summary>
    public int XStep { get; set; } = 1;

    /// <summary>
    /// Step in Y axis
    /// </summary>
    public int YStep { get; set; } = 1;

    readonly SKPaint _axisPaint = new ()
    {
        Style = SKPaintStyle.Fill,
        Color = SKColor.Parse("#767676")
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
        TextSize = 14,
    };
    private readonly SKCanvas _canvas;

    public void ShowGrid()
    {
        // Draw Vertical Axis
        for (float XLoc = XRang.Start; XLoc <= XRang.End; XLoc += XStep)
        {
            DrawLine(XLoc, YRang.Start, XLoc, YRang.End);
            WritText(XLoc.ToString().PadRight(3), XLoc, YRang.Start - 0.5f);
        }

        // Draw Herozontal Axis
        for (float Yloc = YRang.Start; Yloc <= YRang.End; Yloc += YStep)
        {
            DrawLine(XRang.Start, Yloc, XRang.End, Yloc);
            WritText(Yloc.ToString().PadRight(3), XRang.Start - 0.7f, Yloc);
        }
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
        _canvas.DrawLine(_x0, _y0, _x1, _y1, _axisPaint);
    }

    /// <summary>
    /// Drow a circule point
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="r">redious of point</param>
    public void DrawPoint(float x, float y,float r=16f)
    {
        var _x = XCordinate2Pixel(x);
        var _y = YCordinate2Pixel(y);
        _canvas.DrawCircle(_x, _y, r, _pointPaint);
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
        var w = _textPaint.MeasureText(str);
        _textPaint.TextSize = _textPaint.TextSize * PixelPerUintX / w;
        _canvas.DrawText(str, _x, _y, _textPaint);

    }

    public void DrawGraph(GraphInfo graph)
    {
        var xMax = graph.Points.Max(p => p.XCoordinate);
        var xmin = graph.Points.Min(p => p.XCoordinate);

        var yMax = graph.Points.Max(p => p.YCoordinate);
        var ymin = graph.Points.Min(p => p.YCoordinate);

        XRang = (xmin, xMax);
        YRang = (ymin, yMax);

        ShowGrid();
        foreach (var point in graph.Points)
        {
            DrawPoint(point.XCoordinate, point.YCoordinate);
        }
    }

    private int XCordinate2Pixel(float cordinate)
# warning need to use scale transform insted of subtract
    => (int)((cordinate - XRang.Start) * PixelPerUintX);

    private int YCordinate2Pixel(float cordinate)
# warning need to use scale transform insted of subtract
    => (int)((cordinate - YRang.Start) * PixelPerUintY);
}
