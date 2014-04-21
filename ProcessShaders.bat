@echo off
SET PROCESSOR=GLSLProcessor\bin\Debug\GLSLProcessor.exe
SET SHADER_DIR=ShadersGL
SET OUTPUT_DIR=Debug\Assets\effects
REM materials
%PROCESSOR% %SHADER_DIR%/DcDt.ini %OUTPUT_DIR%/materials/DcDt.effect
%PROCESSOR% %SHADER_DIR%/DcDtEc.ini %OUTPUT_DIR%/materials/DcDtEc.effect
%PROCESSOR% %SHADER_DIR%/DcDtEt.ini %OUTPUT_DIR%/materials/DcDtEt.effect
%PROCESSOR% %SHADER_DIR%/DcDtTwo.ini %OUTPUT_DIR%/materials/DcDtTwo.effect
%PROCESSOR% %SHADER_DIR%/DcDtOcOt.ini %OUTPUT_DIR%/materials/DcDtOcOt.effect
%PROCESSOR% %SHADER_DIR%/DcDtOcOtTwo.ini %OUTPUT_DIR%/materials/DcDtOcOtTwo.effect
%PROCESSOR% %SHADER_DIR%/Nebula.ini %OUTPUT_DIR%/materials/Nebula.effect
%PROCESSOR% %SHADER_DIR%/DcDtEcOcOt.ini %OUTPUT_DIR%/materials/DcDtEcOcOt.effect
%PROCESSOR% %SHADER_DIR%/Masked2DetailMapMaterial.ini %OUTPUT_DIR%/materials/Masked2DetailMapMaterial.effect
%PROCESSOR% %SHADER_DIR%/DetailMap2Dm1Msk2PassMaterial.ini %OUTPUT_DIR%/materials/DetailMap2Dm1Msk2PassMaterial.effect
%PROCESSOR% %SHADER_DIR%/DetailMapMaterial.ini %OUTPUT_DIR%/materials/DetailMapMaterial.effect
%PROCESSOR% %SHADER_DIR%/AtmosphereMaterial.ini %OUTPUT_DIR%/materials/AtmosphereMaterial.effect
REM other effects
%PROCESSOR% %SHADER_DIR%/Planet.ini %OUTPUT_DIR%/Planet.effect
