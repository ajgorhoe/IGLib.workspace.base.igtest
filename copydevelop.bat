
echo off

set basedir="shell"

echo " "
echo Copying all applications from directory %basedir% to the bin directory:
echo " "

CALL copyappneural.bat
CALL copyappmessy.bat
CALL copyappextended.bat

