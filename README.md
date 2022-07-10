#  Custom  Solution Targets

Light weight extension for visual studio **2022**. **before** and **after** targets are supported from msbuild but they will not work with visual studio. They are useful when you need to take some actions before build or publish of the whole solution, for example:

you can stop the iis or any interfering service before starting the build and start it when the visual studio is done.

## How to:

You can create you desired target in this format in the solution folder:

- **before.[solution-name].sln.targets**: before.MySolution.sln.targets
- **after.[solution-name].sln.targets**: after.MySolution.sln.targets


both  files need to be in a valid project format:

```xml
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <Target Name="MyBeforeSolutionTrigger" BeforeTargets="Build">
        <Message Importance="High" Text="xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" />
        <Message Importance="High" Text="Solution Build Started. *********" />
        <Message Importance="High" Text="xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" />
    </Target>
</Project>
```

```xml
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <Target Name="MyAfterSolutionTrigger" AfterTargets="Build">
        <Message Importance="High" Text="xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" />
        <Message Importance="High" Text="Solution Build Ended. *********" />
        <Message Importance="High" Text="xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" />
    </Target>
</Project>
```

Sample output:

```log
Build started...
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
Solution Build Started. *********
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
1>------ Build started: Project: WebApplication1, Configuration: Debug Any CPU ------
2>------ Build started: Project: WebApplication2, Configuration: Debug Any CPU ------
1>  WebApplication1 -> C:\Users\GA\source\repos\WebApplication2\deploy\bin\Debug\WebApplication1.dll
1>  Publishing project WebApplication1.
2>  WebApplication2 -> C:\Users\GA\source\repos\WebApplication2\deploy\bin\Debug\WebApplication2.dll
1>  Copying all files to temporary location below for package/publish:
1>  obj\Debug\Package\PackageTmp.
2>  Publishing project WebApplication2.
2>  Copying all files to temporary location below for package/publish:
2>  obj\Debug\Package\PackageTmp.
========== Build: 2 succeeded, 0 failed, 3 up-to-date, 0 skipped ==========
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
Build Ended. *********
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```
