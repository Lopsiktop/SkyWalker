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

    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var context = new SkyDbContext();
        foreach (var station in context.Stations) 
        {
            StationsBox.Items.Add(station.Name);
        }
    }
}
