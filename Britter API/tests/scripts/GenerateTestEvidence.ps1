$Banner = @"
               _   ______________   ______                                               
              / | / / ____/_  __/  / ____/___ _   _____  _________ _____ ____            
             /  |/ / __/   / /    / /   / __ \ | / / _ \/ ___/ __  `/ __  `/ _ \           
          _ / /|  / /___  / /    / /___/ /_/ / |/ /  __/ /  / /_/ / /_/ /  __/           
         (_)_/ |_/_____/ /_/     \____/\____/|___/\___/_/   \__,_/\__, /\___/            
                                                                 /____/                  
                                                                         V2.0            
"@

###########################################################################################################
#  This script runs the soultion's test projects and outputs the test results and coverage HTML reports.  #
###########################################################################################################
#                                                                                                         #
#  Required NuGet packages to be referenced in each xUnit test project:                                   #
#  - coverlet.collector                                                                                   #
#  - Microsoft.NET.Test.Sdk                                                                               #
#  - xunit                                                                                                #
#  - xunit.runner.visualstudio                                                                            #
#  - ReportGenerator                                                                                      #
#                                                                                                         #
#  Change log:                                                                                            #
#                                                                                                         #
#  V1.0 - Inital release.                                                                                 #
#  V2.0 - Replaced coverlet.msbuild with coverlet.collector.                                              #
#         Visual Studio 17.4.0 introduced a breaking change where '/:p' parameters are no longer passed   #
#         to coverlet via 'dotnet test'. Coverlet.collector now supports the required features that were  #
#         implemented in coverlet.msbuild.                                                                #
#                                                                                                         #
#         A Coverlet.runsettings.xml file must be created and placed in a 'Scripts' folder at the root    #
#         of the solution. More information can be found here:                                            #
#         https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md    #
#                                                                                                         #
###########################################################################################################


###############
#  Variables  #
###############
$Title = "Example Project Code Coverage Report"
$Solution = "$PSScriptRoot/../.."
$CodeQuality = "$Solution/tests/CodeQuality"
$CoverageResults = "$CodeQuality/Coverage/CoverageResults/"
$CoverageReport = "$CodeQuality/Coverage/CoverageReport/"
$CoverageHistory = "$CodeQuality/Coverage/CoverageHistory/"
$TestResults = "$Solution/tests/CodeQuality/TestResults/"
$Trxer = "$PSScriptRoot/TrxerConsole.exe"
$TestingEvidenceZip = "$CodeQuality/TestingEvidence.zip"


###############
#  Functions  #
###############
# Writes the specified string to the console
function ToConsole { param ([string] $String) 
   Write-Host -ForegroundColor Green -BackgroundColor Black $String 
}

# Deletes the specified folder
function DeleteFolder { param ([string] $Path)
      if(Test-Path -Path $($Path)){
         Remove-Item "$($Path)" -Recurse
   }
}

# Gets the latest NuGet package version
function GetLatestNuGetVersion { param ([string] $PackageName)
    $folder = Get-ChildItem "$home/.nuget/packages/$($PackageName)" -Directory | sort -Property Name -Descending | Select -First 1
    return $folder.Name
}

#####################
#  Package options  #
#####################
# Report Generator options
$reportGeneratorOptions = New-Object PSObject -Property @{
    Title = $Title
    Version = "[PLACE HOLDER]"
    CyclomaticComplexityThreshold = "10"
    ReportTypes = "Html;Badges;MarkdownSummary"
    HistoryDir = $CoverageHistory
    TargetFramework = "net7.0" 
}

ToConsole -String $Banner

#House keeping
DeleteFolder -Path $CoverageResults
DeleteFolder -Path $CoverageReport
DeleteFolder -Path $TestResults


############
#  Script  #  
############
$Banner = @"
#########################################################################################
#                         Run tests and calculate code coverage                         #
#########################################################################################
"@
ToConsole -String $Banner

cd "$($Solution)"

# List all tests
dotnet test --no-build --list-tests

# Run the tests and gather code coverage
dotnet test --logger "trx" --collect:"XPlat Code Coverage" --results-directory "$($TestResults)" --settings $PSScriptRoot/Coverlet.runsettings.xml --nologo


$Banner = @"
#########################################################################################
#                                 Generate HTML reports                                 #
#########################################################################################
"@
ToConsole -String $Banner

# Get the latest Report Generator package version
$reportGeneratorOptions.Version = GetLatestNuGetVersion -packageName "ReportGenerator"
ToConsole -String "Lastest package version for Report Generator is: $($reportGeneratorOptions.Version)"

ToConsole -String "`n`nSetting the Cyclomatic Complexity threshold"
# Open the file
$path = $(Resolve-Path "$home/.nuget/packages/reportgenerator/$($reportGeneratorOptions.Version)/tools/$($reportGeneratorOptions.TargetFramework)/appsettings.json").Path
$json = (Get-Content ($path) | ConvertFrom-Json)
ToConsole -String "CC value read from file: $($json.riskHotspotsAnalysisThresholds.metricThresholdForCyclomaticComplexity)"

# Modify the value
$json.riskHotspotsAnalysisThresholds.metricThresholdForCyclomaticComplexity = $reportGeneratorOptions.CyclomaticComplexityThreshold
ConvertTo-Json $json -Depth 2 | Out-File $path
ToConsole -String "CC value written to file: $($reportGeneratorOptions.CyclomaticComplexityThreshold)"


# Generate code coverage html report
ToConsole -String "`n`nGenerating code coverage report"
& $home/.nuget/packages/reportgenerator/$($reportGeneratorOptions.Version)/tools/$($reportGeneratorOptions.TargetFramework)/ReportGenerator.exe "-reports:$($TestResults)*/coverage.opencover.xml" "-targetdir:$($CoverageReport)" -reporttypes:"$($reportGeneratorOptions.ReportTypes)" -historydir:"$($reportGeneratorOptions.HistoryDir)" -title:"$($reportGeneratorOptions.Title)"

# Generate test results html report
ToConsole -String "`n`nGenerating test results report"
$files = Get-ChildItem $($TestResults)*.trx
foreach ($file in $files) 
{
    & $Trxer "$($TestResults)$file"
}

# Open the coverage report in the browser, resolving the path first
ToConsole -String "`n`nOpening reports in the browser"
$path = $(Resolve-Path "$($CoverageReport)index.html").Path
Start-Process -FilePath($path)

# Open the test result reports in the browser, resolving the path first
foreach ($file in Get-ChildItem $($TestResults)*.html) 
{
    $path = Join-Path -Path $TestResults -ChildPath $file

    Start-Process -FilePath($path)
}

# Zip up the testing evidence
Start-Sleep -Seconds 2.0
ToConsole -String "`n`nZipping testing evidence to: $TestingEvidenceZip"
Compress-Archive -Path "$CodeQuality" -DestinationPath $TestingEvidenceZip -CompressionLevel Optimal -Force


$Banner = @"
#########################################################################################
#                            Coverage and testing complete                              #
#########################################################################################
"@
ToConsole -String "`n"
ToConsole -String $Banner

Read-Host -prompt "Press any key to exit" 