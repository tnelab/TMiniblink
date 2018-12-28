using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
namespace Tnelab.TneAppMapTool
{
    //根据名称查找项目
    public static class DTEHelper
    {
        public static EnvDTE.Project GetProject(EnvDTE.DTE dte, string name)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            for (var i = 1; i <= dte.Solution.Projects.Count; i++)
            {
                if (dte.Solution.Projects.Item(i).Name == name)
                {
                    return dte.Solution.Projects.Item(i);
                }
                for (int t = 1; t <= dte.Solution.Projects.Item(i).ProjectItems.Count; t++)
                {
                    if (dte.Solution.Projects.Item(i).ProjectItems.Item(t).Name == name)
                    {
                        return dte.Solution.Projects.Item(i).ProjectItems.Item(t).SubProject;
                    }
                }
            }
            return null;
        }

        //从项目项集合中搜索搜索子元素
        public static EnvDTE.ProjectItem GetItem(EnvDTE.ProjectItems items, string name)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            for (var i = 1; i <= items.Count; i++)
            {
                if (items.Item(i).Name == name)
                {
                    return items.Item(i);
                }
            }
            return null;
        }
        //根据路径获取项目项
        public static EnvDTE.ProjectItem GetItem(EnvDTE.DTE dte, string path)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            string[] names = path.Split('/');
            var project = GetProject(dte, names[0]);
            var items = project.ProjectItems;
            EnvDTE.ProjectItem item = null;
            for (int i = 1; i < names.Length; i++)
            {
                item = GetItem(items, names[i]);
                items = item.ProjectItems;
            }
            return item;
        }
        //public static ProjectItem GetItemByFilePath(DTE dte, string name, bool recursive)
        //{
        //    Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        //    ProjectItem projectItem = null;
        //    foreach (Project project in dte.Solution.Projects)
        //    {
        //        projectItem = GetItemInProject(project, name, recursive);

        //        if (projectItem != null)
        //        {
        //            break;
        //        }
        //    }
        //    return projectItem;
        //}
        //public static ProjectItem GetItemInProject(Project project, string name, bool recursive)
        //{
        //    Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        //    ProjectItem projectItem = null;

        //    if (project.Kind != Constants.vsProjectKindSolutionItems)
        //    {
        //        if (project.ProjectItems != null && project.ProjectItems.Count > 0)
        //        {
        //            projectItem = GetItem(project.ProjectItems, name, recursive);
        //        }
        //    }
        //    else
        //    {
        //        // if solution folder, one of its ProjectItems might be a real project
        //        foreach (ProjectItem item in project.ProjectItems)
        //        {
        //            Project realProject = item.Object as Project;

        //            if (realProject != null)
        //            {
        //                projectItem = GetItemInProject(realProject, name, recursive);

        //                if (projectItem != null)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return projectItem;
        //}
    }
}
