namespace _2048
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            mainGrid.AddColumnDefinition(new ColumnDefinition(10));
            mainGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.AddColumnDefinition(new ColumnDefinition(10));

            mainGrid.AddRowDefinition(new RowDefinition(10));
            mainGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.AddRowDefinition(new RowDefinition(10));

            VerticalStackLayout mainStack = new VerticalStackLayout() 
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            mainGrid.Add(mainStack, 1, 1);

            Label label = new Label
            {
                Text = "2048",
                FontFamily = "Poppins",
                TextColor = Colors.White,
                FontSize = 200,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Button button = new Button
            {
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold, 
                Text = "Start",
                FontSize = 20,
                WidthRequest = 250,
                HeightRequest = 75,
                BackgroundColor = Colors.LimeGreen,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.Center,
            };
            button.Clicked += async (sender, e) => 
                await Navigation.PushAsync(new GamePage());

            mainStack.Children.Add(label);
            mainStack.Children.Add(button);
        }
       
    }
}
