while ( ($(Get-Date).Hour-lt22)) {
    Start-Sleep -s 3600
}

while ( ($(Get-Date).Hour-lt23)) {
    Start-Sleep -s 600
}

Stop-Process -Name "chrome"

Stop-Computer