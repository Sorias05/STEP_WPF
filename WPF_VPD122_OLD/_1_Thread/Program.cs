using LibDatabase;
using LibDatabase.Entities;
using System.Diagnostics;
using System.Threading;

namespace _1_Thread
{
    public delegate void ConnectionCompleteDelegate(MyDataContext context);
    class Program
    {
        public static event ConnectionCompleteDelegate _connectionComplete;
        public static void Main(string[] args)
        {
            _connectionComplete += Program__connectionComplete;
            int idThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Main flow {0}", idThread);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Thread queue = new Thread(Queue);
            //queue.Start();
            //SendMessages();
            //queue.Join();
            Thread connect = new Thread(ConnectionDataBase);
            connect.Start();
            //int count = myData.Users.Count();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        private static void Program__connectionComplete(MyDataContext context)
        {
            Console.WriteLine("Event completed connection {0}", Thread.CurrentThread.ManagedThreadId);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < 1000; i++)
            {
                context.Users.Add(new UserEntity 
                {
                    Name="Іван",
                    Phone="0938783232",
                    Password="88888888"
                });
                context.SaveChanges();
                ShowMessage($"Insert user: {i}", ConsoleColor.Yellow);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        private static void ConnectionDataBase()
        {;
            ShowMessage("Begin of connection to data base.", ConsoleColor.Red);
            MyDataContext myData = new MyDataContext();
            ShowMessage("End of connection to data base.", ConsoleColor.Red);
            if (_connectionComplete != null)
                _connectionComplete(myData);
        }
        private static void ShowMessage(string text, ConsoleColor color)
        {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = consoleColor;
        }
        private static void Queue()
        {
            int idThread = Thread.CurrentThread.ManagedThreadId;
            var consoleColor = Console.ForegroundColor;
            Console.WriteLine("Secondary flow {0}", idThread);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Passanger processing: {0}", i + 1);
                Console.ForegroundColor = consoleColor;
            }
        }  
        private static void SendMessages()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(300);
                Console.WriteLine("Messages sending: {0}", i + 1);
            }
        }
    }
}
