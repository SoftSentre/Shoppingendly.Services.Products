#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("dotnet-restore")
    .Does(() => 
    {
        DotNetCoreRestore("../SoftSentre.Shoppingendly.Services.Products.sln");
    });

Task("dotnet-build")
    .IsDependentOn("dotnet-restore")	
    .Does(() => 
    {
        DotNetCoreBuild("../SoftSentre.Shoppingendly.Services.Products.sln", new DotNetCoreBuildSettings 
        {
            Configuration = configuration,
            MSBuildSettings = new DotNetCoreMSBuildSettings
            {
                TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error
            }
        });
    });

Task("run-xunit-tests")	
    .IsDependentOn("dotnet-build")
    .Does(() => 
    {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration
        };
    
        DotNetCoreTest("../tests/Unit/SoftSentre.Shoppingendly.Services.Products.Tests.Unit.csproj", settings);
    });	

Task("Default")
    .IsDependentOn("run-xunit-tests");

RunTarget(target);