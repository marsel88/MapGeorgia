using MapGeorgia.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using swf = System.Windows.Forms;

namespace MapGeorgia.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private byte[] _imgArr;
        private List<Place> Places { get; set; }

        public AdminWindow()
        {
            InitializeComponent();

            DataContext = this;
            Places = MapEntities.Place.ToList();
            DataLV.ItemsSource = Places;

            StubCB();
            
        }

        public static bool IsDelete = false;
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

        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Place photo | *.jpg;";

            if ((bool)o.ShowDialog())
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(o.FileName, UriKind.Relative);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                GetIMGFolder(bitmapImage);
            }
        }

        private void GetIMGFolder(BitmapImage bi)
        {
            PngBitmapEncoder pe = new PngBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            StringBuilder sb = new StringBuilder();
            pe.Frames.Add(BitmapFrame.Create(bi));
            pe.Save(ms);
            _imgArr = ms.ToArray();
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = MapEntities.Category.FirstOrDefault(i => i.Name == CategoryCB.Text);

                Place place = new Place()
                {
                    ID = Convert.ToInt32(Guid.NewGuid()),
                    Name = NameTB.Text,
                    Description = DescriptionTB.Text,
                    Rating = RatingTB.Text,
                    CategoryID = c.ID,
                    Longitude = Convert.ToDouble(PositionXTB.Text.Trim()),
                    Latitude = Convert.ToDouble(PositionYTB.Text.Trim()),
                    Photo = _imgArr
                };

                MapEntities.Place.Add(place);
                MapEntities.SaveChanges();

                Places = MapEntities.Place.ToList();
                DataLV.ItemsSource = Places;

                AlertCard.Opacity = 1;
                ClearTools();
            }
            catch
            {
                new AlertWindow("Invalid data. Try again").ShowDialog();
            }
        }

        private void ClearItemBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearTools();
        }

        private void ClearTools()
        {
            NameTB.Text = String.Empty;
            DescriptionTB.Text = String.Empty;
            RatingTB.Text = String.Empty;
            PositionXTB.Text = String.Empty;
            PositionYTB.Text = String.Empty;
            CategoryCB.Text = String.Empty;

            _imgArr = null;
        }

        private void StubCB()
        {
            foreach(var item in MapEntities.Category.ToList()) 
            {
                CategoryCB.Items.Add(item.Name);
            }
        }

        private void DelElBtn_Click(object sender, RoutedEventArgs e)
        {
            var ourBtn = sender as Button;
            var id = Guid.Parse(ourBtn.CommandParameter.ToString());

            var place = MapEntities.Place.FirstOrDefault(x => Convert.ToInt32(x.ID) == id);

            new AlertWindow("This element was deleted?", true).ShowDialog();

            if (place != null)
            {
                MapEntities.Place.Remove(place);
                MapEntities.SaveChanges();

                Places = MapEntities.Place.ToList();
                DataLV.ItemsSource = Places;
            }
            else
            {
                new AlertWindow().ShowDialog();
            }
        }

        private void EditElBtn_Click(object sender, RoutedEventArgs e)
        {
            var ourBtn = sender as Button;
            var id = Guid.Parse(ourBtn.CommandParameter.ToString());

            var place = MapEntities.Place.FirstOrDefault(x => x.ID == id);

            if( place != null)
            {
                NameTB.Text = place.Name;
                DescriptionTB.Text = place.Description;
                RatingTB.Text = place.Rating;
                PositionXTB.Text = place.Latitude.ToString();
                PositionYTB.Text = place.Longitude.ToString();
                CategoryCB.Text = place.Category.Name;
            }
        }
    }
}
