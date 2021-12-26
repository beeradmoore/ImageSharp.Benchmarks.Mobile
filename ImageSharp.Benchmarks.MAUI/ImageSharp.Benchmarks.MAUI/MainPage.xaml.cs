namespace ImageSharp.Benchmarks.MAUI;

public partial class MainPage : ContentPage
{
	public List<Type> Items { get; } = new List<Type>();


	public MainPage()
	{
		Items.Add(typeof(ImageSharp.Benchmarks.MAUI.Benchmarks.IntroBasic));
		Items.Add(typeof(ImageSharp.Benchmarks.MAUI.Benchmarks.LoadResizeSave));

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

