#include <idp.iss>

[Setup]
AppName=Stem+
AppVersion=2.0.1.1
DefaultDirName={pf}\Stem+
AppPublisher=Stem+
AppPublisherURL=http://stemplus.vn/
DefaultGroupName=Stem+
SetupIconFile=favicon.ico
WizardImageFile=logobig.bmp
WizardSmallImageFile=logo1.bmp
UninstallDisplayIcon={app}\StemPlus.exe
OutputBaseFilename=StemPlus_Setup_2.0.1.1
UninstallDisplayName=Stem+
CreateUninstallRegKey=yes
OutputDir=userdocs:Inno Setup Examples Output

[UninstallDelete]
;This works only if it is installed in default location
Type: filesandordirs; Name: {pf}\Stem+
[InstallDelete]
;Type: filesandordirs; Name: {pf}\Stem+
 Type: files; Name: {app}\AppServices.dll
 Type: files; Name: {app}\Repositories.dll
 Type: files; Name: {app}\Model.dll

[Languages]
Name: Vietnamese; MessagesFile: "Vietnamese.isl"

[Dirs]
Name: "{app}"; Permissions: everyone-full
Name: "{app}\App_data"; Permissions: everyone-full

[Files]
Source: "*.ico"; DestDir: "{app}"
Source: "*.dll"; DestDir: "{app}";Permissions: everyone-full
Source: "*.exe"; DestDir: "{app}" ;Permissions: everyone-full
Source: "*.config"; DestDir: "{app}" ;Permissions: everyone-full
Source: "Pdf\*"; DestDir: "{app}\Pdf";Permissions: everyone-full;
Source: "App_data\stem_plus.sdf"; DestDir: "{app}\App_data";Permissions: everyone-full;Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
Source: "dependencies\UltraViewerQS.exe"; DestDir: {app}; Permissions: everyone-full;

[Icons] 
Name: {group}\Stem+; Filename: {app}\StemPlus.exe; WorkingDir: {app}; IconFilename: {app}\favicon.ico; Comment: "Stem+";
Name: {commondesktop}\Stem+; Filename: {app}\StemPlus.exe; WorkingDir: {app}; IconFilename: {app}\favicon.ico; Comment: "Stem+"; 
Name: {commondesktop}\UltraViewerQS; Filename: {app}\UltraViewerQS.exe; WorkingDir: {app};

[Registry]
Root: HKCU; Subkey: "Software\Stem+"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Stem+"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Stem+\Settings"; ValueType: string; ValueName: "Path"; ValueData: "{app}"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: String; ValueName: "{app}\StemPlus.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; 
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: String; ValueName: "{app}\UltraViewerQS.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; 

[UninstallDelete]
Type: dirifempty; Name: "{app}";

[Code]
const  
  FrameworkURL = 'https://back.stemplus.vn/dependencies/NDP48-x86-x64-AllOS-ENU.exe';
  FrameworkInstallerFileName = '{tmp}\NDP48-x86-x64-AllOS-ENU.exe';     
  Framework48ReleaseVersion = 528040;// Giá trị Release tương ứng với .NET Framework 4.8 là 528040
    
  Sql64URL = 'https://back.stemplus.vn/dependencies/SSCERuntime_x64-ENU.exe';
  Sql64InstallerFileName = '{tmp}\SSCERuntime_x64-ENU.exe';   
  
  Sql86URL = 'https://back.stemplus.vn/dependencies/SSCERuntime_x86-ENU.exe';
  Sql86InstallerFileName = '{tmp}\SSCERuntime_x86-ENU.exe'; 
  
  //Visual C++ 2005
  VisualCUrl = 'https://back.stemplus.vn/dependencies/VC_redist.x86.exe';
  VisualCInstallerName = '{tmp}\VC_redist.x86.exe';

  //Microsoft Edge WebView2 Runtime (cần cho StemPlus.exe hiển thị nội dung bài học HTML5/WebGL)
  WebView2ClientId = '{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}';

  WebView2X64Url = 'https://back.stemplus.vn/dependencies/MicrosoftEdgeWebView2RuntimeInstallerX64.exe';
  WebView2X64InstallerName = '{tmp}\MicrosoftEdgeWebView2RuntimeInstallerX64.exe';

  WebView2X86Url = 'https://back.stemplus.vn/dependencies/MicrosoftEdgeWebView2RuntimeInstallerX86.exe';
  WebView2X86InstallerName = '{tmp}\MicrosoftEdgeWebView2RuntimeInstallerX86.exe';

var CancelWithoutPrompt: boolean;

//-----------------các hàm kiểm tra môi trường trong REGISTRY
function IsVC2005Installed: Boolean;
begin
  // Kiểm tra xem phiên bản VC++ 2005 Redistributable x86 đã được cài đặt chưa
  // để lấy đúng phiên bản, xem list sau: https://zzz.buzz/notes/vc-redist-packages-and-related-registry-entries/
  Result := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{ad8a2fa1-06e7-4b0d-927d-6e54b3d31028}') or
            RegKeyExists(HKLM, 'SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{710f4c1c-cc18-4c49-8cbf-51240c89a1a2}');
end;

function IsFrameworkInstalled: Boolean;
var
    ReleaseVersion: Cardinal;
begin  
    // Kiểm tra khóa registry và lấy giá trị Release
    if RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', ReleaseVersion) then
    begin
        // Kiểm tra nếu phiên bản lớn hơn hoặc bằng 528040 (tức là .NET Framework 4.8 hoặc cao hơn)
        Result := ReleaseVersion >= Framework48ReleaseVersion;
    end
    else
    begin
        // Trả về False nếu không tìm thấy khóa registry hoặc giá trị Release
        Result := False;
    end;
end;

function IsSqlInstalled: Boolean;
var
  IsInstalled: Boolean;
begin
  // Kiểm tra sql server compact edition phiên bản 4.0
  IsInstalled := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Microsoft SQL Server Compact Edition\v4.0');  //check 32bit
  if not IsInstalled then
    IsInstalled := RegKeyExists(HKLM, 'SOFTWARE\WOW6432Node\Microsoft\Microsoft SQL Server Compact Edition\v4.0');  //64-bit

  Result := IsInstalled;
end;

function IsWebView2Installed: Boolean;
begin
  // Runtime được đăng ký per-machine dưới key EdgeUpdate Clients; kiểm tra cả 2 vị trí
  // vì Inno Setup là process 32-bit nên bị Windows tự redirect sang WOW6432Node trên OS 64-bit
  Result := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\EdgeUpdate\Clients\' + WebView2ClientId) or
            RegKeyExists(HKLM, 'SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\' + WebView2ClientId);
end;

//-----------------các hàm kiểm tra môi trường trong REGISTRY

//-------các hàm install

procedure InstallFramework;
var
  ResultCode: Integer;
  StatusText: string;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Ðang cài .NET framework 4.8...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
      if not Exec(ExpandConstant(FrameworkInstallerFileName), '/q /norestart', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin    
    MsgBox('Lỗi cài đặt Microsoft Windows Desktop .NET framework 4.8: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
    CancelWithoutPrompt := true;    
  end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

procedure InstallSQL;
var
  ResultCode: Integer;
  StatusText: string;
  InstallFilePath: string;
begin
  try
    StatusText := WizardForm.StatusLabel.Caption;
    WizardForm.StatusLabel.Caption := 'Ðang cài cơ sở dữ liệu...';
    WizardForm.ProgressGauge.Style := npbstMarquee;
    if IsWin64 then
    begin      
      InstallFilePath := ExpandConstant(Sql64InstallerFileName);             
    end
    else
    begin      
      InstallFilePath := ExpandConstant(Sql86InstallerFileName);
    end;
    
    if not Exec(InstallFilePath, '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin          
      MsgBox('Lỗi cài đặt cơ sở dữ liệu: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
      CancelWithoutPrompt := true;
      WizardForm.Close;       
    end;  
  finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

procedure InstallVC2005;
var
  ResultCode: Integer;
  StatusText: string;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Ðang cài Microsoft Visual C++ 2005 Redistributable...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    if not Exec(ExpandConstant(VisualCInstallerName), '/install /quiet /norestart', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    begin    
      MsgBox('Lỗi cài đặt Microsoft Visual C++ 2005 Redistributable: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
      CancelWithoutPrompt := true;    
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

procedure InstallWebView2;
var
  ResultCode: Integer;
  StatusText: string;
  InstallFilePath: string;
begin
  try
    StatusText := WizardForm.StatusLabel.Caption;
    WizardForm.StatusLabel.Caption := 'Đang cài Microsoft Edge WebView2 Runtime...';
    WizardForm.ProgressGauge.Style := npbstMarquee;
    if IsWin64 then
    begin
      InstallFilePath := ExpandConstant(WebView2X64InstallerName);
    end
    else
    begin
      InstallFilePath := ExpandConstant(WebView2X86InstallerName);
    end;

    if not Exec(InstallFilePath, '/silent /install', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    begin
      MsgBox('Lỗi cài đặt Microsoft Edge WebView2 Runtime: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
      CancelWithoutPrompt := true;
      WizardForm.Close;
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;
//----------------------

function InitializeSetup(): Boolean;
begin
  CancelWithoutPrompt := false;
  result := true;
end;

procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  if CurPageID=wpInstalling then
    Confirm := not CancelWithoutPrompt;
end;

procedure InitializeWizard();
begin
    //download
    if not IsVC2005Installed then
    begin
      idpAddFile(VisualCUrl, ExpandConstant(VisualCInstallerName));
    end;
    
    if not IsFrameworkInstalled then
    begin
      idpAddFile(FrameworkURL, ExpandConstant(FrameworkInstallerFileName));
    end;
    
    if not IsSqlInstalled then
    begin
      if IsWin64 then
      begin 
        idpAddFile(Sql64URL, ExpandConstant(Sql64InstallerFileName));
      end
      else
      begin
        idpAddFile(Sql86URL, ExpandConstant(Sql86InstallerFileName));
      end
    end;

    if not IsWebView2Installed then
    begin
      if IsWin64 then
      begin
        idpAddFile(WebView2X64Url, ExpandConstant(WebView2X64InstallerName));
      end
      else
      begin
        idpAddFile(WebView2X86Url, ExpandConstant(WebView2X86InstallerName));
      end
    end;

    idpDownloadAfter(wpReady);
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
begin
  // Khi bước cài đặt chính hoàn tất
  if CurStep = ssPostInstall then
  begin
    //visual C++
    if not IsVC2005Installed then 
    begin      
      InstallVC2005();
    end;
    
    // Kiểm tra xem Framework đã được cài đặt chưa
    if not IsFrameworkInstalled then 
    begin      
      InstallFramework();
    end;
    
    // Kiểm tra SQL
    if not IsSqlInstalled then
    begin
      InstallSQL();
    end;

    // Kiểm tra Microsoft Edge WebView2 Runtime
    if not IsWebView2Installed then
    begin
      InstallWebView2();
    end;
  end;
end;
