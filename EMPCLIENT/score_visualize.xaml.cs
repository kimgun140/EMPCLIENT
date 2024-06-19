using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EMPCLIENT
{
    /// <summary>
    /// score_visualize.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class score_visualize : Window
    {

        public ChartValues<double> C_Chart { get; set; }


        public score_visualize()
        {
            InitializeComponent();
            C_Chart = new ChartValues<double>();

            Chart1.DataContext = this;




        }
        async public void graph(string User_id)
        {
            NetworkStream stream = MainPage.client.GetStream();

            // 차트 그리는 요청 메세지
            string send_msg;
            byte[] data;
            data = null;
            data = new byte[256];
            send_msg = "차트";// 시그널
            data = Encoding.UTF8.GetBytes(send_msg);
            stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.
            Thread.Sleep(100);
            send_msg = "";

            send_msg = User_id;
            data = null;
            data = new byte[256];
            data = Encoding.UTF8.GetBytes(send_msg); // 그래프를 그릴 유저의 성적 요청 위해서 유저아이디 전송 
            stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.
            Thread.Sleep(100);

            string responses = "";
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                while (true) // 여러번 보내면 
                {
                    data = null;
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);//받는 데이터의 바이트배열, 인덱스, 길이
                    responses = Encoding.UTF8.GetString(data, 0, bytes);
                    if (responses != "전송종료")
                    {
                        int responses1 = int.Parse(responses); // 받은 고객의 성적을 
                        C_Chart.Add(responses1);
                    }
                    else if (responses == "전송종료")
                    {
                        break;
                    }
                }
            }

            ));
           



        }
        async public void testgraph()
        {

            await Dispatcher.BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        int testvalue = i;
                        C_Chart.Add(Convert.ToInt32(testvalue));

                    }
                    //C_Chart = null;
                    //ChartValues<double> value = new ChartValues<double>{1, 2, 3, 4, 5};
                    //C_Chart.Add(Convert.ToInt32(value));

                }));
        }
        async public void charttest(string User_id)
        {
            NetworkStream stream = MainPage.client.GetStream();

            // 차트 그리는 요청 메세지
            string send_msg;
            byte[] data;
            data = null;
            data = new byte[256];
            send_msg = "차트";// 시그널
            data = Encoding.UTF8.GetBytes(send_msg);
            stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.
            Thread.Sleep(100);
            send_msg = "";

            send_msg = User_id;
            data = null;
            data = new byte[256];
            data = Encoding.UTF8.GetBytes(send_msg); // 그래프를 그릴 유저의 성적 요청 위해서 유저아이디 전송 
            stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.
            Thread.Sleep(100);


            // 데이터 받기 
            data = null;
            data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);//받는 데이터의 바이트배열, 인덱스, 길이
            string responses = Encoding.UTF8.GetString(data, 0, bytes);



            string[] words = responses.Split(','); // 문자열을 ,를 구분자로 잘라서 배열에 담기 
            for (int i = 0; i < words.Length; i++)
            {
                C_Chart.Add(int.Parse(words[i]));// 공백제거 

            }
            
            //await Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        int testvalue = i;
            //        C_Chart.Add(Convert.ToInt32(testvalue));

            //    }

            //}));


            while (true)
            {
                if (responses != "전송종료")
                {
                    data = null;
                    data = new byte[256];
                    bytes = stream.Read(data, 0, data.Length);//받는 데이터의 바이트배열, 인덱스, 길이
                    responses = Encoding.UTF8.GetString(data, 0, bytes);
                }
                else
                {
                    break;
                }

            }

           
        }

        private void Button_Click(object sender, object e)
        {
            testgraph();
        }
    }
}
