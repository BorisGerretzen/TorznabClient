using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;

[GitHubActions(
    "test",
    GitHubActionsImage.UbuntuLatest,
    // On = [GitHubActionsTrigger.PullRequest, GitHubActionsTrigger.WorkflowDispatch, GitHubActionsTrigger.Push], 
    InvokedTargets = [nameof(Test)],
    FetchDepth = 10000,
    OnPushBranches = ["main"],
    OnPullRequestBranches = ["main"]
)]
[GitHubActions(
    "publish",
    GitHubActionsImage.UbuntuLatest,
    On = [GitHubActionsTrigger.WorkflowDispatch],
    // InvokedTargets = [nameof(Pack), nameof(Push)], 
    InvokedTargets = [nameof(Pack)],
    ImportSecrets = [nameof(NugetApiKey)],
    FetchDepth = 10000
)]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion] readonly GitVersion GitVersion;
    [Parameter("API Key for the NuGet server.")] [Secret] readonly string NugetApiKey;
    [Parameter("NuGet server URL.")] readonly string NugetSource = "https://api.nuget.org/v3/index.json";
    [Parameter("NuGet package version.")] readonly string PackageVersion;
    [Solution] readonly Solution Solution;
    Project PublishProject => Solution.GetProject("TorznabClient");

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            ArtifactsDirectory.CreateOrCleanDirectory();
            DotNetTasks.DotNetClean(s => s
                .SetProject(Solution)
            );
        });

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => s
                .SetProjectFile(Solution)
            );
        });

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Log.Information("GitVersion = {Value}", GitVersion.MajorMinorPatch);
            DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .EnableNoRestore()
            );
        });

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild()
            );
        });

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Pack => _ => _
        .DependsOn(Clean, Test)
        .Before(Push)
        .Requires(() => Configuration == Configuration.Release)
        .Executes(() =>
        {
            DotNetTasks.DotNetPack(s => s
                .EnableNoRestore()
                .EnableNoBuild()
                .SetProject(PublishProject)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetProperty("PackageVersion", PackageVersion ?? GitVersion.NuGetVersionV2)
            );
        });

    // ReSharper disable once AllUnderscoreLocalParameterName
    Target Push => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetNuGetPush(s => s
                .SetSource(NugetSource)
                .SetApiKey(NugetApiKey)
                .CombineWith(ArtifactsDirectory.GlobFiles("*.nupkg"), (s, v) => s
                    .SetTargetPath(v)
                )
            );
        });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);
}