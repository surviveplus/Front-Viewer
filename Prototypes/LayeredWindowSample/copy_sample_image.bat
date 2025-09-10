@echo off
echo Copying sample.png to build output directories...

echo Copying to AppNet8...
if not exist "AppNet8\bin\Debug\net8.0\" mkdir "AppNet8\bin\Debug\net8.0\"
copy "sample.png" "AppNet8\bin\Debug\net8.0\"

echo Copying to AppDotNetFramework48...
if not exist "AppDotNetFramework48\bin\Debug\net48\" mkdir "AppDotNetFramework48\bin\Debug\net48\"
copy "sample.png" "AppDotNetFramework48\bin\Debug\net48\"

echo Done!
pause