# Basic Sitecore Host Application

This is a basic Sitecore Host application that displays a demo web page. This was originally used as a demo for [SUGCON EU](https://www.sugcon.eu/) 2019 and at the [Atlanta Sitecore User Group](https://www.meetup.com/Atlanta-Sitecore/) in March 2019.

## Components

### `ScHost`

This is the base Sitecore Host application. It initializes Sitecore Host...and that's about it.

### `GC.Plugin.Messaging.Web`

This is the plugin that provides functionality to the Sitecore Host application. It initializes a web host and provides a demo page to be displayed.

## Usage

To build the Sitecore Host application, you can run `dotnet publish` on the `ScHost` project. This will compile and publish the application. This will build and publish both the `ScHost` base Sitecore Host application and the dependent `GC.Plugin.Messaging.Web` project.

**You will need a Sitecore subscription license placed in the `sitecoreruntime` folder of the published application in order for it to run.**

To use the plugin independently, you can run `dotnet pack` on the `GC.Plugin.Messaging.Web` project which will properly generate a NuGet package for use with a Sitecore Host application. You'll need to then extract and install the NuGet package based on Sitecore's documentation [here](https://doc.sitecore.com/developers/91/sitecore-experience-management/en/add-a-runtime-plugin-manually.html#UUID-eb8606b9-8730-496a-b367-671c8b9dbab7_section-idm45363803631184).

## Docker

There is a `Dockerfile` included in the project which will build and create a Docker image of the Sitecore Host application. The `Dockerfile` expects the Sitecore `license.xml` to be in the root (same folder as the `Dockerfile`) in order to build the image.