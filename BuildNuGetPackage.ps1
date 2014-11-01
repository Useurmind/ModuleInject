Function GetVersion() {
	 [Int32]$version = Get-Content .\ModuleInject.Version.txt
	 [Int32]$incVersion = $version+1
	 Set-Content .\ModuleInject.Version.txt $incVersion
	 return $version
}

Write-Host "Calculating new minor version number"

$minorVersion = GetVersion

Write-Host "Minor version will be $minorVersion"

Write-Host "Filling .nuspec file"

[String]$nuspecTemplate = Get-Content .\ModuleInject.nuspec.template

[String]$nuspecContent = $nuspecTemplate.Replace("@minorVersion@", $minorVersion)

Set-Content .\ModuleInject\ModuleInject.nuspec $nuspecContent

[string]$completeVersion =  [regex]::match($nuspecContent,"\<version\>.*\<\/version\>").ToString()

$completeVersion = $completeVersion.Trim("<version>").Trim("</version>")

$nugetPackage = "ModuleInject.$completeVersion.nupkg"

$result = mkdir "NuGetPackages" -Force
$packCommand = @'
nuget.exe Pack ModuleInject\ModuleInject.csproj -Prop Configuration='NuGet Package' -OutputDirectory NuGetPackages -includereferencedprojects
'@

$pushCommand = "nuget.exe Push NuGetPackages\$nugetPackage"

Write-Host "Packing NuGet package"
Invoke-Expression -Command:$packCommand
Write-Host "Pushing NuGet package"
Invoke-Expression -Command:$pushCommand
Write-Host "Done."

