using SuperHeroAPI_DotNet8.Interfaces;

namespace SuperHeroAPI_DotNet8.Services
{
    public class FileLogWriter : ILogWriter
    {
        private readonly string _filePath;

        public FileLogWriter(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteLog(string message)
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
