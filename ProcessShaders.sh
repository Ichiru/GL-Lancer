#!/bin/bash
processor="mono ./GLSLProcessor/bin/Debug/GLSLProcessor.exe"
ShaderDir=./ShadersGL
OutputDir=./Debug/Assets/effects
# materials
$processor $ShaderDir/DcDt.ini $OutputDir/materials/DcDt.effect
$processor $ShaderDir/DcDtEc.ini $OutputDir/materials/DcDtEc.effect
$processor $ShaderDir/DcDtEt.ini $OutputDir/materials/DcDtEt.effect
$processor $ShaderDir/DcDtTwo.ini $OutputDir/materials/DcDtTwo.effect
$processor $ShaderDir/DcDtOcOt.ini $OutputDir/materials/DcDtOcOt.effect
$processor $ShaderDir/DcDtOcOtTwo.ini $OutputDir/materials/DcDtOcOtTwo.effect
$processor $ShaderDir/Nebula.ini $OutputDir/materials/Nebula.effect
$processor $ShaderDir/DcDtEcOcOt.ini $OutputDir/materials/DcDtEcOcOt.effect
$processor $ShaderDir/Masked2DetailMapMaterial.ini $OutputDir/materials/Masked2DetailMapMaterial.effect
$processor $ShaderDir/DetailMap2Dm1Msk2PassMaterial.ini $OutputDir/materials/DetailMap2Dm1Msk2PassMaterial.effect
$processor $ShaderDir/DetailMapMaterial.ini $OutputDir/materials/DetailMapMaterial.effect
$processor $ShaderDir/AtmosphereMaterial.ini $OutputDir/materials/AtmosphereMaterial.effect
# other effects
$processor $ShaderDir/Planet.ini $OutputDir/Planet.effect
