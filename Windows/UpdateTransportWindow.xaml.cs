using SkyWalker.Data;
using SkyWalker.Models;
using System.ComponentModel;
using System.Windows;

namespace SkyWalker.Windows;

public partial class UpdateTransportWindow : Window
{
    private readonly Transport _transport;

    public UpdateTransportWindow(Transport transport)
    {
        InitializeComponent();

        _transport = transport;

        IdentifierBox.Text = transport.Identifier;
        Model.Text = transport.Model;
        Color.Text = transport.Color;
        HourPrice.Text = transport.HourPrice.ToString();
        DayPrice.Text = transport.DayPrice.ToString();
    }

    private void UpdateClick(object sender, RoutedEventArgs e)
    {
        var station = StationsBox.SelectedItem as Station;
        var identifier = IdentifierBox.Text;
        var model = Model.Text;
        var color = Color.Text;
        var hourPrice = decimal.Parse(HourPrice.Text);
        var dayPrice = decimal.Parse(DayPrice.Text);

        _transport.Station = station;
        _transport.Identifier= identifier;
        _transport.Model= model;
        _transport.HourPrice= hourPrice;
        _transport.DayPrice= dayPrice;
        _transport.Color= color;

        var context = new SkyDbContext();
        context.Update(_transport);
        context.SaveChanges();

        this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var context = new SkyDbContext();
        foreach (var station in context.Stations)
        {
            StationsBox.Items.Add(station);
            if(_transport.Station.Id == station.Id)
                StationsBox.SelectedItem = station;
        }
    }
}
