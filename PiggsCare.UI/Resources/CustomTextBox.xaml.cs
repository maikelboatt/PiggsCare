using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace PiggsCare.UI.Resources
{
    public partial class CustomTextBox:MvxWpfView
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(CustomTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(CustomTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data),
            typeof(string),
            typeof(CustomTextBox),
            new PropertyMetadata(default(string)));

        public CustomTextBox()
        {
            InitializeComponent();
        }

        public bool CheckEmpty { get; set; }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string Data
        {
            get => (string)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
