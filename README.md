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
* 切换为debug模式，可进行调试
## 项目结构
* Tools\Tnelab.TneAppExeTemplate
> 项目模板
* Tools\Tnelab.TneAppLibForNetStandardTemplate
> 项目模板
* Tools\Tnelab.TneAppLibTemplate
> 项目模板
* Tools\Tnelab.TneAppMapTool
> 代码生成器
* Tools\Tnelab.TneAppToolkit
> 综合工具包
* 所有.t4
> 废弃的代码生成工具，留档用
* cmd
> 代码实验用
* Tnelab.MiniBlinkV
> MiniBlink的封装
* Tnelab.TneForm
> 主项目
* Tnelab.TneForm.Test
> 测试程序
* Tnelab.TneForm.Test.BLL
> 测试组件
* Tnelab.TneForm.Test.Entity
> 测试组件
