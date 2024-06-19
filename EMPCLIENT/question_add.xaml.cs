using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
using Microsoft.TeamFoundation.Common.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMPCLIENT
{
    /// <summary>
    /// question_add.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class question_add : Window
    {
        List<MyData> data = new List<MyData>();
        public question_add()
        {
            InitializeComponent();
            //NativeMethods.AllocConsole();



        }
        static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AllocConsole();
        }


        List<MyData> myDatas = new List<MyData>();
        public class MyData
        {
            public string word { get; set; }
            public string meaning { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NetworkStream stream = MainPage.client.GetStream();
            //question_add.Text;

            // 문제추가 시그널 보내기
            string send_message = "문제관리";
            byte[] data;
            data = null;
            data = Encoding.UTF8.GetBytes(send_message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(100);


            send_message = "";
            send_message = "추가";
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
                // 이거는 반복을 할 필요가 없네 한번만 보내면 되니까 
                ////a문제추가 종료 시그널 보내기 
                //send_message = "문제추가종료";
                //data = null;
                //data = Encoding.UTF8.GetBytes(send_message);
                //stream.Write(data, 0, data.Length);

            }
            //send_message = "데이터전송완료";
            //data = null;
            //data = Encoding.UTF8.GetBytes(send_message);
            //stream.Write(data, 0, data.Length);
            //Thread.Sleep(100);


        }
        public void Quest_(JToken obj)
        {
            if (obj != null)
            {
                foreach (var ans_ in obj)
                {
                    MyData myData = new MyData(); //객체를 계속 만들어줘야함
                    myData.word = ans_["Keyword"].ToString();
                    myData.meaning = ans_["Answer"].ToString().Replace('\n', ' ') + '\n';
                    //addstring = ques.Qus_key + ques.Qus_ans;
                    myDatas.Add(myData);
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) // 미리보기 버튼 
        {
            NetworkStream stream = MainPage.client.GetStream();
            // 서버에 문제관리 시그널 보내기 
            string send_message = "문제관리";
            byte[] data;
            data = null;
            data = Encoding.UTF8.GetBytes(send_message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(100);
            send_message = "미리보기";

            data = null;
            data = Encoding.UTF8.GetBytes(send_message);
            stream.Write(data, 0, data.Length);
            Thread.Sleep(100);


            send_message = question_add_btn.Text; // 추가할 문제 검색어 
            if (!string.IsNullOrEmpty(send_message))
            {
                //  문자 검색어  보내기
                data = null;
                data = Encoding.UTF8.GetBytes(send_message); //검색어 변환
                stream.Write(data, 0, data.Length); // 검색어 보내기 
                Thread.Sleep(100);

            }




            MyData myData = new MyData();
            data = null;
            data = new byte[2000];
            string responses = "";
            int bytes = stream.Read(data, 0, data.Length);
            responses = Encoding.UTF8.GetString(data, 0, bytes);
            testbox.Text = responses + "\n";
            Console.WriteLine(responses);

            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responses);
            //myData = JsonConvert.DeserializeObject<MyData>(responses); 이것도 되나? 



            if (dictionary != null)
            {
                //MyData quest_data = new MyData();
                foreach (var kvp in dictionary)
                {
                    MyData quest_data = new MyData();
                    quest_data.word = kvp.Key;
                    quest_data.meaning = kvp.Value;
                    myDatas.Add(quest_data);

                }
                question_listview.ItemsSource = myDatas;
                question_listview.Items.Refresh();
            }else
            {
                MessageBox.Show("검색어를 정확히 입력해주세요!");
            }




        }


    }
}
