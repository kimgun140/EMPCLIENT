using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMPCLIENT
{
    /// <summary>
    /// question_add.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class question_add : Window
    {
        public question_add()
        {
            InitializeComponent();
        }

        public class MyData
        {
            public string word { get; set; }
            public string meaning { get; set; }
        }
        List<MyData> data_list ;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NetworkStream stream = MainPage.client.GetStream();
            //question_add.Text;






            // 문제추가 시그널 보내기
            string send_message = "문제추가";

            byte[] data;
            data = null;
            data = Encoding.UTF8.GetBytes(send_message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(100);
            send_message = question_add_btn.Text;
            if (!string.IsNullOrEmpty(send_message))
            {
                // 추가할 문자 보내기
                data = null;
                data = Encoding.UTF8.GetBytes(send_message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(100);
                //a문제추가 종료 시그널 보내기 
                send_message = "문제추가종료";
                data = null;
                data = Encoding.UTF8.GetBytes(send_message);
                stream.Write(data, 0, data.Length);


            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // 미리보기 버튼 
        {
            NetworkStream stream = MainPage.client.GetStream();
            string send_message = "문제미리보기";
            byte[] data;
            data = null;
            data = Encoding.UTF8.GetBytes(send_message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(100);
            send_message = question_add_btn.Text; // 추가할 문제 검색어 
            if (!string.IsNullOrEmpty(send_message))
            {
                // 추가할 문자 검색어  보내기
                data = null;
                data = Encoding.UTF8.GetBytes(send_message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(100);

            }
            data = null;
            data = new byte[300];
            string responses = "";


            while (true)
            {
                MyData myData = new MyData();

                int bytes = stream.Read(data, 0, data.Length);
                responses = Encoding.UTF8.GetString(data, 0, bytes);

                // 여기서 json 형식으로 데이터 받기
                JObject jObject = JObject.Parse(responses);

                myData.word = responses;
                myData.meaning = Convert.ToString(jObject[send_message]);

                data_list.Add(myData);

                //Console.WriteLine(send_message); // 검색한 단어
                //Console.WriteLine(jObject[send_message]); // 검색한 단어의 뜻 
                //testbox.Text += "검색한 단어:" + responses + "\n";
                //testbox.Text += "단어의 뜻: " + jObject[send_message] + "\n";
            }


            //JArray jArray = (JArray)jObject["realtimePositionList"];

            //if (jArray != null)
            //{
            //    foreach (JObject item in jArray)
            //    {

            //        testbox.Text = Convert.ToString(item[0]);

            //    }
            //}

        }


    }
}
