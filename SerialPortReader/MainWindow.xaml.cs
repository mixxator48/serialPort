using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Threading;

namespace SerialPortReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort _serialPort;
        public int BaudRate = 92600;
        public string PortName;
        public MainWindow()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            
            foreach (var port in ports)
            {
                PortBox.Items.Add(port);
            }
            
        }
        //Настройка порта
        private void SetPort()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = PortName;
            _serialPort.BaudRate = BaudRate;
            _serialPort.DtrEnable = true;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }

        //Открытие выбранного порта
        private void PortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            PortName = PortBox.SelectedValue.ToString();
            SetPort();

            if (_serialPort.IsOpen == false) {
                _serialPort.Open();
             }
            
            CheckPort();
        }
       
        private void CheckPort()
        {
            System.Threading.Thread.Sleep(5000);
            try
            {
                var message = _serialPort.ReadLine();

                
                if(Convert.ToInt32(message) == BaudRate)
                {
                    MessageBox.Show("Порт готов");
                    readButton.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Неверно настроен порт");
                    readButton.IsEnabled = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Неверно настроен порт: " + e.Message);
                readButton.IsEnabled = false;
            }
        }

        //Получение текста по порту
        private void getMessage(object sender, RoutedEventArgs e)
        {

            try
            {
                var message = _serialPort.ReadLine();

                OutputText.Text = OutputText.Text + message;
            }
            catch (Exception err)
            {
                MessageBox.Show("Буффер пуст");
            }
           

        }

        //Передача текста на ардуину 
        private void sendMessage(object sender, RoutedEventArgs e)
        {
            _serialPort.WriteLine(InputText.Text);
            MessageBox.Show("Сообщение отправлено!");
        }

        //Отчистка вывода
        private void clear(object sender, RoutedEventArgs e)
        {
            OutputText.Text = "";
        }
    }
}
