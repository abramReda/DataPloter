using DataPloter.Constans;
using SkiaSharp;

namespace DataPloter;

public class GraphInfo
{
    public GraphInfo(GraphType graphType = GraphType.MathGraph,DateTime? startDate = null)
    {
        GraphType = graphType;
        if(graphType == GraphType.TimeGraph && startDate == null)
            throw new ArgumentNullException(nameof(startDate), "TimeGraph should has a refrance start date");
        this.StartedDate = startDate;
    }
    public string GraphTitle { get; set; } = String.Empty;
    public string XLabel { get; set; } = string.Empty;
    public string YLabel { get; set; } = string.Empty;
    public List<GraphPoint> Points { get; set; } = new List<GraphPoint>();
    public DateTime? StartedDate { get; set; } = null;
    public SKColor GraphColor { get; set; } = SKColors.Black;
    public GraphType GraphType { get; }
}