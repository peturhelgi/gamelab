using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;

namespace Project.LeveManager
{
    public class JsonUtil<T>
    {
        [JsonIgnore]
        public Type Type;
        JsonSerializerSettings settings;
        IsolatedStorageFile _isf;
        public JsonUtil()
        {
            settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            Type = typeof(T);
            _isf = IsolatedStorageFile.GetUserStoreForApplication();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The path of a serialized JSON file.</param>
        /// <returns></returns>
        public T Load(string path)
        {
            if (!path.EndsWith(".json")) { path += ".json"; }
            T instance;
            try
            {
                using (StreamReader streamReader = new StreamReader(_isf.OpenFile(path, FileMode.Open)))
                {
                    string data = streamReader.ReadToEnd();
                    instance = JsonConvert.DeserializeObject<T>(data, settings);
                }
            }
            catch (FileNotFoundException)
            {
                instance = default(T);
            }
            return instance;
        }

        //make sure file ex
        public bool Save(string path, object obj)
        {
            bool success;
            try
            {
                String data = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
                if (_isf.FileExists(path))
                {
                    _isf.DeleteFile(path);
                }
                StreamWriter streamWriter = new StreamWriter(_isf.CreateFile(path));
                streamWriter.Write(data);
                streamWriter.Dispose();
                success = true;
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
    }
}
