using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EMPCLIENT
{
    /// <summary>
    /// grade_show.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class grade_show : Window
    {
        List<cc_info> cc_info_List = new List<cc_info>();
        public grade_show()
        {
            InitializeComponent();
        }
        class cc_info
        {
            public string CCNUM { get; set; }
            public string ID { get; set; }
            public string SCORE { get; set; }

            public string date { get; set; }// 

        }


        private void Button_Click(object sender, RoutedEventArgs e) // 유저 성적 요청할거임
        {
            NetworkStream stream = MainPage.client.GetStream(); //데이터 전송에 사용된 스트림


            string send_msg;

            // 상담원 로그인 플래그 보내기 
            byte[] data;
            data = null;
            data = new byte[256];
            send_msg = "성적요청";// 시그널
            data = Encoding.UTF8.GetBytes(send_msg);
            stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

            Thread.Sleep(100);

            data = null;
            data = new byte[256];
            if (user_id.Text != null)
            {
                send_msg = user_id.Text; // 해당 유저 성적 요청 
                data = Encoding.UTF8.GetBytes(send_msg);
                stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

                Thread.Sleep(100);
            }
            string responses = "";
            while (responses != "전송종료") // 서버에서 보내는 데이터를  받아 줘야지 전송종료 메세지 올때 까지 이름 점수
            {
                cc_info cc_Info = new cc_info();
                data = null;
                data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length); //받는 데이터의 바이트배열, 인덱스, 길이
                if ((responses = Encoding.UTF8.GetString(data, 0, bytes)) != "전송종료")// 이름 받기
                    break;

                cc_Info.ID = responses;
                data = null;
                data = new byte[256];
                bytes = stream.Read(data, 0, data.Length); //받는 데이터의 바이트배열, 인덱스, 길이
                responses = Encoding.UTF8.GetString(data, 0, bytes); // 점수 받기 
                cc_Info.SCORE = responses;
                cc_info_List.Add(cc_Info);
                score_listview.ItemsSource = cc_info_List; // 리스트뷰 아이템에 넣기 
            }

        }


    }
}
