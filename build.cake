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
	.IsDependentOn("DockerBuild")
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

Task("Default");

RunTarget(target);