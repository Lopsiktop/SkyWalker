using Microsoft.EntityFrameworkCore;
using SkyWalker.Data;
using SkyWalker.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SkyWalker.Windows;

public partial class Cabinet : Window
{
    private readonly User? _user = null;

    public Cabinet(User user)
    {
        InitializeComponent();

        _user = user;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        Environment.Exit(0);
    }

    private void CreateTransportClick(object sender, RoutedEventArgs e)
    {
        var identifier = IdentifierBox.Text;
        var model = Model.Text;
        var color = Color.Text;
        var hourPrice = decimal.Parse(HourPrice.Text);
        var dayPrice = decimal.Parse(DayPrice.Text);

        var station = StationsBox.SelectedItem as Station;

        var transport = new Transport
        {
            OwnerId = _user.Id,
            StationId = station.Id,
            Identifier = identifier,
            Color = color,
            Model = model,
            HourPrice = hourPrice,
            DayPrice = dayPrice
        };

        var context = new SkyDbContext();
        context.Transports.Add(transport);
        context.SaveChanges();

        transport.Station = station;
        TransportsData.Items.Add(transport);

        MessageBox.Show("Транспорт успешно создан!");
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var context = new SkyDbContext();
        foreach (var station in context.Stations) 
        {
            StationsBox.Items.Add(station);
        }

        var own = context.Transports.Where(x => x.OwnerId == _user.Id);
        foreach (var transport in own)
            TransportsData.Items.Add(transport);
    }

    private void DeleteTransportClick(object sender, RoutedEventArgs e)
    {
        Transport? transport = TransportsData.SelectedItem as Transport;
        if (transport is null)
            return;

        var context = new SkyDbContext();
        context.Transports.Remove(transport);
        context.SaveChanges();

        TransportsData.Items.Remove(transport);
    }

    private void UpdateClick(object sender, RoutedEventArgs e)
    {
        Transport? transport = TransportsData.SelectedItem as Transport;
        if (transport is null)
            return;

        var edit = new UpdateTransportWindow(transport);
        edit.ShowDialog();
    }
}
