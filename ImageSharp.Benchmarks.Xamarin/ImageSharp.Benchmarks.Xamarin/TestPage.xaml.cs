using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageSharp.Benchmarks.Xamarin
{
    public partial class TestPage : ContentPage
    {
        AsyncCommand _runCommand;
        public AsyncCommand RunCommand => _runCommand ??= new AsyncCommand(RunAsync, allowsMultipleExecutions: false);

        AsyncCommand _shareCommand;
        public AsyncCommand ShareCommand => _shareCommand ??= new AsyncCommand(ShareAsync, allowsMultipleExecutions: false);

        bool _canShare = false;
        public bool CanShare
        {
            get { return _canShare; }
            set
            {
                if (_canShare != value)
                {
                    _canShare = value;
                    OnPropertyChanged();
                }
            }
        }

        bool _canGoBack = true;
        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                if (_canGoBack != value)
                {
                    _canGoBack = value;
                    OnPropertyChanged();

                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        NavigationPage.SetHasBackButton(this, value);
                    }
                }
            }
        }

        Type _benchmarkType;

        public TestPage(Type benchmarkType)
        {
            _benchmarkType = benchmarkType;

            InitializeComponent();
            BindingContext = this;

            Title = benchmarkType.Name;

            // Don't show back button for Android.
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                NavigationPage.SetHasBackButton(this, false);
            }
        }

        bool _originalKeepScreenOn;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _originalKeepScreenOn = DeviceDisplay.KeepScreenOn;
            DeviceDisplay.KeepScreenOn = true;
        }

        protected override void OnDisappearing()
        {
            DeviceDisplay.KeepScreenOn = _originalKeepScreenOn;

            base.OnDisappearing();
        }

        async Task RunAsync()
        {
            CanShare = false;
            CanGoBack = false;
            OutputEditor.Text = "Running...";
            try
            {
                var logger = new AccumulationLogger();
                await Task.Run(() =>
                {
                    /*
                    var config = default(IConfig);
#if DEBUG
                    config = new DebugInProcessConfig();
#endif
                    */

                    var artifactsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    
                    var config = ManualConfig.CreateEmpty()
                        .AddColumnProvider(DefaultColumnProviders.Instance)
                        .AddLogger(ConsoleLogger.Default);
                    config.ArtifactsPath = artifactsPath;

                    var summary = BenchmarkRunner.Run(_benchmarkType, config);

                    MarkdownExporter.Console.ExportToLog(summary, logger);
                    ConclusionHelper.Print(logger,
                            summary.BenchmarksCases
                                   .SelectMany(benchmark => benchmark.Config.GetCompositeAnalyser().Analyse(summary))
                                   .Distinct()
                                   .ToList());
                });

                System.Diagnostics.Debug.WriteLine(logger.GetLog());

                OutputEditor.Text = logger.GetLog();
                CanShare = true;
            }
            catch (Exception err)
            {
                OutputEditor.Text = $"Error: {err.Message}";
                await DisplayAlert("Error", err.Message, "Ok");
            }
            finally
            {
                CanGoBack = true;
            }
        }


        async Task ShareAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = OutputEditor.Text,
                Title = "Share Results"
            });
        }


        protected override bool OnBackButtonPressed()
        {
            if (CanGoBack)
            {
                Navigation.PopAsync();
            }
            return true;
        }

    }
}
