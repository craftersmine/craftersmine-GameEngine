using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.Packager.Lib.Core;

namespace craftersmine.GDK.Core
{
    public class ProjectFile
    {
        public string[] FileContents { get; set; }
        public string Filepath { get; private set; }
        public string Extention { get; private set; }
        public ProjectFileType Filetype { get; private set; }

        public ProjectFile(string projectRoot, string filepath)
        {
            Filepath = Path.Combine(projectRoot, filepath);
            FileContents = LoadContents();
        }

        public string[] LoadContents()
        {
            string[] cont = File.ReadAllLines(Filepath);
            try
            {
                switch (Path.GetExtension(Filepath).ToLower())
                {
                    case ".cs":
                        Filetype = ProjectFileType.CSharpSource;
                        Extention = "cs";
                        break;
                    case ".cmpkg":
                        Filetype = ProjectFileType.ContentPackage;
                        Extention = "cmpkg";
                        try
                        {
                            Analyzer.AnalyzePackage(Filepath);
                        }
                        catch (Exception)
                        {
                            throw new InvalidFileContentsException();
                        }
                        break;
                    case ".scene":
                        Filetype = ProjectFileType.SceneData;
                        Extention = "scene";
                        if (cont[0] != "%SCENEMETADATA")
                            throw new InvalidFileContentsException();
                        break;
                    case ".gameobj":
                        Filetype = ProjectFileType.GameObjectData;
                        Extention = "gameobj";
                        if (cont[0] != "%GAMEOBJMETADATA")
                            throw new InvalidFileContentsException();
                        break;
                    case ".gameappdata":
                        Filetype = ProjectFileType.GameApplicationData;
                        Extention = "gameappdata";
                        if (cont[0] != "%GAMEWNDAPPDATA")
                            throw new InvalidFileContentsException();
                        break;
                    default:
                        Filetype = ProjectFileType.Unknown;
                        Extention = Path.GetExtension(Filepath);
                        break;
                }
            }
            catch (Exception)
            {
                throw new InvalidFileContentsException();
            }
            return cont;
        }

        public void SaveContents(string[] contents)
        {
            File.WriteAllLines(Filepath, contents);
        }
    }

    public enum ProjectFileType
    {
        CSharpSource, ContentPackage, SceneData, GameObjectData, GameApplicationData, Unknown
    }
    
    [Serializable]
    public class InvalidFileContentsException : Exception
    {
        public InvalidFileContentsException() { }
        public InvalidFileContentsException(string message) : base(message) { }
        public InvalidFileContentsException(string message, Exception inner) : base(message, inner) { }
        protected InvalidFileContentsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
