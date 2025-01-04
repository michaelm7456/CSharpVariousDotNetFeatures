@echo off
echo Checking for pending updates...

REM Run the PowerShell script to check and install updates
powershell -NoProfile -ExecutionPolicy Bypass -Command "& {Import-Module PSWindowsUpdate; $pendingUpdates = Get-WindowsUpdate; if ($pendingUpdates) { Write-Host 'There are pending updates:' -ForegroundColor Green; $pendingUpdates | Format-Table -Property KB, Title, Size, LastModified } else { Write-Host 'No pending updates found.' -ForegroundColor Red }; Write-Host 'Checking for new updates...' -ForegroundColor Yellow; $allUpdates = Get-WindowsUpdate -MicrosoftUpdate -AcceptAll; if ($allUpdates) { Write-Host 'Updates available and will now be installed:' -ForegroundColor Green; $allUpdates | Format-Table -Property KB, Title; Install-WindowsUpdate -AcceptAll -AutoReboot } else { Write-Host 'No new updates found.' -ForegroundColor Red } }"

echo Update check complete.
pause