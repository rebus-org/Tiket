@echo off

set version=%2
set currentdir=%~dp0
set root=%currentdir%\..
set toolsdir=%root%\tools
set ilmerge=%toolsdir%\Ilmerge\ilmerge.exe
set nuget=%toolsdir%\NuGet\NuGet.exe
set tiketdir=%root%\Tiket
set releasedir=%tiketdir%\bin\Release
set mergedir=%releasedir%\merged
set deploydir=%root%\deploy

if "%version%"=="" (
	echo Please specify which version to build as a parameter.
	echo.
	goto exit
)

echo This will build, tag, and release version %version% of Tiket.
echo.
echo Please make sure that all changes have been properly committed!
pause


if exist "%mergedir%" (
	echo Cleaning up old merge dir %mergedir%
	rd %mergedir% /s/q
)

if exist "%deploydir%" (
	echo Cleaning up old deploy dir %deploydir%
	rd %deploydir% /s/q
)

echo Building version %version%

"msbuild" "%tiketdir%\Tiket.csproj" "/p:Configuration=Release"

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

%ilmerge% /out:%mergedir%\Tiket.dll %releasedir%\Tiket.dll %releasedir%\Newtonsoft.Json.dll /targetplatform:"v4,%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /internalize

echo Packing...

echo Creating deploy dir %deploydir%
mkdir %deploydir%

%nuget% pack %tiketdir%\Tiket.nuspec -OutputDirectory %deploydir% -Version %version%

echo Tagging...

git tag %version%

echo Pushing to NuGet.org...

%nuget% push %deploydir%\*.nupkg

:exit