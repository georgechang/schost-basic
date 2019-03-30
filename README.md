# Basic Sitecore Host Application

This is a basic Sitecore Host application that displays a demo web page. This was originally used as a demo for [SUGCON EU](https://www.sugcon.eu/) 2019 and at the [Atlanta Sitecore User Group](https://www.meetup.com/Atlanta-Sitecore/) in March 2019.

## Components 🧩

### `ScHost`

This is the base Sitecore Host application. It initializes Sitecore Host...and that's about it.

### `GC.Plugin.Messaging.Web`

This is the plugin that provides functionality to the Sitecore Host application. It initializes a web host and provides a demo page to be displayed.

## Usage 🙋‍♂️

There is a Cake script that will execute the build and deployment of the application.

Simply run `build.ps1` and the script will build and deploy the application into the `bin` directory of `ScHost`. This will include the dependent `GC.Plugin.Messaging.Web` plugin and deploy the files to the appropriate directories in the application.

If you'd like to publish your application to a different path, you can execute the Cake script like this:

`.\build.ps1 -ScriptArgs '-output="C:\output"'`

**You will need a Sitecore subscription license placed in the `sitecoreruntime` folder of the published application in order for it to run.**

To use the plugin independently, you can run `dotnet pack` on the `GC.Plugin.Messaging.Web` project which will properly generate a NuGet package for use with a Sitecore Host application. You'll need to then extract and install the NuGet package based on Sitecore's documentation [here](https://doc.sitecore.com/developers/91/sitecore-experience-management/en/add-a-runtime-plugin-manually.html#UUID-eb8606b9-8730-496a-b367-671c8b9dbab7_section-idm45363803631184).

## Docker 🚚

There is a `Dockerfile` included in the project which will build and create a Docker image of the Sitecore Host application. The `Dockerfile` expects the Sitecore `license.xml` to be in the root (same folder as the `Dockerfile`) in order to build the image.

The Cake script also has steps to build the Docker image. You can run the following to build a Docker image:

`.\build.ps1 -Target "Docker Build"`

Or, to build and run the image, you can do:

`.\build.ps1 -Target "Docker Run"`

By default, it tags the images as `schost:latest`. If you want to change the tag of the Docker image that is built with the Cake script, you can run either the `Docker Build` or `Docker Run` targets with the this extra parameter:

`.\build.ps1 -Target "Docker Build" -ScriptArgs '-tag="newtag:latest"'`

## Disclaimer ⚠

This is only intended as a proof-of-concept and does not contain production-ready code. Please don't treat it as such. 😎