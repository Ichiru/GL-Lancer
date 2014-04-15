#!/bin/bash
processor="mono ./GLSLProcessor/bin/Debug/GLSLProcessor.exe"
ShaderDir=./ShadersGL
OutputDir=./Debug/Assets/effects
# materials
$processor $ShaderDir/DcDt.ini $OutputDir/materials/DcDt.effect
$processor $ShaderDir/DcDtEc.ini $OutputDir/materials/DcDtEc.effect
$processor $ShaderDir/Nebula.ini $OutputDir/materials/Nebula.effect
$processor $ShaderDir/DcDtEcOcOt.ini $OutputDir/materials/DcDtEcOcOt.effect
$processor $ShaderDir/Masked2DetailMapMaterial.ini $OutputDir/materials/Masked2DetailMapMaterial.effect
$processor $ShaderDir/AtmosphereMaterial.ini $OutputDir/materials/AtmosphereMaterial.effect
# other effects
$processor $ShaderDir/Planet.ini $OutputDir/Planet.effect
