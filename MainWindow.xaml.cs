using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public DataTable Select(string selectSQL) 
        {
            DataTable dataTable = new DataTable("dataBase");                
                                                                           
            SqlConnection sqlConnection = new SqlConnection("server=DESKTOP-KMCHIP8;Trusted_Connection=Yes;DataBase=DataBasesUniversity;");
            sqlConnection.Open();                                         
            SqlCommand sqlCommand = sqlConnection.CreateCommand();       
            sqlCommand.CommandText = selectSQL;                          
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); 
            sqlDataAdapter.Fill(dataTable);                                
            return dataTable;
        }


        string sql;
        DataTable data;
        public MainWindow()
        {
            InitializeComponent();
             data = Select("SELECT * FROM [dbo].[Customers]");
            DataGrid.ItemsSource = data.DefaultView;


        }

        private void ComboBox_Initialized(object sender, EventArgs e)
        {
            ComboBox.Items.Add("1.Найти покупателей, проживающих в городе Казань");
            ComboBox.Items.Add("2.Найти покупателей, фамилии которых начинаются с заданного символа");
            ComboBox.Items.Add("3.Найти покупателей, фамилии которых содержат заданную последовательность символов");
            ComboBox.Items.Add("4.Найти покупателей, фамилии которых оканчиваются заданным символом");
            ComboBox.Items.Add("5.Выдать список покупателей с указанием значения выражения Balance*100");
            ComboBox.Items.Add("6.Определить число поставщиков каждого товара");
            ComboBox.Items.Add("7.Найти минимальную цену заданного товара");
            ComboBox.Items.Add("8.Выдать упорядоченный по возрастанию цен список поставщиков заданного товара");
            ComboBox.Items.Add("9.Найти покупателей, некоторые заказы которых можно выполнить, не нарушая лимитирующего ограничения");
            ComboBox.Items.Add("10.Найти всех покупателей указанного товара");
            ComboBox.Items.Add("11.Найти максимальный по стоимости заказ");
            ComboBox.Items.Add("12.Найти все тройки <покупатель,поставщик,заказ>, удовлетворяющие условию");
            ComboBox.Items.Add("13.Вывести фамилии в таблице с заказами");
            ComboBox.Items.Add("14.Вывести список покупателей из Казани, баланс которых больше 100 000");
            ComboBox.Items.Add("15.Вывести города поставщиков без повторений");
            ComboBox.Items.Add("16.Вывести количество городов в которых есть поставщики");
            ComboBox.Items.Add("17. Вывести города в которых проживают покупатели без повторений");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    sql = "select * from \"Customers\" where \"Address\" like 'Казань%'";        
                    break;
                case 1:
                    sql = "select * from \"Customers\" where \"Family\" like 'А%'";
                    break;
                case 2:
                    sql = "select * from \"Customers\" where \"Family\" like 'Ам%'";
                    break;
                case 3:
                    sql = "select * from \"Customers\" where \"Family\" like '%ов'";
                    break;
                case 4:
                    sql = "select \"Family\", \"Balance\" * 100 from \"Customers\"";
                    break;
                case 5:
                    sql = "select \"Commodity\", count(*) from \"Providers\" Group by \"Commodity\"";
                    break;
                case 6:
                    sql = "Select min(\"Price\"),\"Commodity\" from \"Providers\" where \"Commodity\" = 'Пельмени' Group by \"Commodity\", \"Price\"";
                    break;
                case 7:
                    sql = "select min(\"Price\"),\"Commodity\",\"Name\" from \"Providers\" where \"Commodity\" = 'Сигареты' group by \"Commodity\", \"Price\", \"Name\"";
                    break;
                case 8:
                    sql = "select \"Family\" from \"Customers\" where \"IdCs\" in (select \"IdCs\" from \"Orders\" inner join \"Providers\" on \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" and \"Orders\".\"Number\" * \"Providers\".\"Price\" < \"Orders\".\"Limit\")";
                    break;
                case 9:
                    sql = "select \"Family\" from \"Customers\" where \"IdCs\" in (select \"IdCs\" from \"Orders\" where \"Commodity\" = 'Нагетсы')";
                    break;
                case 10:
                    sql = "select top 1 max(\"Orders\".\"Number\"*\"Providers\".\"Price\"), \"Providers\".\"Commodity\" from \"Orders\" inner join \"Providers\" on \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" group by \"Providers\".\"Commodity\" order by max(\"Orders\".\"Number\" * \"Providers\".\"Price\") DESC ";
                    break;
                case 11:
                    sql = "select \"Customers\".\"Family\",\"Providers\".\"Name\",\"Providers\".\"Address\",\"Orders\".\"Commodity\" from \"Orders\",\"Providers\",\"Customers\" where \"Customers\".\"IdCs\" = \"Orders\".\"IdCs\" and \"Providers\".\"Address\" = \"Customers\".\"Address\" and \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" and \"Orders\".\"Number\" * \"Providers\".\"Price\" <= \"Orders\".\"Limit\"";
                    break;
                case 12:
                    sql = "select \"Customers\".\"Family\",\"Orders\".\"Commodity\",\"Orders\".\"Number\",\"Orders\".\"Limit\" from \"Orders\",\"Customers\" where \"Customers\".\"IdCs\" = \"Orders\".\"IdCs\" group by \"Customers\".\"Family\", \"Orders\".\"Commodity\", \"Orders\".\"Number\", \"Orders\".\"Limit\"";
                    break;
                case 13:
                    sql = "select * from \"Customers\" where \"Address\" like 'Казань' and \"balance\" >= 100000";
                    break;
                case 14:
                    sql = "select \"Providers\".\"Address\" from \"Providers\" group by \"Address\"";
                    break;
                case 15:
                    sql = "select count(distinct (\"Address\")) from \"Providers\"";
                    break;
                case 16:
                    sql = "select distinct (\"Address\") as Адрес from \"Customers\"";
                    break;
            }
            data = Select(sql);
            TextBox.Text = sql;
            DataGrid.ItemsSource = data.DefaultView;

        }

   
        private void ComboboxParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboboxParam.SelectedIndex)
            {
                case 0:
                    sql = $"select * from \"Customers\" where \"Address\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 1:
                    sql = $"select * from \"Customers\" where \"Family\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 2:
                    sql = $"select * from \"Customers\" where \"Family\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 3:
                    sql = $"select * from \"Customers\" where \"Family\" like '%{TexBoxParam.Text}'";
                    data = Select(sql);
                    break;
            }
            TextBox.Text = sql;
            DataGrid.ItemsSource = data.DefaultView;
        }

        private void TexBoxParam_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TexBoxParam.Text = "";
        }

        private void TexBoxParam_TouchDown(object sender, TouchEventArgs e)
        {
            TexBoxParam.Text = "";
        }

        private void ComboboxParam_Initialized(object sender, EventArgs e)
        {
            ComboboxParam.Items.Add("1.Найти покупателей, проживающих в указанном городе");
            ComboboxParam.Items.Add("2.Найти покупателей, фамилии которых начинаются с заданного символа");
            ComboboxParam.Items.Add("3.Найти покупателей, фамилии которых содержат заданную последовательность символов");
            ComboboxParam.Items.Add("4.Найти покупателей, фамилии которых оканчиваются заданным символом");

        }
    }
}
