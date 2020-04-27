using Newtonsoft.Json;
using Planner.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Planner.Utilty
{
    public class DataService
    {
        private readonly FileInfo _sourceFile;

        public DataService()
            : this(new FileInfo("FolderData.txt"))
        {
        }

        public DataService(FileInfo sourceFile)
        {
            _sourceFile = sourceFile;
        }

        public ObservableCollection<Folder> LoadData()
        {
            if (!_sourceFile.Exists)
            {
                _sourceFile.Create().Dispose();
                return new ObservableCollection<Folder>();
            }

            using(StreamReader reader = _sourceFile.OpenText())
            {
                string Data = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<Folder>>(Data);
            }
        }
        public void SaveData(ObservableCollection<Folder> collection)
        {
            using (StreamWriter WriterThread = _sourceFile.CreateText())
            {
                string TextToWrite = JsonConvert.SerializeObject(collection);
                WriterThread.Write(TextToWrite);
            }
        }

    }
}
