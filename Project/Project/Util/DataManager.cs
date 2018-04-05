using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Windows.Storage;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Project.Util
{
    public class DataManager<T>
    {
        [JsonIgnore]
        public Type Type;
        JsonSerializerSettings settings;
        public DataManager() {
            settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        }

        public T Load(String path) {
            string data = File.ReadAllText(path);
            T instance = JsonConvert.DeserializeObject<T>(data, settings);
            return instance;
        }

        public async void Save(string path, object obj) {
            // TODO: Clean up this mess
           string data = JsonConvert.SerializeObject(obj);
           StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
           StorageFile file = await storageFolder.GetFileAsync(path);
           // await FileIO.WriteTextAsync(file, data);

            var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0)) {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream)) {
                    dataWriter.WriteString(JsonConvert.SerializeObject(obj, Formatting.Indented, settings));
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();
        }
    }
}
