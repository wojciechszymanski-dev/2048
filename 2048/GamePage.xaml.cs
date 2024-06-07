namespace _2048
{
    public partial class GamePage : ContentPage
    {
        Random rand;
        int[,] gridState;
        Grid innerGrid;
        Dictionary<(int, int), Button> buttonMap;

        public GamePage()
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
            buttonMap = new Dictionary<(int, int), Button>();

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
            navGrid.HorizontalOptions = LayoutOptions.Start;
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

        private async void NavigationButtonClicked(string direction)
        {
            bool moved = false;

            switch (direction)
            {
                case "W":
                    moved = await MoveUp();
                    break;
                case "S":
                    moved = await MoveDown();
                    break;
                case "A":
                    moved = await MoveLeft();
                    break;
                case "D":
                    moved = await MoveRight();
                    break;
            }

            if (moved)
            {
                AddRandomNumber();
                UpdateUI();
            }

            if (IsGameOver())
            {
                await DisplayAlert("Game Over", "You lose!", "OK");
            }
        }

        private async Task<bool> MoveLeft()
        {
            bool moved = false;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 1; col < 4; col++)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol > 0 && (gridState[row, currentCol - 1] == 0 || gridState[row, currentCol - 1] == gridState[row, currentCol]))
                        {
                            if (gridState[row, currentCol - 1] == 0)
                            {
                                var button = GetButtonAt(row, currentCol);
                                gridState[row, currentCol - 1] = gridState[row, currentCol];
                                gridState[row, currentCol] = 0;
                                buttonMap[(row, currentCol - 1)] = button;
                                buttonMap.Remove((row, currentCol));

                                if (button != null)
                                {
                                    await button.TranslateTo(-button.Width, 0, 50);
                                    button.TranslationX = 0;
                                    SetButtonPosition(button, row, currentCol - 1);
                                }

                                currentCol--;
                                moved = true;
                            }
                            else if (gridState[row, currentCol - 1] == gridState[row, currentCol])
                            {
                                var button = GetButtonAt(row, currentCol);
                                gridState[row, currentCol - 1] *= 2;
                                gridState[row, currentCol] = 0;
                                buttonMap.Remove((row, currentCol));

                                if (button != null)
                                {
                                    await button.TranslateTo(-button.Width, 0, 50);
                                    button.TranslationX = 0;
                                    SetButtonPosition(button, row, currentCol - 1);
                                }

                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private async Task<bool> MoveUp()
        {
            bool moved = false;

            for (int col = 0; col < 4; col++)
            {
                for (int row = 1; row < 4; row++)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow > 0 && (gridState[currentRow - 1, col] == 0 || gridState[currentRow - 1, col] == gridState[currentRow, col]))
                        {
                            if (gridState[currentRow - 1, col] == 0)
                            {
                                var button = GetButtonAt(currentRow, col);
                                gridState[currentRow - 1, col] = gridState[currentRow, col];
                                gridState[currentRow, col] = 0;
                                buttonMap[(currentRow - 1, col)] = button;
                                buttonMap.Remove((currentRow, col));

                                if (button != null)
                                {
                                    await button.TranslateTo(0, -button.Height, 50);
                                    button.TranslationY = 0;
                                    SetButtonPosition(button, currentRow - 1, col);
                                }

                                currentRow--;
                                moved = true;
                            }
                            else if (gridState[currentRow - 1, col] == gridState[currentRow, col])
                            {
                                var button = GetButtonAt(currentRow, col);
                                gridState[currentRow - 1, col] *= 2;
                                gridState[currentRow, col] = 0;
                                buttonMap.Remove((currentRow, col));

                                if (button != null)
                                {
                                    await button.TranslateTo(0, -button.Height, 50);
                                    button.TranslationY = 0;
                                    SetButtonPosition(button, currentRow - 1, col);
                                }

                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private async Task<bool> MoveDown()
        {
            bool moved = false;

            for (int col = 0; col < 4; col++)
            {
                for (int row = 2; row >= 0; row--)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow < 3 && (gridState[currentRow + 1, col] == 0 || gridState[currentRow + 1, col] == gridState[currentRow, col]))
                        {
                            if (gridState[currentRow + 1, col] == 0)
                            {
                                var button = GetButtonAt(currentRow, col);
                                gridState[currentRow + 1, col] = gridState[currentRow, col];
                                gridState[currentRow, col] = 0;
                                buttonMap[(currentRow + 1, col)] = button;
                                buttonMap.Remove((currentRow, col));

                                if (button != null)
                                {
                                    await button.TranslateTo(0, button.Height, 50);
                                    button.TranslationY = 0;
                                    SetButtonPosition(button, currentRow + 1, col);
                                }

                                currentRow++;
                                moved = true;
                            }
                            else if (gridState[currentRow + 1, col] == gridState[currentRow, col])
                            {
                                var button = GetButtonAt(currentRow, col);
                                gridState[currentRow + 1, col] *= 2;
                                gridState[currentRow, col] = 0;
                                buttonMap.Remove((currentRow, col));

                                if (button != null)
                                {
                                    await button.TranslateTo(0, button.Height, 50);
                                    button.TranslationY = 0;
                                    SetButtonPosition(button, currentRow + 1, col);
                                }

                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private async Task<bool> MoveRight()
        {
            bool moved = false;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 2; col >= 0; col--)
                {
                    if (gridState[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol < 3 && (gridState[row, currentCol + 1] == 0 || gridState[row, currentCol + 1] == gridState[row, currentCol]))
                        {
                            if (gridState[row, currentCol + 1] == 0)
                            {
                                var button = GetButtonAt(row, currentCol);
                                gridState[row, currentCol + 1] = gridState[row, currentCol];
                                gridState[row, currentCol] = 0;
                                buttonMap[(row, currentCol + 1)] = button;
                                buttonMap.Remove((row, currentCol));

                                if (button != null)
                                {
                                    await button.TranslateTo(button.Width, 0, 50);
                                    button.TranslationX = 0;
                                    SetButtonPosition(button, row, currentCol + 1);
                                }

                                currentCol++;
                                moved = true;
                            }
                            else if (gridState[row, currentCol + 1] == gridState[row, currentCol])
                            {
                                var button = GetButtonAt(row, currentCol);
                                gridState[row, currentCol + 1] *= 2;
                                gridState[row, currentCol] = 0;
                                buttonMap.Remove((row, currentCol));

                                if (button != null)
                                {
                                    await button.TranslateTo(button.Width, 0, 50);
                                    button.TranslationX = 0;
                                    SetButtonPosition(button, row, currentCol + 1);
                                }

                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }


        private Button GetButtonAt(int row, int col)
        {
            buttonMap.TryGetValue((row, col), out var button);
            return button;
        }

        private void SetButtonPosition(Button button, int row, int col)
        {
            Grid.SetRow(button, row + 1);
            Grid.SetColumn(button, col + 1);
        }

        private void AddRandomNumber()
        {
            List<(int, int)> emptyCells = new List<(int, int)>();

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (gridState[row, col] == 0)
                    {
                        emptyCells.Add((row, col));
                    }
                }
            }

            if (emptyCells.Count > 0)
            {
                var (row, col) = emptyCells[rand.Next(emptyCells.Count)];
                gridState[row, col] = rand.Next(0, 10) == 0 ? 4 : 2;
            }
        }

        private Dictionary<int, Color> valueColorMap = new Dictionary<int, Color>();

        private void UpdateUI()
        {
            innerGrid.Children.Clear();
            buttonMap.Clear();
            for (int row = 1; row < 5; row++)
            {
                for (int col = 1; col < 5; col++)
                {
                    int value = gridState[row - 1, col - 1];

                    Color backgroundColor = GetValueColor(value);

                    Button button = new Button
                    {
                        BackgroundColor = backgroundColor,
                        CornerRadius = 10,
                        FontSize = 40,
                        TextColor = Colors.Black,
                        Text = value == 0 ? "" : value.ToString()
                    };
                    innerGrid.Add(button, col, row);
                    buttonMap[(row - 1, col - 1)] = button;
                }
            }
        }

        private Color GetValueColor(int value)
        {
            if (!valueColorMap.ContainsKey(value))
            {
                Random rand = new Random(value);
                byte[] rgb = new byte[3];
                rand.NextBytes(rgb);
                Color randomColor = Color.FromRgb(rgb[0], rgb[1], rgb[2]);

                valueColorMap[value] = randomColor;
            }

            return valueColorMap[value];
        }

        private bool IsGameOver()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (gridState[row, col] == 0)
                    {
                        return false;
                    }
                }
            }

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (gridState[row, col] == gridState[row, col + 1])
                    {
                        return false;
                    }
                }
            }

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (gridState[row, col] == gridState[row + 1, col])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
