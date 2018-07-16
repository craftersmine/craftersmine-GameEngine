using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GDK.Core
{
    public class Project
    {
        public string Filepath { get; private set; }
        public List<ProjectFile> Files { get; } = new List<ProjectFile>();
        public string ProjectName { get; private set; }
        public string ProjectRoot { get; private set; }

        public Project(string filepath)
        {
            Filepath = filepath;
        }

        public void LoadProject()
        {
            string[] cont = File.ReadAllLines(Filepath);
            ProjectRoot = Path.GetDirectoryName(Filepath);

            if (cont[0] == "%PROJECTFILE")
            {
                foreach (var ln in cont)
                {
                    string[] splittedLine = ln.Split(':');
                    switch (splittedLine[0])
                    {
                        case "PROJECTNAME":
                            ProjectName = splittedLine[1];
                            break;
                        case "PROJECTFILE":
                            Files.Add(new ProjectFile(ProjectRoot, splittedLine[1]));
                            break;
                    }
                }
            }
        }
    }
}
