﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = System.String;

namespace Level_Importer
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>

    [ContentImporter(".json", DisplayName = "Level Importer", DefaultProcessor = "LevelProcessor")]
    public class ContentImporter1 : ContentImporter<TInput>
    {

        public override TInput Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing Map file: {0}", filename);

            using (StreamReader streamReader = new StreamReader(filename))
            {
                return streamReader.ReadToEnd();
            }
        }

    }

}
