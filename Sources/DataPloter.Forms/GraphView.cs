
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

    readonly Axis _axis;
    public GraphView()
    {
        PaintSurface += new EventHandler<SKPaintSurfaceEventArgs>(OnCanvasViewPaintSurface);
        // adding pinch Gesture for zooming
        var pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        GestureRecognizers.Add(pinchGesture);

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
        _axis = new Axis();
        
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

        _axis.Init(canvas, info);

        // axis.ShowGrid();
        _axis.DrawGraph(GraphData);
    }

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Running)
        {
            Console.WriteLine($"In the Zoom {e.Scale} scale");
            _axis.ZoomInOut(2-e.Scale);
            InvalidateSurface();

        }
    }

    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                _axis.Translate(e.TotalX/20, e.TotalY/20);
                Console.WriteLine("Translate the axis");
                InvalidateSurface();
                break;
        }
    }

}
