try
{
	Write-Output 'Cleaning wwwroot...'
	
	Remove-Item '.\MemberTrack.WebApi\wwwroot\*.*' 
	Remove-Item '.\MemberTrack.WebApi\wwwroot\scripts\*.*'
	Remove-Item '.\MemberTrack.WebApi\wwwroot\scripts\material-icons\*.*'
	
	Write-Output 'Copying index template to wwwroot...'
	
	Copy-Item '.\MemberTrack.Client\index-template.html' '.\MemberTrack.WebApi\wwwroot' -force
	Copy-Item '.\MemberTrack.Client\favicon.ico' '.\MemberTrack.WebApi\wwwroot' -force
	
	Rename-Item '.\MemberTrack.WebApi\wwwroot\index-template.html' index.html
	
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