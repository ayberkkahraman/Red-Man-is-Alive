@echo off
echo "Autoclean v1"
pause
echo "STARTING CLEAN"
echo "DOWNLOADING GITIGNORE"
powershell -command "iwr -outf .gitignore https://raw.githubusercontent.com/github/gitignore/master/Unity.gitignore"
echo "GIT INIT"
git init
git add *
git commit -m "Initial commit"
echo "GIT COMMIT"
git clean -xfd
@RD /S /Q ".git"