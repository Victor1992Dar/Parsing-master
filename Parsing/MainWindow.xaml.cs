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
        Dictionary<string, string> CityName_urlWeather = new Dictionary<string, string>(); //города
		Dictionary<string, string> AreaName_urlWeather = new Dictionary<string, string>(); //области

		private void StartParse_Click(object sender, RoutedEventArgs e)
		{
			UpdateAreaName_urlWeather("https://yandex.ru/pogoda/region/225");
			UpdateCityName_urlWeather(AreaName_urlWeather["Алтайский край"]);
		}

		public void UpdateCityName_urlWeather(string urlArea)
		#region Функция заполнения словаря Город:ссылка на погоду
		{
			var startUrl = "https://yandex.ru";
			var addUrl = "";
			var urlWeather = "";

			var url = urlArea; //Регион Москва и МО

			var web = new HtmlWeb();
			var doc = web.Load(url);

			for (int iterAZ = 3; iterAZ < 36; iterAZ++)
			{
				try
				{
					for (int iterLeftRight = 1; iterLeftRight < 3; iterLeftRight++)
					{
						try
						{
							for (int iterTopDown = 1; iterTopDown < 30; iterTopDown++)
							{
								var htmlNodes = doc.DocumentNode.SelectNodes($"/html/body/div/div[5]/div[{iterAZ}]/ul[{iterLeftRight}]/li[{iterTopDown}]/a"); //Горки в url
								string CityName = "";
								foreach (var node in htmlNodes)
								{
									CityName = node.InnerText;
									tbOut.Text += CityName.ToString() + "\n"; //текст между <>
									CityName_urlWeather.Add(CityName, "");

									addUrl = node.Attributes["href"].Value.ToString(); //Добавочная сылка на страницу погоды Горки
									tbOut.Text += "Добавочная ссылка: " + addUrl + "\n";
								}

								urlWeather = startUrl + addUrl; //Полный адрес ссылки на погоду Горки      
								CityName_urlWeather[CityName] = urlWeather;
								tbOut.Text += $"Полный адрес ссылки на {CityName}: " + urlWeather + "\n";

							}
						}
						catch
						{
							continue;
						}
					}
				}
				catch
				{
					continue;
				}
			}
		}
		#endregion
		public void UpdateAreaName_urlWeather(string urlCountry)
		#region Функция заполнения словаря Область:ссылка на область
		{
			var startUrl = "https://yandex.ru";
			var addUrl = "";
			var urlArea = "";

			var url = urlCountry; //Страна

			var web = new HtmlWeb();
			var doc = web.Load(url);

			for (int iterAZ = 3; iterAZ < 36; iterAZ++)
			{
				try
				{
					for (int iterLeftRight = 1; iterLeftRight < 3; iterLeftRight++)
					{
						try
						{
							for (int iterTopDown = 1; iterTopDown < 30; iterTopDown++)
							{
								var htmlNodes = doc.DocumentNode.SelectNodes($"/html/body/div/div[5]/div[{iterAZ}]/ul[{iterLeftRight}]/li[{iterTopDown}]/a"); //Область в url
								string AreaName = "";
								foreach (var node in htmlNodes)
								{
									AreaName = node.InnerText;
									tbOut.Text += AreaName.ToString() + "\n"; //текст между <>
									AreaName_urlWeather.Add(AreaName, "");

									addUrl = node.Attributes["href"].Value.ToString(); //Добавочная сылка на страницу Области
									tbOut.Text += "Добавочная ссылка: " + addUrl + "\n";
								}

								urlArea = startUrl + addUrl; //Полный адрес ссылки на Область      
								AreaName_urlWeather[AreaName] = urlArea;
								tbOut.Text += $"Полный адрес ссылки на {AreaName}: " + urlArea + "\n";

							}
						}
						catch
						{
							continue;
						}
					}
				}
				catch
				{
					continue;
				}
			}
		}
		#endregion


		private void Bt_test_Click(object sender, RoutedEventArgs e)
        {
			//MessageBox.Show(CityName_urlWeather["Москва"]);		
			var webWeather = new HtmlWeb();
			var docWeather = webWeather.Load(CityName_urlWeather["Красногорск"]);
			var tempNow = docWeather.DocumentNode.SelectNodes("/html/body/div[1]/div[6]/div[1]/div/div[2]/div[1]/div[5]/a//div[1]/span[1]");

			MessageBox.Show("Температура сейчас " + tempNow[0].InnerText);
            MessageBox.Show(CityName_urlWeather["Авсюнино"]);
        }
    }
}
