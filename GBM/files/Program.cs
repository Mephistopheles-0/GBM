using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

class Program
{
    static void Main()
    {
        // Parameters for the GBM
        double initialPrice = 100.0;
        double drift = 0.1;
        double volatility = 0.2;
        double timeIncrement = 0.1;
        int numSteps = 100;

        // Generating GBM path
        double[] prices = GenerateGBM(initialPrice, drift, volatility, timeIncrement, numSteps);

        // Creating a new form for plotting
        var form = new Form();
        form.Text = "Geometric Brownian Motion Plot";

        // Creating a new PlotView
        var plotView = new OxyPlot.WindowsForms.PlotView();
        form.Controls.Add(plotView);
        plotView.Dock = DockStyle.Fill;

        // Creating a new LineSeries for the GBM path
        var series = new LineSeries();
        for (int i = 0; i < prices.Length; i++)
        {
            series.Points.Add(new DataPoint(i * timeIncrement, prices[i]));
        }

        // Creating a new PlotModel and adding the series
        var plotModel = new PlotModel();
        plotModel.Series.Add(series);

        // Setting the title and axes
        plotModel.Title = "Geometric Brownian Motion";
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Price" });

        // Setting the PlotModel for the PlotView
        plotView.Model = plotModel;

        // Displaying the form
        Application.Run(form);
    }

    static double[] GenerateGBM(double initialPrice, double drift, double volatility, double timeIncrement, int numSteps)
    {
        double[] prices = new double[numSteps];
        Random random = new Random();

        prices[0] = initialPrice;

        for (int i = 1; i < numSteps; i++)
        {
            double randomValue = random.NextDouble();
            double standardNormal = Math.Sqrt(-2.0 * Math.Log(randomValue)) * Math.Sin(2.0 * Math.PI * random.NextDouble());

            double deltaT = timeIncrement;
            double exponentialTerm = Math.Exp((drift - 0.5 * volatility * volatility) * deltaT + volatility * Math.Sqrt(deltaT) * standardNormal);
            prices[i] = prices[i - 1] * exponentialTerm;
        }

        return prices;
    }
}