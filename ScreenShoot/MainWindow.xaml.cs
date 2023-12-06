﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace ScreenShoot
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

        BitmapImage ByteToImage(byte[] byteArray)
        {
            BitmapImage bitmapImage = new BitmapImage();


            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            UdpClient client = new UdpClient();
            var remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            var buffer = new byte[ushort.MaxValue - 29];
            await client.SendAsync(buffer, buffer.Length, remoteEP);
            var list = new List<byte>();
            var maxlen = buffer.Length;
            var len = 0;
            while (true)
            {
                var result = await client.ReceiveAsync();
                buffer = result.Buffer;
                len = buffer.Length;
                list.AddRange(buffer);
                if (len != maxlen) break;
            }

            ClientImage.Source = ByteToImage(list.ToArray());
            list.Clear();
        }
    }
}

