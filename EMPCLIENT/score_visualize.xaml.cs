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
        public void graph(string User_id)
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
            while(true)
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
                    break;
            }

            

        }




    }
}
