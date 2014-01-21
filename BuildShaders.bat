@echo off
echo Building Shaders...
cd Shaders
for /R %%f in (*.fx) do (
 	..\Third-Party\MGCB\mgcb /platform:Linux "/outputDir:bin" /intermediateDir:obj "/build:%%f" /quiet
	robocopy bin ..\GLLancer\Assets\effects /NFL /NDL /NJH /NJS /nc /ns /np
	erase bin /q
	erase obj /q
)
cd ..
