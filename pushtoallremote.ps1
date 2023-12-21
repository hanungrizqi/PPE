$remotes = git remote
foreach ($remote in $remotes) {
Write-Host "Pushing to remote: $remote"
git push $remote
}