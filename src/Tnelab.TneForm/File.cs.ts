//此代码由机器生成，请不要手动修改
///<reference path="./tnemap.ts"/>
namespace TMiniblink{

	@Tnelab.ToMap("TMiniblink.File","System.IO.File")
	export class File extends Tnelab.NativeObject {
		@Tnelab.InvokeInfo("OpenText", "System.String")
		public static OpenText(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("CreateText", "System.String")
		public static CreateText(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("AppendText", "System.String")
		public static AppendText(_path:string):any {return undefined;}
		public static Copy(_tneMapId:number,_sourceFileName:string,_destFileName:string):void;
		public static Copy(_tneMapId:number,_sourceFileName:string,_destFileName:string,_overwrite:boolean):void;
		@Tnelab.InvokeInfo("Copy", "System.String","System.String","System.Boolean")
		@Tnelab.InvokeInfo("Copy", "System.String","System.String")
		public static Copy(tneMapId:number):any{}
		public static Create(_tneMapId:number,_path:string):any;
		public static Create(_tneMapId:number,_path:string,_bufferSize:number):any;
		public static Create(_tneMapId:number,_path:string,_bufferSize:number,_options:any):any;
		public static Create(_tneMapId:number,_path:string,_bufferSize:number,_options:any,_fileSecurity:any):any;
		@Tnelab.InvokeInfo("Create", "System.String","System.Int32","System.IO.FileOptions","System.Security.AccessControl.FileSecurity")
		@Tnelab.InvokeInfo("Create", "System.String","System.Int32","System.IO.FileOptions")
		@Tnelab.InvokeInfo("Create", "System.String","System.Int32")
		@Tnelab.InvokeInfo("Create", "System.String")
		public static Create(tneMapId:number):any{}
		@Tnelab.InvokeInfo("Delete", "System.String")
		public static Delete(_path:string):void {}
		@Tnelab.InvokeInfo("Exists", "System.String")
		public static Exists(_path:string):boolean {return undefined;}
		public static Open(_tneMapId:number,_path:string,_mode:any):any;
		public static Open(_tneMapId:number,_path:string,_mode:any,_access:any):any;
		public static Open(_tneMapId:number,_path:string,_mode:any,_access:any,_share:any):any;
		@Tnelab.InvokeInfo("Open", "System.String","System.IO.FileMode","System.IO.FileAccess","System.IO.FileShare")
		@Tnelab.InvokeInfo("Open", "System.String","System.IO.FileMode","System.IO.FileAccess")
		@Tnelab.InvokeInfo("Open", "System.String","System.IO.FileMode")
		public static Open(tneMapId:number):any{}
		@Tnelab.InvokeInfo("SetCreationTime", "System.String","System.DateTime")
		public static SetCreationTime(_path:string,_creationTime:any):void {}
		@Tnelab.InvokeInfo("SetCreationTimeUtc", "System.String","System.DateTime")
		public static SetCreationTimeUtc(_path:string,_creationTimeUtc:any):void {}
		@Tnelab.InvokeInfo("GetCreationTime", "System.String")
		public static GetCreationTime(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("GetCreationTimeUtc", "System.String")
		public static GetCreationTimeUtc(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("SetLastAccessTime", "System.String","System.DateTime")
		public static SetLastAccessTime(_path:string,_lastAccessTime:any):void {}
		@Tnelab.InvokeInfo("SetLastAccessTimeUtc", "System.String","System.DateTime")
		public static SetLastAccessTimeUtc(_path:string,_lastAccessTimeUtc:any):void {}
		@Tnelab.InvokeInfo("GetLastAccessTime", "System.String")
		public static GetLastAccessTime(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("GetLastAccessTimeUtc", "System.String")
		public static GetLastAccessTimeUtc(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("SetLastWriteTime", "System.String","System.DateTime")
		public static SetLastWriteTime(_path:string,_lastWriteTime:any):void {}
		@Tnelab.InvokeInfo("SetLastWriteTimeUtc", "System.String","System.DateTime")
		public static SetLastWriteTimeUtc(_path:string,_lastWriteTimeUtc:any):void {}
		@Tnelab.InvokeInfo("GetLastWriteTime", "System.String")
		public static GetLastWriteTime(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("GetLastWriteTimeUtc", "System.String")
		public static GetLastWriteTimeUtc(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("GetAttributes", "System.String")
		public static GetAttributes(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("SetAttributes", "System.String","System.IO.FileAttributes")
		public static SetAttributes(_path:string,_fileAttributes:any):void {}
		public static GetAccessControl(_tneMapId:number,_path:string):any;
		public static GetAccessControl(_tneMapId:number,_path:string,_includeSections:any):any;
		@Tnelab.InvokeInfo("GetAccessControl", "System.String","System.Security.AccessControl.AccessControlSections")
		@Tnelab.InvokeInfo("GetAccessControl", "System.String")
		public static GetAccessControl(tneMapId:number):any{}
		@Tnelab.InvokeInfo("SetAccessControl", "System.String","System.Security.AccessControl.FileSecurity")
		public static SetAccessControl(_path:string,_fileSecurity:any):void {}
		@Tnelab.InvokeInfo("OpenRead", "System.String")
		public static OpenRead(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("OpenWrite", "System.String")
		public static OpenWrite(_path:string):any {return undefined;}
		public static ReadAllText(_tneMapId:number,_path:string):string;
		public static ReadAllText(_tneMapId:number,_path:string,_encoding:any):string;
		@Tnelab.InvokeInfo("ReadAllText", "System.String","System.Text.Encoding")
		@Tnelab.InvokeInfo("ReadAllText", "System.String")
		public static ReadAllText(tneMapId:number):any{}
		public static WriteAllText(_tneMapId:number,_path:string,_contents:string):void;
		public static WriteAllText(_tneMapId:number,_path:string,_contents:string,_encoding:any):void;
		@Tnelab.InvokeInfo("WriteAllText", "System.String","System.String","System.Text.Encoding")
		@Tnelab.InvokeInfo("WriteAllText", "System.String","System.String")
		public static WriteAllText(tneMapId:number):any{}
		@Tnelab.InvokeInfo("ReadAllBytes", "System.String")
		public static ReadAllBytes(_path:string):any {return undefined;}
		@Tnelab.InvokeInfo("WriteAllBytes", "System.String","System.Byte[]")
		public static WriteAllBytes(_path:string,_bytes:any):void {}
		public static ReadAllLines(_tneMapId:number,_path:string):any;
		public static ReadAllLines(_tneMapId:number,_path:string,_encoding:any):any;
		@Tnelab.InvokeInfo("ReadAllLines", "System.String","System.Text.Encoding")
		@Tnelab.InvokeInfo("ReadAllLines", "System.String")
		public static ReadAllLines(tneMapId:number):any{}
		public static ReadLines(_tneMapId:number,_path:string):any;
		public static ReadLines(_tneMapId:number,_path:string,_encoding:any):any;
		@Tnelab.InvokeInfo("ReadLines", "System.String","System.Text.Encoding")
		@Tnelab.InvokeInfo("ReadLines", "System.String")
		public static ReadLines(tneMapId:number):any{}
		public static WriteAllLines(_tneMapId:number,_path:string,_contents:any):void;
		public static WriteAllLines(_tneMapId:number,_path:string,_contents:any,_encoding:any):void;
		public static WriteAllLines(_tneMapId:number,_path:string,_contents:any):void;
		public static WriteAllLines(_tneMapId:number,_path:string,_contents:any,_encoding:any):void;
		@Tnelab.InvokeInfo("WriteAllLines", "System.String","System.Collections.Generic.IEnumerable<System.String>","System.Text.Encoding")
		@Tnelab.InvokeInfo("WriteAllLines", "System.String","System.Collections.Generic.IEnumerable<System.String>")
		@Tnelab.InvokeInfo("WriteAllLines", "System.String","System.String[]","System.Text.Encoding")
		@Tnelab.InvokeInfo("WriteAllLines", "System.String","System.String[]")
		public static WriteAllLines(tneMapId:number):any{}
		public static AppendAllText(_tneMapId:number,_path:string,_contents:string):void;
		public static AppendAllText(_tneMapId:number,_path:string,_contents:string,_encoding:any):void;
		@Tnelab.InvokeInfo("AppendAllText", "System.String","System.String","System.Text.Encoding")
		@Tnelab.InvokeInfo("AppendAllText", "System.String","System.String")
		public static AppendAllText(tneMapId:number):any{}
		public static AppendAllLines(_tneMapId:number,_path:string,_contents:any):void;
		public static AppendAllLines(_tneMapId:number,_path:string,_contents:any,_encoding:any):void;
		@Tnelab.InvokeInfo("AppendAllLines", "System.String","System.Collections.Generic.IEnumerable<System.String>","System.Text.Encoding")
		@Tnelab.InvokeInfo("AppendAllLines", "System.String","System.Collections.Generic.IEnumerable<System.String>")
		public static AppendAllLines(tneMapId:number):any{}
		@Tnelab.InvokeInfo("Move", "System.String","System.String")
		public static Move(_sourceFileName:string,_destFileName:string):void {}
		public static Replace(_tneMapId:number,_sourceFileName:string,_destinationFileName:string,_destinationBackupFileName:string):void;
		public static Replace(_tneMapId:number,_sourceFileName:string,_destinationFileName:string,_destinationBackupFileName:string,_ignoreMetadataErrors:boolean):void;
		@Tnelab.InvokeInfo("Replace", "System.String","System.String","System.String","System.Boolean")
		@Tnelab.InvokeInfo("Replace", "System.String","System.String","System.String")
		public static Replace(tneMapId:number):any{}
		@Tnelab.InvokeInfo("Decrypt", "System.String")
		public static Decrypt(_path:string):void {}
		@Tnelab.InvokeInfo("Encrypt", "System.String")
		public static Encrypt(_path:string):void {}
		@Tnelab.InvokeInfo("Equals", "System.Object")
		public  Equals(_obj:any):boolean {return undefined;}
		@Tnelab.InvokeInfo("GetHashCode")
		public  GetHashCode():number {return undefined;}
		@Tnelab.InvokeInfo("GetType")
		public  GetType():any {return undefined;}
		@Tnelab.InvokeInfo("ToString")
		public  ToString():string {return undefined;}
		public constructor() {super(arguments);}
	}
	Tnelab.RegisterNativeMapAsync("System.IO.File","TMiniblink.File");
}
