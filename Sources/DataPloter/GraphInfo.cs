using SkiaSharp;

namespace DataPloter;

public class GraphInfo
{
    public string GraphTitle { get; set; } = String.Empty;
    public string XLabel { get; set; } = string.Empty;
    public string YLabel { get; set; } = string.Empty;
    public List<GraphPoint> Points { get; set; } = new List<GraphPoint>();
    public SKColor GraphColor { get; set; } = SKColors.Black;

}