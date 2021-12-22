using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using ImageSharp.Benchmarks.Benchmarks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageSharp.Benchmarks
{
    public partial class MainPage : ContentPage
    {
        public List<Type> Items { get; } = new List<Type>();

        public MainPage()
        {
            Items.Add(typeof(IntroBasic));
            Items.Add(typeof(LoadResizeSave));

            InitializeComponent();
            BindingContext = this;

            NavigationPage.SetBackButtonTitle(this, "Back");
        }

        void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            {
                return;
            }

            if (sender is CollectionView collectionView)
            {
                collectionView.SelectedItem = null;
            }

            if (e.CurrentSelection[0] is Type selectedType)
            {
                Navigation.PushAsync(new TestPage(selectedType));
                //Navigation.PushModalAsync(new NavigationPage(new TestPage(selectedType)));
            }
        }
    }
}
