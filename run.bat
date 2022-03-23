@echo off
setlocal

if not exist %~dp0\Wordler\ mkdir %~dp0\Wordler

pushd %~dp0\Wordler

if not exist "Wordler.exe" (
	echo No binary executable for the SQL Compiler found. Downloading from the latest release on GitHub before execution...
	bitsadmin /transfer mydownloadjob /download /priority FOREGROUND "https://github.com/vesk4000/Wordler/releases/download/v1.2/Wordler.exe" %~dp0\Wordler\Wordler.exe
)

if not exist "wordlist.txt" (
	echo No binary executable for the SQL Compiler found. Downloading from the latest release on GitHub before execution...
	bitsadmin /transfer mydownloadjob /download /priority FOREGROUND "https://github.com/vesk4000/Wordler/releases/download/v1.2/wordlist.txt" %~dp0\Wordler\wordlist.txt
)

start "" /wait /B Wordler.exe --tl 20