#!/bin/bash
processor="mono ./GLSLProcessor/bin/Debug/GLSLProcessor.exe"
ShaderDir=./ShadersGL
OutputDir=./Debug/Assets/effects
$processor $ShaderDir/DcDt.ini $OutputDir/materials/DcDt.effect
$processor $ShaderDir/DcDtEc.ini $OutputDir/materials/DcDtEc.effect
$processor $ShaderDir/Nebula.ini $OutputDir/materials/Nebula.effect

