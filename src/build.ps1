
properties {
    $testMessage = 'Executed Test!'
    $compileMessage = 'Executed Compile!'
	$cleanMessage = 'Executed Clean!'
    $debugMessage = "Debug Build Finished"
    $releaseMessage = "Release Build Finished"
    $projectName = 'MyBatis.Net'
    $projectVersion = "1.1"
    $nunitReportInstalled = $false
    $projectConfig = "Debug"
    $buildDefines = $null
    $currentBuildDefines = $null
    $buildDir = "bin\" + $projectConfig
    $outDir = ".\build"     
    
    ####################
    #External Programs
    ####################

    #Need to replace this project seems to be dead.
    $ndocHome = "C:\Program Files\NDoc 1.3\bin\net\1.1"    
    $nunitConsole = ".\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe"
    $reportUnit = "packages\ReportUnit.1.2.1\tools\ReportUnit.exe"
}

task default -depends Build 

task Build -depends Compile, Clean {
     if(!(Test-Path $outDir"\dist"))
    {
        md $outDir"\dist"
    }
     Copy-Item ".\MyBatis.Common\$buildDir\*.dll" $outDir"\dist"
    Copy-Item ".\MyBatis.DataMapper\$buildDir\*.dll" $outDir"\dist"
}

task Test -depends Compile, Clean { 
    Write-Host building test projects.
    msbuild /p:Configuration=$projectConfig .\MyBatis.Common.Test\MyBatis.Common.Test.csproj
    msbuild /p:Configuration=$projectConfig .\MyBatis.DataMapper.SqlClient.Test\MyBatis.DataMapper.SqlClient.Test.csproj
    msbuild /p:Configuration=$projectConfig .\MyBatis.DataMapper.Sqllite.Test\MyBatis.DataMapper.Sqllite.Test.csproj
    Write-Host test projects built running tests
    & $nunitConsole  ".\MyBatis.Common.Test\$buildDir\MyBatis.Common.Test.dll" "-result=MyBatis.Common.Test.xml" "-work=.\$outDir\test"
    & $nunitConsole  ".\MyBatis.DataMapper.SqlClient.Test\$buildDir\MyBatis.DataMapper.SqlClient.Test.dll" "-result=MyBatis.DataMapper.SqlClient.Test.xml" "-work=.\$outDir\test"
    & $nunitConsole  ".\MyBatis.DataMapper.Sqllite.Test\$buildDir\MyBatis.DataMapper.Sqllite.Test.dll" "-result=MyBatis.DataMapper.Sqllite.Test.xml" "-work=.\$outDir\test"
    & $reportUnit ".\$outDir\test"
    $testMessage
}

task Compile -depends Clean {    
    Write-Host $debug  
    msbuild /p:Configuration=$projectConfig .\MyBatis.Common\MyBatis.Common.csproj
    msbuild /p:Configuration=$projectConfig .\MyBatis.DataMapper\MyBatis.DataMapper.csproj
    $compileMessage
}

task Clean {
      rd -Path .\*\bin -r
      #rd -Path .\build -r -Force
}

task Debug -depends Test {

}

task ? -Description "Helper to display task info" {
	Write-Documentation
}