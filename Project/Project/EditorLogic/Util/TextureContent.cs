using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace TheGreatEscape.EditorLogic.Util
{
    public static class TextureContent
    {
        public static Dictionary<string, T> LoadListContent<T>(ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/Sprites/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, T> result = new Dictionary<String, T>();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                string filePath = "Sprites/" + contentFolder + "/";
                result[key] = contentManager.Load<T>(filePath + key);
            }
            return result;
        }
    }
}
