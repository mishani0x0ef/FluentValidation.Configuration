Param(
	[string]$configuration = "Release"
)

Set-Location ..\

$nUnit = Get-ChildItem -Filter "nunit3-console.exe" -Recurse | Select-Object -First 1;
if([string]::IsNullOrEmpty($nUnit) -or !(Test-Path -Path $nUnit.FullName)) {
	throw "nUnit console runner missed. Include it into solution - https://www.nuget.org/packages/NUnit.ConsoleRunner/.";
}

# Get tests to run.
$testDirs = Get-ChildItem -Recurse -Directory | where {$_.Name -like "*.Tests"};
$testsCount = ($testDirs | measure).Count;

if($testsCount -gt 0) {
	Write-Host "Detected ${testsCount} project(s) with tests. Running tests...";
	if(!(Test-Path -Path ".build\temp")) {
		New-Item -ItemType Directory -Path ".build\temp";
	}	
}
else {
	Write-Host "No tests detected. Exit.";
	return;
}

$testFailed = $false;

# Run tests.
foreach($dir in $testDirs) {
	$projDir = $dir.FullName;
	$projName = $dir.BaseName;
	$assembly = "${projDir}\bin\${configuration}\${projName}.dll";
	$testResult = ".build\temp\${projName}.Result.xml";

	if(!(Test-Path -Path $assembly)) {
		Write-Warning "Cannot found assembly for testing by path '${assembly}'.";
		continue;
	}

	Write-Host "Run tests for '${projName}'. Results - '${testResult}'.";
	& $nUnit.FullName "/result:$testResult" "$assembly";

	if($LastExitCode -ne 0) {
		Write-Warning "Some tests from '${projName}' failed.";
		$testFailed = $true;
	}
}

if($testFailed) {
	throw "Some of tests was failed.";
}