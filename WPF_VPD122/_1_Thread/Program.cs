using LibDatabase;
using LibDatabase.Delegates;
using LibDatabase.Entities;
using System.Diagnostics;
using System.Threading;

namespace _1_Thread
{
    class Porgram
    {
        public static event ConnectionCompleteDelegate _connectionComplete;
        public static void Main(string[] args)
        {
            _connectionComplete += Porgram__connectionComplete;
            int idThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Головний потік {0}",idThread);
            

            ///Thread queue = new Thread(Queue);
            ///queue.Start();
            ///SendMessage();
            ///queue.Join();
            Thread connect = new Thread(ConnnectionDatabase);
            connect.Start();
            //int count = myDataContext.Users.Count();

            
        }

        private static void Porgram__connectionComplete(MyDataContext context)
        {
            Console.WriteLine("Event conplete connection {0}", Thread.CurrentThread.ManagedThreadId);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                context.Users.Add(new UserEntity
                {
                    Name="Іван",
                    Phone="983 d9s9d 88s8",
                    Password="123456"    
                });
                context.SaveChanges();
                ShowMessage($"Insert user: {i}", ConsoleColor.Yellow);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

        }

        private static void ConnnectionDatabase()
        {
            ShowMessage("Begin connection database", ConsoleColor.Red);
            MyDataContext myDataContext = new MyDataContext();
            ShowMessage("Connection database completed", ConsoleColor.Red);
            if(_connectionComplete!=null)
                _connectionComplete(myDataContext);
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
            Console.WriteLine("Другорядний потік {0}", idThread);
            for (int i = 0; i < 10; i++)
            {
                //Thread.Sleep(200);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Обробка пасажира: {0}", i + 1);
                Console.ForegroundColor = consoleColor;
            }
        }
        private static void SendMessage()
        {
            for (int i = 0; i < 10; i++)
            {
                //Thread.Sleep(300);
                Console.WriteLine("Відправка повідомлень: {0}", i + 1);
            }
        }
    }
}

