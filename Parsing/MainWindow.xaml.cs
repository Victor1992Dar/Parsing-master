using HtmlAgilityPack;
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

namespace Parsing
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
        }
        Dictionary<string, string> CityName_urlWeather = new Dictionary<string, string>();

        private void StartParse_Click(object sender, RoutedEventArgs e)
		{
            UpdateBase("https://yandex.ru/pogoda/region/1");
        }

        public void UpdateBase(string urlRegion)
        {
            var startUrl = "https://yandex.ru";
            var addUrl = "";
            var urlWeather = "";

            var url = urlRegion; //Регион Москва и МО

            var web = new HtmlWeb();
            var doc = web.Load(url);

            for (int iterAZ = 3; iterAZ < 36; iterAZ++)
            {
                try
                {
                    for (int iterLeftRight = 1; iterLeftRight <= 2; iterLeftRight++)
                    {
                        for (int iterTopDown = 1; iterTopDown < 30; iterTopDown++)
                        {
                            var htmlNodes = doc.DocumentNode.SelectNodes($"/html/body/div/div[5]/div[{iterAZ}]/ul[{iterTopDown}]/li[{iterLeftRight}]/a"); //Горки в url
                            string CityName = " ";
                            foreach (var node in htmlNodes)
                            {
                                CityName = node.InnerText;
                                tbOut.Text += node.InnerText.ToString() + "\n"; //текст между <>
                                CityName_urlWeather.Add(node.InnerText, "");

                                addUrl = node.Attributes["href"].Value.ToString(); //Добавочная сылка на страницу погоды Горки
                                tbOut.Text += "Добавочная ссылка: " + addUrl + "\n";
                            }

                            urlWeather = startUrl + addUrl; //Полный адрес ссылки на погоду Горки      
                            CityName_urlWeather[CityName] = urlWeather;
                            tbOut.Text += "Полный адрес ссылки на Авсюнино: " + urlWeather + "\n";


                            //Работа с температурой на странице погоды Города.
                            var webWeather = new HtmlWeb();
                            var docWeather = webWeather.Load(urlWeather);

                            var tempNow = docWeather.DocumentNode.SelectNodes("/html/body/div[1]/div[6]/div[1]/div/div[2]/div[1]/div[5]/a//div[1]/span[1]"); //Температура
                            tbOut.Text += "Температура сейчас " + tempNow[0].InnerText + "\n";
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private void Bt_test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(CityName_urlWeather["Москва"]);
            MessageBox.Show(CityName_urlWeather["Красногорск"]);
            MessageBox.Show(CityName_urlWeather["Авсюнино"]);
        }
    }
}
