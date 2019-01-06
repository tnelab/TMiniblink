using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.TneAppMapTool
{
    static class JsNativeMapBuilder
    {
        public static string Build(CodeModel codeModel,string theNamespace,string theBase,List<string> theImportList)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            //var project = DTEHelper.GetProject(theDTE, this.ProjectName);
            //var pro = project.Properties.Item("AssemblyName");
            //var assemblyName = pro.Value.ToString();
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("//此代码由机器生成，请不要手动修改");
            foreach (var import in theImportList)
            {
                strBuilder.AppendLine($"///<reference path=\"{import}\"/>");
            }
            strBuilder.Append("namespace ").Append(theNamespace).AppendLine("{");
            var constructorInfoList = new List<string>();
            codeModel.ProcessConstructor((funcInfo) => {
                if (funcInfo.ParamList.Count != 0)
                {
                    constructorInfoList.Insert(0, $"@Tnelab.ConstructorInfo({string.Join(",", funcInfo.ParamList.Select(it => "\"" + it.TypeName + "\"").ToArray())})");
                }
                else
                {
                    constructorInfoList.Insert(0, $"@Tnelab.ConstructorInfo()");
                }
            });
            strBuilder.AppendLine(String.Join("\r\n", constructorInfoList.Select(it => $"\t{it}").ToArray()));
            strBuilder.AppendLine($"\t@Tnelab.ToMap(\"{theNamespace}.{codeModel.ClassName}\",\"{codeModel.NamespaceName}.{codeModel.ClassName}\")");
            strBuilder.AppendLine($"\texport class {codeModel.ClassName} extends {theBase} {{");
            codeModel.ProcessProperty((propInfo) => {
                if (propInfo.HasSet)
                {                    
                    if (propInfo.Name != "this")
                    {
                        strBuilder.AppendLine($"\t\t@Tnelab.InvokeInfo(undefined,\"{propInfo.TypeName}\")");
                        var staticFlag = propInfo.IsStatic ? "static" : "";
                        strBuilder.AppendLine($"\t\tpublic {staticFlag} set {propInfo.Name}(value:{GetJsTypeNameByTypeName(propInfo.TypeName)}) {{ }}");
                    }
                    else
                    {
                        strBuilder.AppendLine($"\t\t[index:{GetJsTypeNameByTypeName(propInfo.ParamList[0].TypeName)}]:{GetJsTypeNameByTypeName(propInfo.TypeName)};");
                    }
                }
                if (propInfo.HasGet)
                {
                    if (propInfo.Name != "this")
                    {
                        var staticFlag = propInfo.IsStatic ? "static" : "";
                        strBuilder.AppendLine($"\t\tpublic {staticFlag} get {propInfo.Name}():{GetJsTypeNameByTypeName(propInfo.TypeName)} {{ return undefined; }}");
                    }
                }
            });
            codeModel.ProcessFunction((funcInfoList) => {
                if (funcInfoList.Count > 1)
                {
                    var invokeInfoList = new List<string>();
                    bool isStatic = false;
                    foreach (var funcInfo in funcInfoList)
                    {
                        if (funcInfo.ParamList.Count != 0)
                        {
                            invokeInfoList.Insert(0, $"@Tnelab.InvokeInfo(\"{funcInfo.Name}\", {string.Join(",", funcInfo.ParamList.Select(it => "\"" + it.TypeName + "\"").ToArray())})");
                        }
                        else
                        {
                            invokeInfoList.Insert(0, $"@Tnelab.InvokeInfo(\"{funcInfo.Name}\")");
                        }
                        funcInfo.ParamList.Insert(0, new CodeParamInfo() { Name = "tneMapId", TypeName = "System.Int32" });
                        if (funcInfo.GenericTypeArguments.Count > 0)
                        {
                            funcInfo.ParamList.Insert(1, new CodeParamInfo() { Name = "tneMapGenericTypeInfo", TypeName = "System.String" });
                        }
                        var staticFlag = funcInfo.IsStatic ? "static" : "";
                        if(!isStatic)
                            isStatic = funcInfo.IsStatic;
                        strBuilder.AppendLine($"\t\tpublic {staticFlag} {funcInfo.Name}({string.Join(",", funcInfo.ParamList.Select(it => $"_{it.Name}:{GetJsTypeNameByTypeName(it.TypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)}"))}):{GetJsTypeNameByTypeName(funcInfo.ReturnTypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)};");
                    }
                    strBuilder.AppendLine(String.Join("\r\n", invokeInfoList.Select(it => $"\t\t{it}").ToArray()));
                    var staticFlag2 = isStatic ? "static" : "";
                    strBuilder.AppendLine($"\t\tpublic {staticFlag2} {funcInfoList[0].ShortName}(tneMapId:number):any{{}}");
                }
                else
                {
                    var funcInfo = funcInfoList[0];
                    if (funcInfo.ParamList.Count != 0)
                    {
                        strBuilder.AppendLine($"\t\t@Tnelab.InvokeInfo(\"{funcInfo.Name}\", {string.Join(",", funcInfo.ParamList.Select(it => "\"" + it.TypeName + "\"").ToArray())})");
                    }
                    else
                    {
                        strBuilder.AppendLine($"\t\t@Tnelab.InvokeInfo(\"{funcInfo.Name}\")");
                    }
                    if (funcInfo.GenericTypeArguments.Count > 0)
                    {
                        funcInfo.ParamList.Insert(0, new CodeParamInfo() { Name = "tneMapGenericTypeInfo", TypeName = "System.String" });
                    }
                    var staticFlag = funcInfo.IsStatic ? "static" : "";
                    strBuilder.AppendLine($"\t\tpublic {staticFlag} {funcInfo.Name}({string.Join(",", funcInfo.ParamList.Select(it => $"_{it.Name}:{GetJsTypeNameByTypeName(it.TypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)}"))}):{GetJsTypeNameByTypeName(funcInfo.ReturnTypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)} {(funcInfo.ReturnTypeName == "System.Void" ? "{}" : "{return undefined;}")}");
                }
            });
            if (codeModel.CodeConstructorList.Count > 1)
            {
                foreach (var funcInfo in codeModel.CodeConstructorList)
                {
                    funcInfo.ParamList.Insert(0, new CodeParamInfo() { Name = "tneMapId", TypeName = "System.Int32" });
                    if (codeModel.GenericTypeArguments.Count > 0)
                    {
                        funcInfo.ParamList.Insert(1, new CodeParamInfo() { Name = "tneMapGenericTypeInfo", TypeName = "System.String" });
                    }
                    strBuilder.AppendLine($"\t\tpublic constructor({string.Join(",", funcInfo.ParamList.Select(it => $"_{it.Name}:{GetJsTypeNameByTypeName(it.TypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)}"))});");
                }
                strBuilder.AppendLine($"\t\tpublic constructor(...arg: any[]){{super(arguments);}}");
            }
            else if(codeModel.CodeConstructorList.Count==1)
            {
                var funcInfo = codeModel.CodeConstructorList[0];
                if (codeModel.GenericTypeArguments.Count > 0)
                {
                    funcInfo.ParamList.Insert(0, new CodeParamInfo() { Name = "tneMapGenericTypeInfo", TypeName = "System.String" });
                }
                strBuilder.AppendLine($"\t\tpublic constructor({string.Join(",", funcInfo.ParamList.Select(it => $"_{it.Name}:{GetJsTypeNameByTypeName(it.TypeName, codeModel.GenericTypeArguments, funcInfo.GenericTypeArguments)}"))}) {{super(arguments);}}");
            }
            strBuilder.AppendLine("\t}");
            strBuilder.AppendLine("}");
            return strBuilder.ToString();
        }
        static string GetJsTypeNameByTypeName(string tName, List<string> classGenericTypeArguments = null, List<string> funcGenericTypeArguments = null)
        {
            if(tName==null)
            {

            }
            tName = tName.Trim();
            if (funcGenericTypeArguments != null && funcGenericTypeArguments.Count != 0)
            {
                if (funcGenericTypeArguments.Count(it => it == tName) != 0)
                {
                    return tName;
                }
            }
            if (classGenericTypeArguments != null && classGenericTypeArguments.Count != 0)
            {
                if (classGenericTypeArguments.Count(it => it == tName) != 0)
                {
                    return tName;
                }
            }
            string gv = "any";
            switch (tName)
            {
                case "System.Char":
                case "System.String":
                    gv = "string";
                    break;
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Double":
                case "System.Decimal":
                case "System.Single":
                case "System.Byte":
                case "System.SByte":
                    gv = "number";
                    break;
                case "System.Boolean":
                    gv = "boolean";
                    break;
                case "System.Void":
                    gv = "void";
                    break;
            }
            if (gv == "any")
            {
                if (tName.StartsWith("System.Action"))
                {
                    if (tName.IndexOf("<") == -1)
                    {
                        gv = "()=>void";
                    }
                    else
                    {
                        var gvd = tName.Replace("System.Action<", "").Replace(">", "").Split(',');
                        for (var i = 0; i < gvd.Length; i++)
                        {
                            gvd[i] = $"arg{i}:{GetJsTypeNameByTypeName(gvd[i], classGenericTypeArguments, funcGenericTypeArguments)}";
                        }
                        gv = $"({String.Join(",", gvd)})=>void";
                    }
                    //gv="Function";
                }
                else if (tName.StartsWith("System.Func"))
                {
                    var gvd = tName.Replace("System.Func<", "").Replace(">", "").Split(',');
                    var paramTypes = new string[gvd.Length - 1];
                    var returnType = gvd[gvd.Length - 1];
                    for (var i = 0; i < gvd.Length - 1; i++)
                    {
                        paramTypes[i] = $"arg{i}:{GetJsTypeNameByTypeName(gvd[i], classGenericTypeArguments, funcGenericTypeArguments)}";
                    }
                    gv = $"({String.Join(",", paramTypes)})=>{GetJsTypeNameByTypeName(returnType, classGenericTypeArguments, funcGenericTypeArguments)}";
                }
            }
            return gv;
        }
    }
}
