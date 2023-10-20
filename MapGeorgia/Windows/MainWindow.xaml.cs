using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapGeorgia.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.Maps.MapControl.WPF;

namespace MapGeorgia
{
    /// <key>
    /// CXHi18FfJRErSX5z8Mme~uV00wpU2XqXgk_MFHQ1zqg~AjmdktsV4en7DI0DUd0mCieDaDhs7rfM4rkE31TPwlcvR9nmmme_OjnrlQvEMJte
    /// </key>
    public partial class MainWindow : Window
    {
        private bool _theme = false;
        private List<Place> Places { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Places = MapEntities.Place.ToList();
            DataLV.ItemsSource = Places;

            FilterCategoryStub();
        }

        public static MapEntitiesEntities MapEntities = new MapEntitiesEntities();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            PaletteHelper _paletteHelper = new PaletteHelper();
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = _theme ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);

            _theme = !_theme;
        }

        private void ItemBtn_Click(object sender, RoutedEventArgs e)
        {
            NavMap.Children.Clear();

            var btn = sender as Button;
            var id = Guid.Parse(btn.CommandParameter.ToString());
            var place = MapEntities.Place.FirstOrDefault(x => x.ID == id);

            Pushpin pushpin = new Pushpin();
            pushpin.ToolTip = new ToolTip() { Content = place.Name };
            pushpin.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkTurquoise);
            pushpin.Width = 30;
            pushpin.Height = 30;
            pushpin.Location =
                new Location((double)place.Latitude, (double)place.Longitude);

            NavMap.Children.Add(pushpin);
            NavMap.Center =
                new Location((double)place.Latitude, (double)place.Longitude); ;
        }

        private void FilterCategoryStub()
        {
            foreach (var category in MapEntities.Category.ToList())
            {
                FilterCategoryCB.Items.Add(category.Name);
            }
        }

        private void CancelFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            FilterCategoryCB.Text = string.Empty;
        }

        private void AddFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FilterCategoryCB.Text == string.Empty)
                return;

            if(FilterCategoryCB.Text == "None")
            {
                Places = MapEntities.Place.ToList();
                DataLV.ItemsSource = Places;
                FilterCategoryCB.Text = string.Empty;
                return;
            }

            var category = MapEntities.Category.FirstOrDefault(x => x.Name == FilterCategoryCB.Text);
            var filterCourses = Places;
            filterCourses = MapEntities.Place.Where(z => z.CategoryID == category.ID).ToList();
            DataLV.ItemsSource = filterCourses;
        }

        private void Filter()
        {
            var filterCourses = Places;

            if (SearchTB.Text != string.Empty)
            {
                filterCourses = MapEntities.Place.Where(z => (z.Name.Contains(SearchTB.Text))).ToList();
            }

            DataLV.ItemsSource = filterCourses;
            FilterCategoryCB.Text = string.Empty;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
    }
}
