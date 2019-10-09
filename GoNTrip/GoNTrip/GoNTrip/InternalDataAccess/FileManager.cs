using System;
using System.IO;

namespace GoNTrip.InternalDataAccess
{
    public class FileManager
    {
        public string FilePath { get; private set; }

        public FileManager(Environment.SpecialFolder folder, string filename) =>
            this.FilePath = $"{Environment.GetFolderPath(folder)}/{filename}";

        public void CreateFile()
        {
            using (Stream S = File.Create(FilePath)) { }
        }

        public void CreateFileIfNotExists()
        {
            if (!File.Exists(FilePath))
                CreateFile();
        }

        public string ReadFile()
        {
            using (StreamReader str = new StreamReader(FilePath))
                return str.ReadToEnd();
        }

        public void WriteFile(string data)
        {
            using (StreamWriter strw = new StreamWriter(FilePath, false))
                strw.Write(data);
        }
    }
}
