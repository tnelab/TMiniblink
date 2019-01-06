using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Task = System.Threading.Tasks.Task;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Configuration;
using RazorEngine.Text;
using System.Collections.Generic;

namespace Tnelab.TneAppMapTool
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Map.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class Map : AsyncPackage
    {
        private DTE _dte;
        private Events _dteEvents;
        private DocumentEvents _documentEvents;
        /// <summary>
        /// ToTypeScript GUID string.
        /// </summary>
        public const string PackageGuidString = "305b77a7-2bef-4473-844c-00335148f6fb";

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            _dte = await GetServiceAsync(typeof(SDTE)) as DTE;
            Assumes.Present(_dte);
            _dteEvents = _dte.Events;
            _documentEvents = _dteEvents.DocumentEvents;
            _documentEvents.DocumentSaved += (doc) => {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (doc.Name.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var tdoc = doc.Object("TextDocument") as TextDocument;
                    var sp = tdoc.StartPoint;
                    var ep = sp.CreateEditPoint();
                    //ep.StartOfDocument();
                    var txt = ep.GetLines(1, 2).Trim();
                    if (!txt.StartsWith("//tne://to_ts",StringComparison.OrdinalIgnoreCase))
                        return;
                    ToTypeScript(doc,txt);
                }
                else if (doc.Name.EndsWith(".cshtml",StringComparison.OrdinalIgnoreCase))
                {                    
                    var tdoc = doc.Object("TextDocument") as TextDocument;
                    var sp = tdoc.StartPoint;
                    var ep = sp.CreateEditPoint();
                    //ep.StartOfDocument();
                    var txt = ep.GetLines(1, 2);
                    if (!txt.StartsWith("@*tne://to_html",StringComparison.OrdinalIgnoreCase))
                        return;
                    ToHtml(doc,txt);
                    return;
                }
                //else if (doc.Name.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                //{
                //    StringBuilder sb = new StringBuilder();
                //    foreach(Property p in doc.ProjectItem.Properties)
                //    {
                //        try
                //        {
                //            sb.AppendLine($"{p.Name}=>{p.Value}");
                //        }
                //        catch { }
                //    }
                //    Debug.WriteLine(sb.ToString());
                //}
            };
        }
        void ToTypeScript(Document doc,string tneTxt)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                string theNamespace = null;
                List<string> theImportList = new List<string>();
                var theBase = "Tnelab.NativeObject";
                var tdoc = doc.Object("TextDocument") as TextDocument;
                var sp = tdoc.StartPoint;
                var ep = sp.CreateEditPoint();
                var lineNum = 2;
                string theType = null;
                while (true)
                {
                    try
                    {
                        var txt = ep.GetLines(lineNum, lineNum + 1).Trim();
                        if (txt.StartsWith("//namespace:", StringComparison.OrdinalIgnoreCase))
                        {
                            theNamespace = txt.Split(':')[1].Trim();
                        }
                        else if (txt.StartsWith("//base:", StringComparison.OrdinalIgnoreCase))
                        {
                            theBase = txt.Split(':')[1].Trim();
                        }
                        else if (txt.StartsWith("//import:", StringComparison.OrdinalIgnoreCase))
                        {
                            theImportList.Add(txt.Split(':')[1].Trim());
                        }
                        else if (txt.StartsWith("//type:", StringComparison.OrdinalIgnoreCase))
                        {
                            theType = txt.Split(':')[1].Trim();
                        }
                        else
                        {
                            break;
                        }
                        lineNum++;
                    }
                    catch { break; }
                }
                if (theImportList.Count == 0)
                {
                    theImportList.Add("TneApp.d.ts");
                }
                CodeModel codeModel;
                if (theType != null)
                {
                    codeModel = new CodeModel(theType);
                }
                else
                {
                    codeModel = new CodeModel(doc.ProjectItem.FileCodeModel);
                }

                if (theNamespace == null)
                    theNamespace = codeModel.NamespaceName;
                var code = JsNativeMapBuilder.Build(codeModel, theNamespace, theBase, theImportList);
                var filePath = $"{doc.FullName}.ts";
                ProjectItem tsItem = null;
                bool canSave = true;
                if (!File.Exists(filePath))
                {                    
                    canSave = false;
                }
                File.WriteAllText(filePath, "");
                tsItem = doc.ProjectItem.ProjectItems.AddFromFile(filePath);
                tsItem.Properties.Item("ItemType").Value = "TypeScriptCompile";
                if (!tsItem.IsOpen)
                {
                    var w = tsItem.Open(EnvDTE.Constants.vsViewKindCode);
                    if (!canSave)
                        w.Activate();
                }
                var tdoc2 = tsItem.Document.Object("TextDocument") as TextDocument;
                var tsEditPoint = tdoc2.StartPoint.CreateEditPoint();
                tsEditPoint.Delete(100000);
                tsEditPoint.Insert(code);
                if (canSave)
                    tsItem.Document.Save();
            }
            catch(Exception ex)
            {
                File.WriteAllText($"{doc.FullName}.ts", ex.ToString());
            }
        }
        void ToHtml(Document doc,string tneTxt)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                //var tdoc = doc.Object("TextDocument") as TextDocument;
                var cshtml = File.ReadAllText(doc.FullName);
                var config = new TemplateServiceConfiguration();
                var project = doc.ProjectItem.ContainingProject;
                config.TemplateManager = new TneMapTemplateManager($"{Path.GetDirectoryName(project.FullName)}\\", doc.Path);
                config.CachingProvider = new DefaultCachingProvider();
                config.EncodedStringFactory = new RawStringFactory(); // Raw string encoding.
                config.EncodedStringFactory = new HtmlEncodedStringFactory(); // Html encoding.
                var engin = RazorEngineService.Create(config);
                Engine.Razor = engin;
                var result = $"<!--此代码由机器生成，请不要手动修改-->\r\n";
                result += Engine.Razor.RunCompile(new LoadedTemplateSource(cshtml, doc.FullName), "ToHtml", null, null, new DynamicViewBag());
                var filePath = $"{doc.FullName.Remove(doc.FullName.Length - 7, 7)}.html";
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, result);
                    var hItem = doc.ProjectItem.ProjectItems.AddFromFile(filePath);
                    hItem.Properties.Item("ItemType").Value= "EmbeddedResource";
                }
                else
                {
                    File.WriteAllText(filePath, result);
                }
            }
            catch(Exception ex)
            {
                File.WriteAllText($"{doc.FullName.Remove(doc.FullName.Length - 7, 7)}.html", ex.ToString());
            }
                        
        }
        #endregion
    }
    class TneMapTemplateManager : ITemplateManager
    {
        readonly Dictionary<ITemplateKey, ITemplateSource> keyPathDic_ = new Dictionary<ITemplateKey, ITemplateSource>();
        string root_;
        string currDir_;
        public TneMapTemplateManager(string root,string currDir)
        {
            root_ = root;
            currDir_ = currDir;
        }
        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {            
            keyPathDic_.Add(key, source);
        }

        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            var key = new FullPathTemplateKey(name, name, resolveType, context);
            return key;
        }

        public ITemplateSource Resolve(ITemplateKey key)
        {
            if (!this.keyPathDic_.ContainsKey(key))
            {
                var filePath = $"{currDir_}{key.Name.Trim().Replace("/", "\\")}";
                if (key.Name.StartsWith("~"))
                {
                    filePath = $"{root_}{key.Name.Trim().Remove(0,2).Replace("/","\\")}";
                }                
                var cshtml = File.ReadAllText(filePath);
                this.keyPathDic_.Add(key, new LoadedTemplateSource(cshtml, filePath));
            }
            return this.keyPathDic_[key];
        }
    }
}
