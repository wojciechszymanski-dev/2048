namespace _2048
{
    public partial class MainPage : ContentPage
    {
        Random rand;
        int[,] gridState;
        Grid innerGrid;

        public MainPage()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));
            outerGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            outerGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));

            outerGrid.AddRowDefinition(new RowDefinition(10));
            outerGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            outerGrid.AddRowDefinition(new RowDefinition(10));

            innerGrid = new Grid
            {
                WidthRequest = 800,
                HeightRequest = 800,
            };

            for (int i = 0; i < 6; i++)
            {
                innerGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                innerGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            outerGrid.Add(innerGrid, 1, 1);

            rand = new Random();
            gridState = new int[4, 4];
            int[] randomIndex = { rand.Next(0, 4), rand.Next(0, 4) };
            gridState[randomIndex[0], randomIndex[1]] = 2;

            UpdateUI();

            Grid navGrid = new Grid();

            navGrid.AddColumnDefinition(new ColumnDefinition(10));
            navGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            navGrid.AddColumnDefinition(new ColumnDefinition(10));
            navGrid.AddRowDefinition(new RowDefinition(10));
            navGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            navGrid.AddRowDefinition(new RowDefinition(10));
            outerGrid.Add(navGrid, 2, 1);

            VerticalStackLayout vStack = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            HorizontalStackLayout hStack = new();

            navGrid.Add(vStack, 1, 1);

            string[] navigationButtonTextArr = { "W", "A", "S", "D" };
            for (int x = 0; x < navigationButtonTextArr.Length; x++)
            {
                Button button = new Button
                {
                    WidthRequest = 100,
                    HeightRequest = 100,
                    Text = navigationButtonTextArr[x],
                    TextColor = Colors.Black,
                    CornerRadius = 10,
                    BackgroundColor = Color.FromArgb("#bbb"),
                };
                button.Clicked += (sender, e) =>
                {
                    NavigationButtonClicked(button.Text);
                };

                if (button.Text == "W") vStack.Children.Add(button);
                else
                {
                    hStack.Children.Add(button);
                }
            }
            vStack.Children.Add(hStack);
        }

        private void NavigationButtonClicked(string direction)
        {
            switch (direction)
            {
                case "W":
                    MoveUp();
                    break;
                case "S":
                    MoveDown();
                    break;
                case "A":
                    MoveLeft();
                    break;
                case "D":
                    MoveRight();
                    break;
            }
            UpdateUI();
        }

        private void MoveUp()
        {
            for (int col = 0; col < 4; col++)
            {
                for (int row = 1; row < 4; row++)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow > 0 && gridState[currentRow - 1, col] == 0)
                        {
                            gridState[currentRow - 1, col] = gridState[currentRow, col];
                            gridState[currentRow, col] = 0;
                            currentRow--;
                        }
                        if (currentRow > 0 && gridState[currentRow - 1, col] == gridState[currentRow, col])
                        {
                            gridState[currentRow - 1, col] *= 2;
                            gridState[currentRow, col] = 0;
                        }
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int col = 0; col < 4; col++)
            {
                for (int row = 2; row >= 0; row--)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow < 3 && gridState[currentRow + 1, col] == 0)
                        {
                            gridState[currentRow + 1, col] = gridState[currentRow, col];
                            gridState[currentRow, col] = 0;
                            currentRow++;
                        }
                        if (currentRow < 3 && gridState[currentRow + 1, col] == gridState[currentRow, col])
                        {
                            gridState[currentRow + 1, col] *= 2;
                            gridState[currentRow, col] = 0;
                        }
                    }
                }
            }
        }

        private void MoveLeft()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 1; col < 4; col++)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol > 0 && gridState[row, currentCol - 1] == 0)
                        {
                            gridState[row, currentCol - 1] = gridState[row, currentCol];
                            gridState[row, currentCol] = 0;
                            currentCol--;
                        }
                        if (currentCol > 0 && gridState[row, currentCol - 1] == gridState[row, currentCol])
                        {
                            gridState[row, currentCol - 1] *= 2;
                            gridState[row, currentCol] = 0;
                        }
                    }
                }
            }
        }

        private void MoveRight()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 2; col >= 0; col--)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol < 3 && gridState[row, currentCol + 1] == 0)
                        {
                            gridState[row, currentCol + 1] = gridState[row, currentCol];
                            gridState[row, currentCol] = 0;
                            currentCol++;
                        }
                        if (currentCol < 3 && gridState[row, currentCol + 1] == gridState[row, currentCol])
                        {
                            gridState[row, currentCol + 1] *= 2;
                            gridState[row, currentCol] = 0;
                        }
                    }
                }
            }
        }

        private void UpdateUI()
        {
            innerGrid.Children.Clear();
            for (int row = 1; row <= 4; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    Button button = new Button
                    {
                        BackgroundColor = Colors.AliceBlue,
                        CornerRadius = 10,
                        FontSize = 80,
                        Text = gridState[row - 1, col - 1] == 0 ? "" : gridState[row - 1, col - 1].ToString()
                    };
                    innerGrid.Add(button, col, row);
                }
            }
        }
    }
}
