# Set paths to important files and directories
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
$sqlProj1Path = "C:\Repositories\Repositories\DatabaseRepo\Project1\Project1.sqlproj"
$sqlProj2Path = "C:\Repositories\Repositories\DatabaseRepo\Project2\Project2.sqlproj"
$sqlProj3Path = "C:\Repositories\Repositories\DatabaseRepo\Project3\Project3.sqlproj"
$sqlPackagePath = "C:\Program Files\Microsoft SQL Server\160\DAC\bin\SqlPackage.exe"

# Set the build output path (where the DACPAC file will be located after the build)
$dacpacOutputDir = "C:\DatabaseRepo\SSDT\MSBuild\"
$dacpacProj1FileName = "Project1.dacpac"
$dacpacProj2FileName = "Project2.dacpac"
$dacpacProj3FileName = "Project3.dacpac"
$dacpacPowCentralPath = Join-Path $dacpacOutputDir $dacpacProj1FileName
$dacpacPoWCredentialsPath = Join-Path $dacpacOutputDir $dacpacProj2FileName
$dacpacPoWPath = Join-Path $dacpacOutputDir $dacpacProj3FileName

# # SQL Server instance and database configuration
$targetServer = "Michael-Laptop"  # Local SQL Server instance
$targetProj1Database = "Database1"  # Target database name
$targetProj2Database = "Database2"  # Target database name
$targetProj3Database = "Database3"  # Target database name

# # Define SqlPackage publish options
$publishOptions = "/p:CreateNewDatabase=False"  # Optionally allow database creation if it doesn't exist

# function to build the sql project
function Build-SqlProj1 {
    Write-Host "Building SQL project..."
    $buildCommand = "& `"$msbuildPath`" `"$sqlProj1Path`" /p:Configuration=Debug"
    Invoke-Expression $buildCommand
    
    if (Test-Path $dacpacPowCentralPath) {
        Write-Host "Build successful! DACPAC located at $dacpacPath"
    } else {
        Write-Error "Build failed or DACPAC not found!"
        exit 1
    }
}

function Build-SqlProj2 {
    Write-Host "Building SQL project..."
    $buildCommand = "& `"$msbuildPath`" `"$sqlProj2Path`" /p:Configuration=Debug"
    Invoke-Expression $buildCommand
    
    if (Test-Path $dacpacPoWCredentialsPath) {
        Write-Host "Build successful! DACPAC located at $dacpacPath"
    } else {
        Write-Error "Build failed or DACPAC not found!"
        exit 1
    }
}

function Build-SqlProj3 {
    Write-Host "Building SQL project..."
    $buildCommand = "& `"$msbuildPath`" `"$sqlProj3Path`" /p:Configuration=Debug"
    Invoke-Expression $buildCommand
    
    if (Test-Path $dacpacPoWPath) {
        Write-Host "Build successful! DACPAC located at $dacpacPath"
    } else {
        Write-Error "Build failed or DACPAC not found!"
        exit 1
    }
}

# Function to publish the DACPAC to the local SQL Server instance
function Publish-DacpacProj1 {
    Write-Host "Publishing DACPAC to SQL Server..."
    $publishCommand = "& `"$sqlPackagePath`" /Action:Publish /SourceFile:`"$dacpacPowCentralPath`" /TargetServerName:`"$targetServer`" /TargetDatabaseName:`"$targetProj1Database`" $publishOptions /ttsc:True "
    Invoke-Expression $publishCommand

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database published successfully!"
    } else {
        Write-Error "Database publishing failed!"
        exit 1
    }
}

function Publish-DacpacProj2 {
    Write-Host "Publishing DACPAC to SQL Server..."
    $publishCommand = "& `"$sqlPackagePath`" /Action:Publish /SourceFile:`"$dacpacPoWCredentialsPath`" /TargetServerName:`"$targetServer`" /TargetDatabaseName:`"$targetProj2Database`" $publishOptions /p:BlockOnPossibleDataLoss=False /ttsc:True "
    Invoke-Expression $publishCommand

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database published successfully!"
    } else {
        Write-Error "Database publishing failed!"
        exit 1
    }
}

function Publish-DacpacProj3 {
    write-host "publishing dacpac to sql server..."
    $publishcommand = "& `"$sqlpackagepath`" /action:publish /sourcefile:`"$dacpacPoWPath`" /targetservername:`"$targetserver`" /targetdatabasename:`"$targetProj3Database`" $publishoptions  /p:BlockOnPossibleDataLoss=False /ttsc:true "
    invoke-expression $publishcommand

    if ($lastexitcode -eq 0) {
        write-host "database published successfully!"
    } else {
        write-error "database publishing failed!"
        exit 1
    }
}

# Main Script Execution
try {
    Build-SqlProj1
	Build-SqlProj2
	Build-SqlProj3
    Publish-DacpacProj1
	Publish-DacpacProj2
	Publish-DacpacProj3
} catch {
    Write-Error "An error occurred: $_"
    exit 1
}