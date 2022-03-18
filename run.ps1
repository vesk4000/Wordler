$directorypath =  (Get-Item .).FullName

#$path = $directorypath + '\Wordler\'
#If (!(test-path $path)) { mkdir $path }

#$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.1/wordlist.txt'
#$path_to_file = $directorypath + '\Wordler\wordlist.txt'
#Invoke-WebRequest $url -OutFile $path_to_file

#$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.1/Wordler.exe'
#$path_to_file = $directorypath + '\Wordler\Wordler.exe'
#$directory_to_file = $directorypath + '\Wordler\'
#Invoke-WebRequest $url -OutFile $path_to_file

Start-Process -FilePath 'Wordler\Wordler Launcher.exe'# -WorkingDirectory $directory_to_file