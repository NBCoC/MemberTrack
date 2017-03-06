try
{
	Write-Output 'Cleaning wwwroot...'
	
	Remove-Item '.\MemberTrack.WebApi\wwwroot\*.*' 
	Remove-Item '.\MemberTrack.WebApi\wwwroot\scripts\*.*'
	Remove-Item '.\MemberTrack.WebApi\wwwroot\scripts\material-icons\*.*'
	
	Write-Output 'Copying client app to wwwroot...'
	
	Copy-Item '.\MemberTrack.Client\index.html' '.\MemberTrack.WebApi\wwwroot' -force
	Copy-Item '.\MemberTrack.Client\favicon.ico' '.\MemberTrack.WebApi\wwwroot' -force
	Copy-Item '.\MemberTrack.Client\nbcc.png' '.\MemberTrack.WebApi\wwwroot' -force
	Copy-Item '.\MemberTrack.Client\scripts\*' '.\MemberTrack.WebApi\wwwroot\scripts' -force
	Copy-Item '.\MemberTrack.Client\scripts\material-icons\*' '.\MemberTrack.WebApi\wwwroot\scripts\material-icons' -force
	
	$exitCode = 0
}
catch 
{
    Write-Error "*** $_"

    Write-Output 'Process aborted.'

    $exitCode = 1
}
finally
{
    Exit $exitCode
}