#addin "nuget:?package=Cake.PowerShell&version=0.4.7"
#addin "nuget:?package=Cake.Figlet&version=1.2.0"
#addin "nuget:?package=Cake.Docker&version=0.9.9"

var hostPath = @"C:\sugcon\host";

var target = Argument("target", "Default");

Setup(ctx => {
    Information("");
    Information(Figlet("sugcon.eu"));
	Information("Sitecore Host");
});

Task("Clean")
	.Does(() =>
	{
		DotNetCoreClean("./src/ScHost");
	}
);

Task("Publish")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		DotNetCorePublish("./src/ScHost/ScHost.csproj", new DotNetCorePublishSettings {
			OutputDirectory = hostPath
		});
	});

Task("Docker Build")
	.Does(() => {
		DockerBuild(
			new DockerImageBuildSettings {
				Rm = true,
				Isolation = "process",
				Tag = new string[] { "schost:latest" }
			},
			"."
		);
	});

Task("Docker Run")
	.Does(() => {
		DockerRun(
			new DockerContainerRunSettings {
				Isolation = "process"
			},
			"schost:latest",
			""
		);
	});

Task("Default")
	.IsDependentOn("Publish");

RunTarget(target);