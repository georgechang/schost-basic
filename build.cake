#addin "nuget:?package=Cake.Docker&version=0.9.9"

var target = Argument("target", "Default");

Task("DockerBuild")
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

Task("DockerRun")
	.Does(() => {
		DockerRun(
			new DockerContainerRunSettings {
				Isolation = "process"
			},
			"schost",
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