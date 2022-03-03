
namespace DataPloter.Views;


using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

public class GraphView : SKCanvasView
{

    public GraphView()
    {
        this.PaintSurface += new EventHandler<SKPaintSurfaceEventArgs>(this.OnCanvasViewPaintSurface);
    }

    public static readonly BindableProperty GraphProperty = BindableProperty.Create(
        nameof(GraphData),
        typeof(GraphInfo),
        typeof(GraphView),
        null,
        BindingMode.OneWay,
        propertyChanged: ChartDataPropertyChanged);


    private static void ChartDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as GraphView)?.InvalidateSurface();
    }

    public GraphInfo GraphData
    {
        get
        {
            return (GraphInfo)GetValue(GraphView.GraphProperty);
        }
        set
        {
            SetValue(GraphView.GraphProperty,value);
        }
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        SKImageInfo info = args.Info;
        SKCanvas canvas = args.Surface.Canvas;

        canvas.Clear();

        Axis axis = new(canvas, info)
        {
            YRang = (0, 10),
            XRang = (100, 120),
            XStep = 3,
            YStep = 1,
        };

        // axis.ShowGrid();
        axis.DrawGraph(GraphData);
    }

}
