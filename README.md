# TneApp(TMiniblink)
基于.net core，[miniblink](http://miniblink.net/)的windows桌面程序开发框架。
## 特性
* 采用HTML5创建UI；
* 支持半透异形窗体创建；
* 支持独立发布；
* 支持asp.net core razor集成，可以使用razor模板构造html页面；
* TS(JS)<=>C#无缝集成，写一份c#可在TS(JS)代码中无缝调用；
* 提供VS2017相关开发模板，让您只专注代码的书写；
## 编译和调试源码
* 安装vs2017并更新到最新版本
* 安装.net core 2.1 x86 sdk
* 把.net core sdk切换到x86模式
> 此电脑->属性->高级系统设置->高级->环境变量->Path->编辑->新建->C:\Program Files (x86)\dotnet\
> 调整其位置，使其位于C:\Program Files\dotnet\(如果有)上面
* 切换为release模式
* 编译Tools\Tnelab.TneAppToolkit
* 退出vs2017，安装编译出来的TMiniblinkToolkit.vsix
* 重新打开项目
* 切换为debug模式，可进行调试
## 项目结构
* Tools\Tnelab.TneAppExeTemplate-项目模板
* Tools\Tnelab.TneAppLibForNetStandardTemplate-项目模板
* Tools\Tnelab.TneAppLibTemplate-项目模板
* Tools\Tnelab.TneAppMapTool-代码生成器
* Tools\Tnelab.TneAppToolkit-综合工具包
* 所有.t4-废弃的代码生成工具，留档用
* cmd-代码实验用
* Tnelab.MiniBlinkV-MiniBlink的封装
* Tnelab.TneForm-主项目
* Tnelab.TneForm.Test-测试程序
* Tnelab.TneForm.Test.BLL-测试组件
* Tnelab.TneForm.Test.Entity-测试组件
## 代码生成工具的使用
> 代码生成器，可以从已有的.cs文件，生成.ts和.js文件
> 还可以从已有.net core类型，生成.ts和.js文件
> 还可以从.cshtml生成.html文件
* 确保已经安装TMiniblinkToolkit.vsix
* 从.cs生成
> * 1.在.cs文件第一行输入：//tne://to_ts，表示需要从本文件生成.ts
> * 2.在第一行后紧紧跟随以下配置指令(顺序不重要，每个个指令单独一行，指令和指令之间不得出现其他行):
> * namespace:<TS或JS类型命名空间>-可配置生成的TS类型的命名空间
> * base:<TS类型>-可配置生成的TS类型的基类
> * import:<TS文件路径>-可以配置生成的TS类型，需要引用的类型所在文件，可多个import
> * 3.疯狂按ctrl+s
* 从已有的.net core类型生成.TS
> * 1.新建一个空的.cs文件
> * 2.在.cs文件第一行输入：//tne://to_ts，表示需要从本文件生成.ts
> * 3.在第一行后紧紧跟随配置指令(同从.cs文件生成TS):
> * type:<.net core类型全名，注意泛型的类型名为x`n，x：表示类型路径，n：表示类型参数个数>
> * 3.疯狂按ctrl+s
* 从.cshtml生成.html文件（注意不要把模板文件拿去生成，最好的做法是，主内容文件拿去生成）
> * 1.文件第一行输入：@\*tne://to_html\*@
> * 2.疯狂按ctrl+s
