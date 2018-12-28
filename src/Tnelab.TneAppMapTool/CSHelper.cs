using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EnvDTE;


namespace Tnelab.TneAppMapTool
{
    class CodeParamInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
    }
    class CodeFunctionInfo
    {
        public string Name { get; set; }
        public string ReturnTypeName { get; set; }
        public string ShortName { get; set; }
        public List<CodeParamInfo> ParamList = new List<CodeParamInfo>();
        public List<string> GenericTypeArguments = new List<string>();
        public string ParamString { get; set; }
        public string InvokeParamString { get; set; }
    }
    class CodePropertyInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool HasSet { get; set; }
        public bool HasGet { get; set; }
        public List<CodeParamInfo> ParamList = new List<CodeParamInfo>();
    }
    class CodeModel
    {
        EnvDTE.FileCodeModel code_model_;
        public string NamespaceName { get; set; }
        public string ClassName { get; set; }
        public string ShortClassName { get; set; }
        public List<CodeFunctionInfo> CodeConstructorList = new List<CodeFunctionInfo>();
        public List<CodeFunctionInfo> CodeFunctionList = new List<CodeFunctionInfo>();
        public List<CodePropertyInfo> CodePropertyList = new List<CodePropertyInfo>();
        public List<string> GenericTypeArguments = new List<string>();


        public CodeModel(EnvDTE.FileCodeModel codeModel)
        {
            code_model_ = codeModel;
            parse();
        }
        //解析代码
        void parse()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (code_model_ == null)
                throw new Exception("代码模型不能为空");

            CodeElements codes = code_model_.CodeElements;

            //查找命名空间，只支持第一命名空间
            CodeNamespace codeNamespace = null;

            for (int i = 1; i <= codes.Count; i++)
            {
                if (codes.Item(i).Kind == vsCMElement.vsCMElementNamespace)
                {
                    codeNamespace = codes.Item(i) as CodeNamespace;
                    break;
                }
            }
            if (codeNamespace == null)
                throw new Exception("未找到命名空间定义");
            NamespaceName = codeNamespace.Name;

            //查找类或定义，只支持第一个类或接口定义
            CodeClass codeClass = null;
            CodeInterface codeInterface = null;
            for (int i = 1; i <= codeNamespace.Members.Count; i++)
            {
                if (codeNamespace.Members.Item(i).Kind == vsCMElement.vsCMElementClass)
                {
                    codeClass = codeNamespace.Members.Item(i) as CodeClass;
                    break;
                }
                else if (codeNamespace.Members.Item(i).Kind == vsCMElement.vsCMElementInterface)
                {
                    codeInterface = codeNamespace.Members.Item(i) as CodeInterface;
                    break;
                }
            }
            if (codeClass == null)
                throw new Exception("未找到类或接口定义");
            if (codeClass != null)
            {
                ShortClassName = codeClass.Name;
                ClassName = codeClass.FullName.Replace($"{this.NamespaceName}.", "");
                if (ClassName.IndexOf("<") != -1)
                {
                    var s = ClassName.IndexOf("<") + 1;
                    var l = ClassName.Length - s - 1;
                    GenericTypeArguments = ClassName.Substring(s, l).Split(',').Select(it => it.Trim()).ToList();

                }
                parse_body(codeClass);
            }
            //else
            //{
            //    ClassName = codeInterface.Name;
            //    parse_body(codeInterface);
            //}
        }
        //解析接口或类
        void parse_body(EnvDTE.CodeClass codeClass)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var children = codeClass.Members;
            foreach (CodeElement codeElement in children)
            {
                //解析方法定义，只解析公有方法
                if (codeElement.Kind == vsCMElement.vsCMElementFunction)
                {
                    CodeFunction codeFunction = codeElement as CodeFunction;
                    if (codeFunction.Access == vsCMAccess.vsCMAccessPublic)
                    {
                        CodeFunctionInfo func_info = new CodeFunctionInfo();
                        //解析返回值
                        var returnType = codeFunction.Type.AsFullName;
                        func_info.ReturnTypeName = returnType;

                        //解析参数
                        //string parms = "";
                        string parms_for_invoke = "";

                        foreach (CodeParameter param in codeFunction.Parameters)
                        {
                            //TextPoint start = param.GetStartPoint();
                            //TextPoint finish = param.GetEndPoint();
                            //parms += start.CreateEditPoint().GetText(finish) + ",";
                            func_info.ParamList.Add(new CodeParamInfo() { Name = param.Name, TypeName = param.Type.AsFullName });
                            parms_for_invoke += param.Name + ",";
                        }
                        //if (parms.Length > 0) parms = parms.Remove(parms.Length - 1, 1);
                        if (parms_for_invoke.Length > 0) parms_for_invoke = parms_for_invoke.Remove(parms_for_invoke.Length - 1, 1);
                        //func_info.ParamString = parms;
                        func_info.InvokeParamString = parms_for_invoke;
                        func_info.ShortName = codeFunction.Name;
                        var tmps = codeFunction.FullName.Split('.');
                        func_info.Name = tmps[tmps.Length - 1];
                        if (func_info.Name.IndexOf("<") != -1)
                        {
                            var s = func_info.Name.IndexOf("<") + 1;
                            var l = func_info.Name.Length - s - 1;
                            func_info.GenericTypeArguments = func_info.Name.Substring(s, l).Split(',').Select(it => it.Trim()).ToList();

                        }
                        if (func_info.ShortName == this.ShortClassName)
                        {
                            CodeConstructorList.Add(func_info);
                        }
                        else if(func_info.ShortName!=codeClass.Name)
                        {
                            CodeFunctionList.Add(func_info);
                        }
                    }
                }
                ////解析属性定义，只解析公有属性
                else if (codeElement.Kind == vsCMElement.vsCMElementProperty)
                {
                    CodeProperty codeProperty = codeElement as CodeProperty;
                    if (codeProperty.Access == vsCMAccess.vsCMAccessPublic)
                    {
                        CodePropertyInfo property_info = new CodePropertyInfo();
                        property_info.Name = codeProperty.Name;
                        property_info.TypeName = codeProperty.Type.AsFullName;
                        try
                        {
                            var getter = codeProperty.Getter;
                            property_info.HasGet = getter != null && getter.Access == vsCMAccess.vsCMAccessPublic;
                        }
                        catch
                        {
                            property_info.HasGet = false;
                        }
                        try
                        {
                            var setter = codeProperty.Setter;
                            property_info.HasSet = setter != null && setter.Access == vsCMAccess.vsCMAccessPublic;
                            foreach (CodeParameter param in setter.Parameters)
                            {
                                //TextPoint start = param.GetStartPoint();
                                //TextPoint finish = param.GetEndPoint();
                                //parms += start.CreateEditPoint().GetText(finish) + ",";
                                property_info.ParamList.Add(new CodeParamInfo() { Name = param.Name, TypeName = param.Type.AsFullName });
                            }                            
                        }
                        catch
                        {
                            property_info.HasSet = false;
                        }
                        CodePropertyList.Add(property_info);
                    }
                }
            }
            foreach(CodeClass baseClass in codeClass.Bases)
            {
                if(baseClass.Kind== vsCMElement.vsCMElementClass)
                {
                    parse_body(baseClass);
                    break;
                }
            }
        }
        //处理构造函数
        public void ProcessConstructor(Action<CodeFunctionInfo> action)
        {
            foreach (var funcInfo in CodeConstructorList)
            {
                action(funcInfo);
            }
        }
        //处理公有方法
        public void ProcessFunction(Action<List<CodeFunctionInfo>> action)
        {
            var group = CodeFunctionList.GroupBy(it => it.ShortName);
            foreach (var item in group)
            {
                action(item.ToList());
            }
        }
        //处理公有属性
        public void ProcessProperty(Action<CodePropertyInfo> action)
        {
            foreach (var prop_info in CodePropertyList)
            {
                action(prop_info);
            }
        }
    }
}
