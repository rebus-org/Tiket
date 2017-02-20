@echo off

set name=%1
set version=%2
set reporoot=%~dp0\..

set tiketdir=%reporoot%\Tiket
set releasedir=%tiketdir%\bin\Release
set mergedir=%releasedir%\merged

if "%name%"=="" (
  echo Please remember to specify the name to build as an argument.
  goto exit_fail
)

if "%version%"=="" (
  echo Please remember to specify which version to build as an argument.
  goto exit_fail
)

if exist "%mergedir%" (
	echo Cleaning up old merge dir %mergedir%
	rd %mergedir% /s/q
)

set msbuild=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe

if not exist "%msbuild%" (
  echo Could not find MSBuild here:
  echo.
  echo   "%msbuild%"
  echo.
  goto exit_fail
)

set sln=%reporoot%\%name%.sln

if not exist "%sln%" (
  echo Could not find SLN to build here:
  echo.
  echo    "%sln%"
  echo.
  goto exit_fail
)

set nuget=%reporoot%\tools\NuGet\NuGet.exe

if not exist "%nuget%" (
  echo Could not find NuGet here:
  echo.
  echo    "%nuget%"
  echo.
  goto exit_fail
)

set ilmerge=%reporoot%\tools\Ilmerge\ilmerge.exe

if not exist "%ilmerge%" (
  echo Could not find IlMerge here:
  echo.
  echo    "%ilmerge%"
  echo.
  goto exit_fail
)

set destination=%reporoot%\deploy

if exist "%destination%" (
  rd "%destination%" /s/q
)

mkdir "%destination%"
if %ERRORLEVEL% neq 0 goto exit_fail

"%msbuild%" "%sln%" /p:Configuration=Release /t:rebuild
if %ERRORLEVEL% neq 0 goto exit_fail

echo Creating merge dir %mergedir%
mkdir %mergedir%

echo Merging
echo.
echo     %releasedir%\Tiket.dll
echo     %releasedir%\Newtonsoft.Json.dll
echo.
echo into
echo.
echo     %mergedir%\Tiket.dll
echo.

"%ilmerge%" "/out:%mergedir%\Tiket.dll" "%releasedir%\Tiket.dll" "%releasedir%\Newtonsoft.Json.dll" /targetplatform:"v4,%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /internalize
if %ERRORLEVEL% neq 0 goto exit_fail

"%nuget%" pack "%name%\%name%.nuspec" -OutputDirectory "%destination%" -Version %version%
if %ERRORLEVEL% neq 0 goto exit_fail

goto exit




:exit_fail

exit /b 1



:exit