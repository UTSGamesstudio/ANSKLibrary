using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using ModelAnimationLibrary;

// TODO: replace this with the type you want to import.
//using TImport = Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent;
using TImport = ModelAnimationPipeline.ANSKProcessContent;

namespace ModelAnimationPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.C:\Users\Joshua\Desktop\Work\SneakyPants3DStuff\AndAgain\TheGreatAlienEscape\src\ProjectSneakyGame\ProjectSneakyGameContent\NPC2.fbx
    /// </summary>

    [ContentImporter(".fbx", DisplayName = "Model Animation Importer", DefaultProcessor = "ModelAnimationProcessor")]
    //public class ModelAnimationImporterFbx : FbxImporter
    public class ModelAnimationImporterFbx : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            
            string fileInfo = System.IO.File.ReadAllText(filename);
            FbxImporter f = new FbxImporter();

            NodeContent c = f.Import(filename, context);

            ANSKFbxData data = new ANSKFbxData();
            data.LoadFbx(filename);

            return new TImport(c, data);
        }
    }
}
