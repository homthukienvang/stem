using System.Reflection;
using System.Runtime.InteropServices;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("StemPlus")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("StemPlus")]
[assembly: AssemblyProduct("StemPlus")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("84033f3e-7754-439b-939b-9f102b8768af")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("2.0.1.1")]
[assembly: AssemblyFileVersion("2.0.1.1")]
//log
//2024-10-03: 2.0.1.1: Sửa Icon và đổi Ultraview thay cho Teamview
//2022-03-21: 2.0.1.0: Thêm bảng tài liệu download, hoàn thiện toàn bộ cấu trúc bảng cơ bản mới nhất và xóa tất cả các file đã được tải cũ + reset toàn bộ client DB.
//2022-03-21: 2.0.0.7: nâng cấp phiên bản Log4net,Gecko firefox. Thêm Package DeviceId để thay thế MacIp cũ khi GENERATE bị lỗi.
//2021-12-20: 2.0.0.7: thêm giao diện "Tài liệu" chứa các mục PDF để view+tải+in
//2021-06-14: 2.0.0.6: thay đổi giao diện
//2021-03-15: 2.0.0.5: update resources Firefox from outside test fix special error
//2021-03-12: 2.0.0.4: remove flash object, change to using Geckofx60.32
