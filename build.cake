#addin "nuget:?package=Cake.Docker&version=0.9.9"

var target = Argument("target", "Default");

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
			OutputDirectory = "C:\\sugcon\\host"
		});
	});

Task("DockerBuild")
	.Does(() => {
		DockerBuild(
			new DockerImageBuildSettings {
				Rm = true,
				//Isolation = "process",
				Tag = new string[] { "schost:messaging" }
			},
			"."
		);
	});

Task("DockerRun")
	.Does(() => {
		DockerRun(
			new DockerContainerRunSettings {
				//Isolation = "process"
			},
			"schost:messaging",
			""
		);
	});

Task("DockerRm")
	.Does(() => {

	});

Task("Default")
	.IsDependentOn("DockerBuild")
	.IsDependentOn("DockerRun");

RunTarget(target);