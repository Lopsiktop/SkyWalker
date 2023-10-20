using SkyWalker.Models;
using System;
using System.ComponentModel;
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
}
