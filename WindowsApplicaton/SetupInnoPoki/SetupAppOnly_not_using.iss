; -- Example3.iss --
; Same as Example1.iss, but creates some registry entries too.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=Stem+
AppVersion=2.0
DefaultDirName={pf}\Stem+
AppPublisher=Stem+
AppPublisherURL=http://stemplus.vn/
DefaultGroupName=Stem+
SetupIconFile=favicon.ico
WizardImageFile=logobig.bmp
WizardSmallImageFile=logo1.bmp
UninstallDisplayIcon={app}\StemPlus.exe
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
Name: "{app}\App_data"; Permissions: everyone-full;

[Files]
Source: "*.ico"; DestDir: "{app}"
Source: "*.dll"; DestDir: "{app}";Permissions: everyone-full
Source: "*.exe"; DestDir: "{app}" ;Permissions: everyone-full
Source: "*.config"; DestDir: "{app}" ;Permissions: everyone-full
Source: "Firefox\*"; DestDir: "{app}\Firefox";Permissions: everyone-full;
Source: "Pdf\*"; DestDir: "{app}\Pdf";Permissions: everyone-full;
Source: "App_data\stem_plus.sdf"; DestDir: "{app}\App_data";Permissions: everyone-full;Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
Source: "dependencies\TeamViewerQS.exe"; DestDir: {app}; Permissions: everyone-full;
Source: "dependencies\Teamviewer.ico"; DestDir: {app}; Permissions: everyone-full;

[Files]
Source: "Firefox\*.dll"; DestDir: "{sys}"; Flags: onlyifdoesntexist sharedfile ignoreversion recursesubdirs createallsubdirs;Permissions: everyone-full
Source: "Firefox\*.dll"; DestDir: "{syswow64}"; Flags: onlyifdoesntexist sharedfile 64bit ignoreversion recursesubdirs createallsubdirs;Permissions: everyone-full; Check: IsWin64;

[Icons] 
Name: {group}\Stem+; Filename: {app}\StemPlus.exe; WorkingDir: {app}; IconFilename: {app}\favicon.ico; Comment: "Stem+";
Name: {commondesktop}\Stem+; Filename: {app}\StemPlus.exe; WorkingDir: {app}; IconFilename: {app}\favicon.ico; Comment: "Stem+"; 
Name: {commondesktop}\TeamViewerQS; Filename: {app}\TeamViewerQS.exe; WorkingDir: {app}; IconFilename: {app}\Teamviewer.ico; Comment: "TeamViewerQS";

[Registry]
Root: HKCU; Subkey: "Software\Stem+"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Stem+"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Stem+\Settings"; ValueType: string; ValueName: "Path"; ValueData: "{app}"
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: String; ValueName: "{app}\StemPlus.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 0,6.1
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; ValueType: String; ValueName: "{app}\TeamViewerQS.exe"; ValueData: "RUNASADMIN"; Flags: uninsdeletekeyifempty uninsdeletevalue; MinVersion: 0,6.1

[UninstallDelete]
Type: dirifempty; Name: "{app}";

[Code]
var CancelWithoutPrompt: boolean;

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