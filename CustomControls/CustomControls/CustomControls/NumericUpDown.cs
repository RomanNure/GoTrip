using System;

using Xamarin.Forms;

namespace CustomControls
{
    public class NumericUpDown : Grid
    {
        private const double DEFAULT_BUTTON_SIZE = 40;
        private const string DEFAULT_UP_SYMBOL = "+";
        private const string DEFAULT_DOWN_SYMBOL = "-";

        public delegate void Changed(NumericUpDown nud);

        public event Changed OnValueChanged;
        public event Changed OnBoundsChanged;

        private Entry ValueWrapper = new Entry() { Keyboard = Keyboard.Numeric, HorizontalOptions = LayoutOptions.FillAndExpand };
        private Button Up = new Button() { BackgroundColor = Color.Transparent, Text = DEFAULT_UP_SYMBOL };
        private Button Down = new Button() { BackgroundColor = Color.Transparent, Text = DEFAULT_DOWN_SYMBOL };

        public Style ValueWrapperStyle { get => ValueWrapper.Style; set { ValueWrapper.Style = value; } }
        public Style ButtonsStyle { get => Up.Style; set { Up.Style = value; Down.Style = value; } }

        private string upSymbol = DEFAULT_UP_SYMBOL;
        public string UpSymbol { get => upSymbol; set { upSymbol = value; Up.Text = upSymbol; } }

        private string downSymbol = DEFAULT_DOWN_SYMBOL;
        public string DownSymbol { get => downSymbol; set { downSymbol = value; Down.Text = downSymbol; } }

        public Color ButtonsBackgroundColor { get => Up.BackgroundColor; set { Up.BackgroundColor = value; Down.BackgroundColor = value; } }

        public NumericUpDown() : base()
        {
            ColumnSpacing = 0;
            RowSpacing = 0;

            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = Math.Max(DEFAULT_BUTTON_SIZE, HeightRequest / 2) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = Math.Max(DEFAULT_BUTTON_SIZE, HeightRequest / 2) });

            Children.Add(ValueWrapper, 0, 0);
            Children.Add(Down, 1, 0);
            Children.Add(Up, 2, 0);

            Up.Clicked += (sender, e) => Increase();
            Down.Clicked += (sender, e) => Decrease();

            ValueWrapper.Unfocused += (sender, e) => Val = Convert.ToDouble(ValueWrapper.Text);
            Update();
        }

        private double min = double.MinValue;
        public double Min
        {
            get { return min; }
            set
            {
                double oldMin = min;

                if (value <= max)
                    min = value;
                else
                {
                    min = max;
                    Max = value;
                }

                Val = Val;
                Update();

                if (min == oldMin)
                    return;

                OnBoundsChanged?.Invoke(this);
            }
        }

        private double max = double.MaxValue;
        public double Max
        {
            get { return max; }
            set
            {
                double oldMax = max;

                if (value >= min)
                    max = value;
                else
                {
                    max = min;
                    Min = value;
                }

                Val = Val;
                Update();

                if (max == oldMax)
                    return;

                OnBoundsChanged?.Invoke(this);
            }
        }

        private double val = 0;
        public double Val
        {
            get { return val; }
            set { double oldValue = val; val = getValidated(value); Update(); if (val != oldValue) OnValueChanged?.Invoke(this); }
        }

        private double delta = 1;
        public double Delta { get { return delta; } set { delta = value; } }

        private uint decimalPlaces = 0;
        public uint DecimalPlaces { get { return decimalPlaces; } set { decimalPlaces = value; Update(); } }

        private double getValidated(double x) => Math.Max(Math.Min(x, max), min);

        public void Increase() => Val = getValidated(Val + Delta);
        public void Decrease() => Val = getValidated(Val - Delta);

        private void Update()
        {
            ValueWrapper.Text = Math.Round(Val, (int)DecimalPlaces).ToString().Replace(",", ".");

            Up.IsEnabled = Val < max;
            Down.IsEnabled = Val > min;
        }
    }
}
