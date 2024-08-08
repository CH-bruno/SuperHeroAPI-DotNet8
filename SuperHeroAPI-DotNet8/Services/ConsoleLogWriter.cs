using SuperHeroAPI_DotNet8.Interfaces;

namespace SuperHeroAPI_DotNet8.Services
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
