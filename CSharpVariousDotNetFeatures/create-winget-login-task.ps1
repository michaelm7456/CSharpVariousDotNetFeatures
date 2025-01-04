# Installation
# Run the below command in terminal to install
# & '$BasePath\create-winget-login-task.ps1'
# $BasePath being the respective folder where 'create-winget-login-task.ps1' script is housed 

# Parameters
$taskName = "Winget Logon Update"  # Task name
$script = "winget-update.bat"
$scriptPath = Join-Path $PSScriptRoot $script  # Path to the batch file

# Define the action to run the script
$action = New-ScheduledTaskAction -Execute $scriptPath

# Define the trigger (at logon of any user)
$trigger = New-ScheduledTaskTrigger -AtLogOn

# Define task settings (run only if user is logged on)
$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable

# Register the scheduled task
Register-ScheduledTask -TaskName $taskName -Action $action -Trigger $trigger -Settings $settings -Description "Runs Winget to update all applications upon login"
