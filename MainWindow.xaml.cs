using Microsoft.Win32;
using Newtonsoft.Json;
using RestAI.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace RestAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagefiles (*.png;*.jpg)|*.png;*.jpg;*.jpeg| All files (*.*)| *.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if(openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(fileName));


                MakePredictionAsync(fileName);

            }
        }

        private async void MakePredictionAsync(string fileName)
        {
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/613f09c5-2d7e-4862-962d-ef85314566dd/image?iterationId=390e1bc8-a06b-45cb-bfd6-e4b35e09015e";
            string Prediction_Key = "ef9b8a6782c8487abbcac5f4fc0a94a6";
            string Content_Type = "application/octet-stream";
            var file = File.ReadAllBytes(fileName);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Prediction-Key", Prediction_Key);

                using (var content = new ByteArrayContent(file))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    var response = await httpClient.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    List<Prediction> predictions = (JsonConvert.DeserializeObject<CustomVision>(responseString)).Predictions;


                    predicitonsListView.ItemsSource = predictions; 
                }
            }


        } 
    }
}
