using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            Type = typeof(T);
        }

        public T Load(string path) {
            string data = File.ReadAllText(path);
            T instance = JsonConvert.DeserializeObject<T>(data, settings);
            return instance;
        }

        public bool Save(string path, object obj) {
            bool success;
            try
            {
                String data = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
                File.WriteAllText(path, data);
                success = true;
            }
            catch (Exception e) {
                success = false;
            }

            return success;
        }
    }
}