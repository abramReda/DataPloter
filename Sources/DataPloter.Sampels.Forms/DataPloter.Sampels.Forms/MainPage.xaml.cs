using DataPloter.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataPloter.Sampels.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var data = new List<GraphPoint>();

            // create the 30 data for thegraph 
            for (int i = 0; i < 30; i++)
            {
                data.Add(new()
                {
                    XCoordinate = i,
                    YCoordinate = i
                });
            }
            graphView.GraphData = new()
            {
                Points = data,
                GraphColor = SKColors.Black,
                GraphTitle = "liner Graph"
            };
        }
    }
}
