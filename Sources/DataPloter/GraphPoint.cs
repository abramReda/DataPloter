using SkiaSharp;

namespace DataPloter;

public class GraphPoint
{
    public float XCoordinate;
    public float YCoordinate;

    /// <summary>
    /// time of the point recorded
    /// </summary>
    public DateTime? Date = null;
    public SKColor Color { get; set; } = SKColors.Yellow;
}
