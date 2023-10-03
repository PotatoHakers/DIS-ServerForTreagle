using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // Устанавливаем IP-адрес и порт для сервера
        IPAddress ipAddress = IPAddress.Parse("127.168.100.9"); // Локальный IP-адрес
        int port = 1024; // Порт для прослушивания

        // Создаем TCP-сервер
        TcpListener listener = new TcpListener(ipAddress, port);
        listener.Start();
        Console.WriteLine("Сервер запущен. Ожидание клиента...");

        // Принимаем клиентское подключение
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Клиент подключен.");

        // Получаем поток для чтения данных от клиента
        NetworkStream networkStream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

        // Преобразуем данные от клиента в строку
        string clientData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Получены данные от клиента: " + clientData);

        // Разделяем строку на три числа
        string[] sides = clientData.Split(',');
        double sideA = double.Parse(sides[0]);
        double sideB = double.Parse(sides[1]);
        double sideC = double.Parse(sides[2]);

        // Вычисляем полупериметр
        double s = (sideA + sideB + sideC) / 2;

        // Вычисляем площадь треугольника по формуле Герона
        double area = Math.Sqrt(s * (s - sideA) * (s - sideB) * (s - sideC));

        // Отправляем результат обратно клиенту
        string result = area.ToString();
        byte[] resultBytes = Encoding.ASCII.GetBytes(result);
        networkStream.Write(resultBytes, 0, resultBytes.Length);
        Console.WriteLine("Площадь треугольника отправлена клиенту: " + result);

        // Завершаем соединение и закрываем сервер
        client.Close();
        listener.Stop();
    }
}