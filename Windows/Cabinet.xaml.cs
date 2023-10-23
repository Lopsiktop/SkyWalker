using Microsoft.EntityFrameworkCore;
using SkyWalker.Data;
using SkyWalker.Models;
using System;
using System.Collections.Generic;
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
        Application.Current.Shutdown();
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

        transport.Owner = _user;
        transport.Station = station;
        TransportsData.Items.Add(transport);

        // rent
        TransportBox.Items.Add(transport);

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

        // rent
        //todo: пожеланию добавить фио к user

        var transports = context.Transports.Include(x => x.Owner);
        foreach (var transport in transports)
        {
            TransportBox.Items.Add(transport);
        }

        var rents = context.Rents.Where(x => x.RenterId == _user.Id && x.Status == Status.Started).Include(x => x.Transport);
        foreach (var rent in rents)
        {
            MyRentsBox.Items.Add(rent);
        }

        var finishedRents = context.Rents.Where(x => x.RenterId == _user.Id && x.Status == Status.Ended).Include(x => x.Transport);
        foreach (var rent in finishedRents)
        {
            MyFinishedRentsBox.Items.Add(rent);
        }
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

        if(edit.DialogResult == true)
        {
            var index = TransportsData.SelectedIndex;
            TransportsData.Items.RemoveAt(TransportsData.SelectedIndex);
            TransportsData.Items.Insert(index, transport);
        }
    }

    private void NewRentClick(object sender, RoutedEventArgs e)
    {
        var rentTypeId = RentTypeBox.SelectedIndex;
        RentType? type = null;

        if (rentTypeId == 0)
            type = RentType.Hours;
        else if (rentTypeId == 1)
            type = RentType.Days;

        if(type == null)
        {
            MessageBox.Show("Выберите тип аренды!");
            return;
        }

        var transport = TransportBox.SelectedItem as Transport;
        if (transport is null)
        {
            MessageBox.Show("Выберите транспорт для аренды!");
            return;
        }

        var context = new SkyDbContext();
        var result = context.Rents.Where(x => x.TransportId == transport.Id)
            .All(x => x.Status == Status.Ended);

        if(result == false)
        {
            MessageBox.Show("Этот транспорт уже арендован!");
            return;
        }

        if (transport.OwnerId == _user.Id)
        {
            MessageBox.Show("Владелец не может арендовать свой же транспорт!");
            return;
        }

        if(transport.DayPrice == null && type == RentType.Days) 
        {
            MessageBox.Show("Выбранный тип аренды не поддерживается!");
            return;
        }

        if(transport.HourPrice == null && type == RentType.Hours)
        {
            MessageBox.Show("Выбранный тип аренды не поддерживается!");
            return;
        }

        decimal? price = 0;

        if (type == RentType.Days)
            price = transport.DayPrice;
        else if (type == RentType.Hours)
            price = transport.HourPrice;

        var rent = new Rent
        {
            RenterId = _user.Id,
            TransportId = transport.Id,
            CreatedAt = DateTime.Now,
            PriceOfUnit = (decimal)price!,
            RentType = (RentType)type,
            Status = Status.Started
        };

        context.Rents.Add(rent);
        context.SaveChanges();
        rent.Transport = transport;

        MyRentsBox.Items.Add(rent);

        MessageBox.Show("Вы успешно арендовали транспорт!");
    }

    private void SearchClick(object sender, RoutedEventArgs e)
    {
        var filter = SearchBox.Text;

        if (string.IsNullOrEmpty(filter))
            return;

        List<Transport> result = new List<Transport>();

        foreach (Transport item in TransportBox.Items)
        {
            if (item.Id.ToString() == filter)
            {
                result.Add(item);
                break;
            }

            if(item.Identifier.ToLower() == filter.ToLower())
            {
                result.Add(item);
                break;
            }

            if (item.Model.ToLower().Contains(filter.ToLower()))
                result.Add(item);
        }

        TransportBox.Items.Clear();

        foreach (var item in result)
        {
            TransportBox.Items.Add(item);
        }
    }

    private void EndRentClick(object sender, RoutedEventArgs e)
    {
        Rent rent = MyRentsBox.SelectedItem as Rent;
        rent.Status = Status.Ended;
        rent.EndedAt = DateTime.Now;

        TimeSpan? duration = rent.EndedAt - rent.CreatedAt;

        double? amount = null;
        if (rent.RentType == RentType.Days)
            amount = duration.Value.TotalDays;
        else if(rent.RentType == RentType.Hours)
            amount = duration.Value.TotalHours;

        amount = Math.Ceiling((double)amount);

        rent.FinalPrice = (decimal)amount * rent.PriceOfUnit;

        var context = new SkyDbContext();
        context.Rents.Update(rent);
        context.SaveChanges();

        MyRentsBox.Items.Remove(rent);
        MyFinishedRentsBox.Items.Add(rent);
    }

    private void UpdateTransportsClick(object sender, RoutedEventArgs e)
    {
        var context = new SkyDbContext();

        TransportBox.Items.Clear();
        var transports = context.Transports.Include(x => x.Owner).Include(x => x.Station);
        foreach (var transport in transports)
        {
            TransportBox.Items.Add(transport);
        }
    }
}
