#addin "nuget:?package=Cake.Figlet&version=1.2.0"
#addin "nuget:?package=Cake.Docker&version=0.9.9"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var tag = Argument("tag", "schost:latest");
var output = Argument("output", "");

Setup(ctx => {
    Information("");
    Information(Figlet("Sitecore Host"));
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
		var settings = new DotNetCorePublishSettings {
			Configuration = configuration
		};

		if (!string.IsNullOrEmpty(output))
		{
			settings.OutputDirectory = output;
		}

		DotNetCorePublish("./src/ScHost/ScHost.csproj", settings);
	});

Task("Docker Build")
	.Does(() => {
		DockerBuild(
			new DockerImageBuildSettings {
				Rm = true,
				Isolation = "process",
				Tag = new string[] { tag }
			},
			"."
		);
	});

Task("Docker Run")
	.IsDependentOn("Docker Build")
	.Does(() => {
		DockerRun(
			new DockerContainerRunSettings {
				Isolation = "process"
			},
			tag,
			""
		);
	});

Task("Default")
	.IsDependentOn("Publish");

RunTarget(target);