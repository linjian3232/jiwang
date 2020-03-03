using System;
using System.IO.Ports;
using System.Threading;

public class Chatting
{
    static bool Judge;
    static SerialPort MyPort;

    public static void Main()
    {
        string name;
        string message;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(Read);

        // 创建一个新的串口类对象，并且全部参数按默认值处理
        MyPort = new SerialPort();
        MyPort.PortName = SetPortName(MyPort.PortName);

        // 设置读写超时时间
        MyPort.ReadTimeout = 500;
        MyPort.WriteTimeout = 500;

        MyPort.Open();
        Judge = true;
        readThread.Start();

        Console.Write("用户名: ");
        name = Console.ReadLine();

        Console.WriteLine("输入 EXIT 退出");

        while (Judge)
        {
            System.DateTime currentTime1 = new System.DateTime();
            currentTime1 = System.DateTime.Now;

            message = Console.ReadLine();
            Console.WriteLine("[SENT {0}]：{1}", currentTime1, message);
            if (stringComparer.Equals("exit", message))
            {
                Judge = false;
            }
            else
            {
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;

                MyPort.WriteLine(
                    String.Format("[RECV {0}][SENT {1}]:<{2}>: {3}", currentTime, currentTime1, name, message));
            }
        }

        readThread.Join();
        MyPort.Close();
    }

    public static void Read()
    {
        while (Judge)
        {
            try
            {
                string message = MyPort.ReadLine();
                Console.WriteLine(message);
            }
            catch (TimeoutException) { }
        }
    }

    // Display Port values and prompt user to enter a port.
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("可用端口:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("输入COM端口值 (默认: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
    // Display BaudRate values and prompt user to enter a value.

}
