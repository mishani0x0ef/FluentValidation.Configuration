Param(
	[string]$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"
)

if([string]::IsNullOrEmpty($msbuild) -or !(Test-Path -Path $msbuild)) {
    Write-Error "MSBuild not found by path '${msbuild}'.";
    return;
}

# Confirmation.
$yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Publish the packages.";
$no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Does not publish the packages.";
$options = [System.Management.Automation.Host.ChoiceDescription[]]($no, $yes);
  
$result = $host.ui.PromptForChoice("Publish packages", "Do you want to upload the NuGet packages to the NuGet server?", $options, 0);

if($result -eq 0) {
    return "Publish was aborted.";
}

Write-Host "Start Publish packages.";

# Clean.

$currentDir = (Get-Item -Path ".\" -Verbose).FullName;
$tempFolder = "${currentDir}\temp";
if(Test-Path -Path $tempFolder) {
    Remove-Item -Path $tempFolder -Recurse;
}

try {
    # Build.
    & $msbuild "build.proj" "/p:Configuration=Release";
    if($LastExitCode -ne 0) {
        throw "Build failed.";
    }

    # Tests Running.
    & "./run-tests.ps1" "Release";

    # todo: add package deployment. MR
}
catch [System.Exception] {
    Write-Error "Publish process was terminated because of errors.";
}
finally {
    if(Test-Path -Path $tempFolder) {
        Remove-Item -Path $tempFolder -Recurse;
    }
}
