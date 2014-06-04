using RestSharp.Portable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Result> Data = new List<Result>();
        List<string> Programs = new List<string>();
        List<Result> Enrollment = new List<Result>();
        //List<Result> Gender = new List<Result>();
        string Campus;
        string Year = "Calendario Académico 09-10";

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Campus = "RIOPIEDRAS";
            DisplayLoading();
            //lblLoad.Text = "";
            //lblLoad.Text = "Loading...";
            GetData(Campus);
                
        }

        private async void GetData(string campus)
        {
            bool done = false;
            int page=0;
            int count = 1;
            while (count <= 1)
            {
                var client = new RestClient(new Uri("https://data.pr.gov/resource/"));
                var request = new RestRequest("admit.json", HttpMethod.Get);
                //request.AddParameter("$limit", 1000);
                //request.AddParameter("$offset", 0);
                request.AddParameter("campus", campus);
                request.AddParameter("calendario", Year);
                var result = await client.Execute<List<Result>>(request);
                Data=(result.Data);
                count = count + 1;
                page = Data.Count;
                result.ToString();
                
            }
            slcCampus.Items.Clear();
            foreach (Result result in Data)
            {
                if (Programs.Count > 0)
                {
                    bool exists = false;
                    foreach (string program in Programs)
                    {
                        if (program == result.program)
                        {
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        Programs.Add(result.program);
                        slcCampus.Items.Add(result.program);
                    }
                }
                else
                {
                    Programs.Add(result.program);
                    Programs[0].ToString();
                }
                slcCampus.SelectedIndex = -1;
                HideLoading();
            }
            /*foreach (Result result in Data)
            {
                if (result.campus == "RIOPIEDRAS")
                {
                    Rio.Add(result);
                }
                if (result.campus == "PONCE")
                {
                    Ponce.Add(result);
                }
                if (result.campus == "BAYAMON")
                {
                    Baya.Add(result);
                }
            }*/
            
        }

        private void btnSanJuan_Click(object sender, RoutedEventArgs e)
        {
            Campus = "RIOPIEDRAS";
            lblVisual.Text = "Visualizando: San Juan";
            DisplayLoading();
            GetData(Campus);
        }

        private void slcCampus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            float enrollment = 0;
            float males = 0;
            float females = 0;
            float percentE = 0;
            float percentM = 0;
            float percentF = 0;

            foreach (Result result in Data)
            {
                if (slcCampus.SelectedItem != null)
                {
                    if (result.program == slcCampus.SelectedItem.ToString() && slcCampus.SelectedIndex != -1)
                    {
                        enrollment++;
                        if (result.genero == "Femenino")
                        {
                            females++;
                        }
                        if (result.genero == "Masculino")
                        {
                            males++;
                        }
                    }
                }
            }

            percentE = ((enrollment / (float)(Data.Count)));
            percentM = ((males /enrollment));
            percentF = ((females / enrollment));
            lblEnrollment.Text = percentE.ToString("P");
            lblFemales.Text = percentF.ToString("P");
            lblMales.Text = percentM.ToString("P");
        }

        public void DisplayLoading()
        {
            lblLoad.Text = "LOADING ICON HERE";
        }

        public void HideLoading()
        {
            lblLoad.Text = "";
        }

        private void btnBaya_Click(object sender, RoutedEventArgs e)
        {
            Campus = "BAYAMON";
            lblVisual.Text = "Visualizando: Bayamón";
            DisplayLoading();
            GetData(Campus);
        }

        private void btnPonce_Click(object sender, RoutedEventArgs e)
        {
            Campus = "PONCE";
            lblVisual.Text = "Visualizando: Ponce";
            DisplayLoading();
            GetData(Campus);
        }

        private void slcYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected;

            if (slcYear != null)
            {
                DisplayLoading();
                var selecteditem = (ComboBoxItem)slcYear.SelectedItem;
                var content = selecteditem.Content.ToString();
                Year = "Calendario Académico " + content;
                GetData(Campus);
            }

            
        }

        
    }
}
