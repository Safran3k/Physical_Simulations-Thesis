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
using System.Windows.Shapes;

namespace Physical_Simulations.Windows
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        List<Pages> slides = new List<Pages>();
        int currentSlideIndex = 0;

        public InformationWindow()
        {
            InitializeComponent();
            InitializeSlides();
            DisplayCurrentVideo();
            Closed += InformationWindow_Closed;
        }

        private async void InformationWindow_Closed(object sender, EventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                await webView.CoreWebView2.ExecuteScriptAsync("document.querySelectorAll('video').forEach(video => video.pause());");
                webView.CoreWebView2.Navigate("about:blank");
                webView.CoreWebView2.Stop();
            }
        }

        private void InitializeSlides()
        {
            slides.Add(new Pages
            { VideoUrl = "https://www.youtube.com/watch?v=yAUFK2zmoM4", Description = "Information about Simulation" });
            slides.Add(new Pages
            { VideoUrl = "https://www.youtube.com/watch?v=1DKWg38WYks", Description = "Information about Simulation graph" });
            slides.Add(new Pages
            { VideoUrl = "https://www.youtube.com/watch?v=8BzfQHRRSVE", Description = "Information about Comparison simulation Part 1" });
            slides.Add(new Pages
            { VideoUrl = "https://www.youtube.com/watch?v=X49y7fxjhQQ", Description = "Information about Comparison simulation Part 2" });
        }

        private void DisplayCurrentVideo()
        {
            webView.Source = new Uri(slides[currentSlideIndex].VideoUrl);
            tbInformationText.Text = slides[currentSlideIndex].Description;
        }

        private void btnPageLeft_Click(object sender, RoutedEventArgs e)
        {
            if (currentSlideIndex > 0)
            {
                currentSlideIndex--;
                webView.CoreWebView2.Navigate("about:blank");
                webView.CoreWebView2.Stop();
                DisplayCurrentVideo();
            }
        }

        private void btnPageRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentSlideIndex < slides.Count - 1)
            {
                currentSlideIndex++;
                webView.CoreWebView2.Navigate("about:blank");
                webView.CoreWebView2.Stop();
                DisplayCurrentVideo();
            }
        }
    }

    public class Pages
    {
        public string VideoUrl { get; set; }
        public string Description { get; set; }
    }
}
