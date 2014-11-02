Function GetVersion() {
	 [Int32]$version = Get-Content .\ModuleInject.Version.txt
	 [Int32]$incVersion = $version+1
	 Set-Content .\ModuleInject.Version.txt $incVersion
	 return $version
}

$packageNames = @(
	"ModuleInject.Interfaces",
	"ModuleInject.Common",
	"ModuleInject"
)

Write-Host "Calculating new minor version number"

$minorVersion = GetVersion

Write-Host "Minor version will be $minorVersion"

Write-Host "Filling .nuspec files"

[String]$nuspecTemplate = Get-Content .\ModuleInject.nuspec.template

[String]$nuspecContent = $nuspecTemplate.Replace("@minorVersion@", $minorVersion)
[string]$completeVersion =  [regex]::match($nuspecContent,"\<version\>.*\<\/version\>").ToString()
$completeVersion = $completeVersion.Trim("<version>").Trim("</version>")

$result = mkdir "NuGetPackages" -Force

ForEach( $packageName in $packageNames ) { 
	$finalNuspecContent = $nuspecContent.Replace("@title@", $packageName)
	Set-Content ".\$packageName\$packageName.nuspec" $finalNuspecContent
}

Write-Host "Creating NuGet packages"

$pushCommands = @()
ForEach( $packageName in $packageNames ) { 

	$nugetPackage = "$packageName.$completeVersion.nupkg"
	$packCommand = @'
nuget.exe Pack $packageName\$packageName.csproj -Prop Configuration='NuGet Package' -OutputDirectory NuGetPackages -includereferencedprojects
'@

	$pushCommand = "nuget.exe Push NuGetPackages\$nugetPackage"
	$pushCommands = $pushCommands + $pushCommand
	
	Write-Host "Packing NuGet package $packageName"
	Invoke-Expression -Command:$packCommand
}

Write-Host "Pushing NuGet packages"

ForEach( $pushCommand in $pushCommands ) { 
	Write-Host "Pushing NuGet package $packageName"
	Invoke-Expression -Command:$pushCommand
}

Write-Host "Done."

