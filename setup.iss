; Inno Setup Script for Gangasiri Tea Factory Billing

#define MyAppName "GTF Billing"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Seran"
#define MyAppURL "https://example.com"
#define MyAppExeName "GTFBilling.exe"
#define MySourceDir "D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\bin\Release\net8.0-windows\publish\win-x64"

[Setup]
AppId={{AUTO_GUID}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\Setup
OutputBaseFilename=GangasiriBilling_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=img\app_icon.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MySourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Exclude the database file to ensure a fresh, empty one is created on first run
Source: "{#MySourceDir}\*"; DestDir: "{app}"; Excludes: "TeaFactoryDB.sqlite"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Fonts\*.ttf"; DestDir: "{autofonts}"; FontInstall: "Iskoola Pota"; Flags: uninsneveruninstall
Source: "img\app_icon.ico"; DestDir: "{app}";

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\app_icon.ico"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\app_icon.ico"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Fonts]
Source: "Fonts\Iskoola Pota Regular.ttf"; DestDir: "{autofonts}"; FontInstall: "Iskoola Pota Regular"; Flags: uninsneveruninstall
Source: "Fonts\nirmala-ui.ttf"; DestDir: "{autofonts}"; FontInstall: "Nirmala UI"; Flags: uninsneveruninstall
Source: "Fonts\NotoSansSinhala-VariableFont_wdth,wght.ttf"; DestDir: "{autofonts}"; FontInstall: "Noto Sans Sinhala"; Flags: uninsneveruninstall
