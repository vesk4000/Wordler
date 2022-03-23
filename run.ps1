$name = Split-Path -Path $PSCommandPath -Leaf  # File Name only
$div = $name.Split('.')[0]

$directorypath =  (Get-Item .).FullName

$path = $directorypath + '\Wordler\'
If (!(test-path $path)) { mkdir $path }

$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.2/wordlist.txt'
$path_to_file = $directorypath + '\Wordler\wordlist.txt'
Invoke-WebRequest $url -OutFile $path_to_file

$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.2/launcher_args.txt'
$path_to_file = $directorypath + '\Wordler\launcher_args.txt'
Invoke-WebRequest $url -OutFile $path_to_file
Add-Content $path_to_file (' -d ' + $div + '/14')

$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.2/Wordler.exe'
$path_to_file = $directorypath + '\Wordler\Wordler.exe'
Invoke-WebRequest $url -OutFile $path_to_file

$url = 'https://github.com/vesk4000/Wordler/releases/download/v1.2.2/WordlerLauncher.exe'
$path_to_file = $directorypath + '\Wordler\WordlerLauncher.exe'
Invoke-WebRequest $url -OutFile $path_to_file

Start-Process -FilePath 'Wordler\WordlerLauncher.exe'

stop-process -Id $PID