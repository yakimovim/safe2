var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./output");
});

Task("BuildAll")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild("./Safe.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration
    });
});

Task("BuildSafe")
    .IsDependentOn("Test")
    .Does(() =>
{
    DotNetCoreBuild("./src/Safe/Safe.csproj", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = "./output/"
    });
});

Task("Test")
    .IsDependentOn("BuildAll")
    .Does(() =>
{
    DotNetCoreTest("./Safe.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});


Task("Pack")
    .IsDependentOn("BuildSafe")
    .Does(() =>
{
    CleanDirectory("./package");
    Zip("./output", "./package/Safe.zip");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);