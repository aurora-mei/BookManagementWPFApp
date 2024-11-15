using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;
using Apitron.PDF.Rasterizer.Navigation;
using Microsoft.Win32;
using BookManagementWPFApp.ViewModels;
using Rectangle = Apitron.PDF.Rasterizer.Rectangle;
using BookManagement.BusinessObjects;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public DocumentVM document;

        private const int GlobalScale = 2;

        private Rectangle destinationRectangle;

        public ReadWindow(Book book)
        {
            InitializeComponent();
            document = new DocumentVM();
            DataContext = document;
            OnOpenFileClick(book.BookPDFLink);
        }

        //Open the book
        private void OnOpenFileClick(string bookLink)
        {
            if (bookLink != null)
            {
                //MessageBox.Show($"{dialog.FileName}");
                var document = new Document(new FileStream(bookLink, FileMode.Open, FileAccess.Read));
                this.document.Document = document;
            }

            UpdatePageView();
        }

        private async void UpdatePageView()
        {
            // Show the "Loading..." message and disable UI elements
            LoadingText.Visibility = Visibility.Visible;
            ListView.Visibility = Visibility.Collapsed;
            ToolBar.Visibility = Visibility.Collapsed;

            try
            {
                if (PagesControl.Items == null)
                {
                    PagesControl.Items.Clear();
                }


                foreach (var page in this.document.Pages)
                {
                    var desiredWidth = (int)page.Width * GlobalScale;
                    var desiredHeight = (int)page.Height * GlobalScale;

                    // I hate task
                    var images = await Task.Run(() =>
                        page.RenderAsBytes(desiredWidth, desiredHeight, new RenderingSettings()));
                    var bitmap = BitmapSource.Create(desiredWidth, desiredHeight, 72, 72, PixelFormats.Bgra32, null,
                        images, desiredWidth * 4);

                    PagesControl.Items.Add(bitmap);
                }
            }
            finally
            {
                // Hide the "Loading..." message and re-enable UI elements
                LoadingText.Visibility = Visibility.Collapsed;
                ListView.Visibility = Visibility.Visible;
                ToolBar.Visibility = Visibility.Visible;
            }
        }

        //Zoom using slider value
        private void UpdateImageZoom()
        {
            Slider slider = this.ZoomSlider;
            if (PagesControl != null)
            {
                PagesControl.LayoutTransform = new ScaleTransform(slider.Value, slider.Value);
            }
        }

        private void UpdateViewLocation(int index)
        {
            if (index == 0)
            {
                PageScroller.ScrollToTop();
                return;
            }

            //MessageBox.Show($"Item: {PagesControl.Items.Count} Index: {index}");
            //Take all the heights (scale with zoom) of the previous page and offset the scroller by that value
            var scale = this.ZoomSlider.Value;
            var upperHeight = 0.0;
            for (var i = 0; i < index; i++)
            {
                var page = (BitmapSource)PagesControl.Items[i];
                upperHeight += page.Height * scale;
            }

            PageScroller.ScrollToVerticalOffset(upperHeight);
        }

        private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView? listView = sender as ListView;

            if (listView != null)
            {
                // Get the index of the selected item
                var selectedIndex = listView.SelectedIndex;

                var selectedItem = listView.SelectedItem;
                UpdateViewLocation(selectedIndex);
            }
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Jump to books mark of the book (if exist)
        private void OnBookmarkSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var newValue = (Bookmark)e.NewValue;
            if (newValue != null)
            {
                document.Document.Navigator.GoToBookmark(newValue);
                destinationRectangle = newValue.GetDestinationRectangle((int)(document.Page.Width * GlobalScale),
                    (int)(document.Page.Height * GlobalScale), null);
            }
        }

        //Page navigation
        private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            var source = (Button)e.Source;
            var doc = document.Document;
            var navigator = doc == null ? null : doc.Navigator;
            if (doc == null || navigator == null)
            {
                return;
            }

            switch ((string)source.CommandParameter)
            {
                case "Next":
                    navigator.MoveForward();
                    break;
                case "Prev":
                    navigator.MoveBackward();
                    break;
                case "First":
                    navigator.Move(0, Origin.Begin);
                    break;
                case "Last":
                    navigator.Move(0, Origin.End);
                    break;
                default:
                    return;
            }

            destinationRectangle = null;
        }

        private void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateImageZoom();
        }
    }
}