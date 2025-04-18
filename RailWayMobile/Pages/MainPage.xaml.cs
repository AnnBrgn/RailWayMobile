using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RailWayMobile.Pages;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public string From
    {
        get
        {
            return _from;
        }
        set
        {
            _from = value;
            OnPropertyChanged();
        }
    }
    private string _from;
    public string Where { get; set; }
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
