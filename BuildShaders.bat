@echo off
echo Building Shaders...
cd Shaders
for /R %%f in (*.fx) do (
 	C:\Users\VirtualBox\Desktop\MonoGame-develop\Tools\MGCB\bin\Debug\mgcb /platform:Linux "/outputDir:bin" /intermediateDir:obj "/build:%%f" /quiet
	robocopy bin ..\GLLancer\Assets\effects /NFL /NDL /NJH /NJS /nc /ns /np
	erase bin /q
	erase obj /q
)
cd ..
