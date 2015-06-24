# Sitecore Pathfinder

Get started, get far, get happy!

![Sitecore Pathfinder](.docs/img/SitecorePathfinder.PNG)
 
Watch the videos on YouTube:
* [01 - Idea and concepts](https://www.youtube.com/watch?v=TcJ0IoI7sVM)
* [02 - HelloWorld](https://www.youtube.com/watch?v=jQz5hAVOTzU)
* [03 - Unit Testing](https://www.youtube.com/watch?v=DWU6D7L8ykg)
* [04 - Html Templates](https://www.youtube.com/watch?v=9aTGhW6ErYM)

# Introduction
Sitecore Pathfinder is a toolchain for Sitecore, that allows developers to use their favorite tools 
in a familiar fashion to develop Sitecore websites.

The toolchain creates a deliverable package from the source files in a project directory and deploys 
The toolchain creates a deliverable package from the source files in a project directory and deploys 
the package to a website where an installer installs the new files and Sitecore items into the website.

The developer process is familiar; edit source files, build and install the package, review the changes on website, repeat.

## Getting started

Pathfinder makes it easy to start working with Sitecore.

1. Install a clean Sitecore (e.g. using [SIM (Sitecore Instance Manager](https://marketplace.sitecore.net/modules/sitecore_instance_manager.aspx))
2. Create an empty folder and xcopy the Pathfinder files to the .sitecore.tools subfolder
3. Execute the scc.exe in the .sitecore.tools folder
4. Edit the scconfig.json file to setup 'project-unique-id', 'wwwroot' and 'host-name'
5. Done - you are now ready

In step 3 Pathfinder creates a blank project for you. It consists of a number of directories and files, 
including an scc.cmd file which is a shortcut to the .sitecore.tools\scc.exe file.

## How does Pathfinder make Sitecore easier
* Familiar developer experience: Edit source files, build project, test website, repeat.
* Text editor agnostic (Visual Studio not required - use Notepad, Notepad++, SublimeText, VS Code etc.)
* Build process agnostic (command-line tool, so it integrates easily with Grunt, Gulp, MSBuild etc.)
* Everything is a file (easy to edit, source control friendly)
* Project directory has whole and single truth (source is not spead across development projects, databases and websites, contineous integration friendly) 
* Project is packaged into a NuGet package and deployed to the website
  * Dependency tracking through NuGet dependencies
  * NuGet package installer on Sitecore website
  * Sitecore.Pathfinder.Core NuGet package tweaks Sitecore defaults to be easier to work with (e.g. removes initial workflow)
* Web Test Runner for running unit tests inside Sitecore website (supports dynamic compilation of C# source files)
* Support for Html Templates (with [Mustache](https://mustache.github.io/mustache.5.html) tags) makes getting started with the Sitecore Rendering Engine easier
* Validate a Sitecore website against 70 rules using Sitecore Rocks SitecoreCop

# Features

## Unit testing
[Watch the video](https://www.youtube.com/watch?v=DWU6D7L8ykg)

Unit testing in Sitecore can be tricky for a number of reasons. One reason is that sometimes you want your 
unit test to be executed within the Sitecore web context. Unless you have advanced mocking capabilities, this
requires you to make a request to a Sitecore website and run the tests.

Pathfinder installs a Web Test Runner in your Sitecore website. When you run the `run-unittests` task, Pathfinder
copies the unit test C# files to the server, compiles them and runs the tests.

This makes it easy to write server-side unit tests in you project and execute the in a Sitecore web context.

## Website validation
Pathfinder integrates with the Sitecore Rocks SitecoreCop feature. SitecoreCop examines the website and can identify
over 70 different types of issues. To validate the website, run the task `validate-website`.

![SitecoreCop validations 1](.docs/img/SitecoreCop2.png)
![SitecoreCop validations 2](.docs/img/SitecoreCop3.png)

## Layout Engines

### Sitecore rendering engine
Pathfinder supports the Sitecore Rendering Engine by supporting a special format for the __Rendering field. 
The format is similar to Html and Xaml, and is parsed when the package is installed into Xml format, that 
Sitecore expects. 

Here is an example of the format in Json.
```js
{
    "Layout": {
        "Devices": [
            {
                "Name": "Default",
                "Layout": "/sitecore/layout/layouts/MvcLayout",
                "Renderings": [
                    {
                      "HelloWorld": {
                      } 
                    }
                ]
            }
        ]
    }
}
```

### Html Templates
[Watch the video](https://www.youtube.com/watch?v=9aTGhW6ErYM)

Pathfinder also supports Html Templating which is simpler way of working with layouts. It resembles working with Mustache
Html Templates in JavaScript. However the Html Templates are resolved on the server and adapted to the Sitecore 
rendering engine.

When using Html Template, you do not have to specify a layout definition or use MVC views. Html Templates are not as 
powerful as the full Sitecore Rendering Engine, placeholders or using MVC views.

On an item, you specify the file name of the Html Template, you want to use, in the HtmlTemplate property.
```js
{
  "Item": {
    "HtmlTemplate": "/layout/renderings/HtmlTemplate.html",
  }
}
```

The Html Template is a Html file that also supports Mustache like tags.

```html
<h1>Fields</h1>
<p>
    {{Title}}
</p>
<p>
    {{Text}}
</p>
{{> Footer.html}}
```

The `{{Title}}` tags will be replace with the Title field in the current context item.

Please notice that Mustache lists and partials are supported (see the Footer tag in the last line). Pathfinder extends the 
Mustache syntax to support Sitecore placeholders - the tag `{{% placeholder-name}}` will be rendered as a Sitecore
placeholder.

Pathfinder creates .html as View renderings and these renderings can used as any other Sitecore rendering.

# Environment

## Notepad
Everything in Pathfinder is a file, so you can use Notepad to edit any file.

## Visual Studio Code

[Visual Studio Code](https://code.visualstudio.com/) is a nice code editor and Pathfinder contains default
configuration files for Code in the .settings directory. The default build task in Code has been configured
to execute the build pipeline in Pathfinder. In Code the build task can be executed by pressing Ctrl+Shift+B.

## Visual Studio

1. Create a web project in Visual Studio
1. Install the Sitecore Pathfinder Nuget package
1. Run .sitecore.tools\scc.exe to create an empty Pathfinder project, files and folders
1. Install GruntJS in the project: npm install grunt --save-dev
1. Install grunt-shell: npm install --save-dev grunt-shell
1. In Solution Explorer, open the project context menu and select Add | Grunt and Bower to Project
1. Right-click gruntfile.js and select Task Runner Explorer
1. Add the following lines to gruntfile.js

```js
module.exports = function (grunt) {
    grunt.initConfig({
        shell: {
            "build-project": {
                command: "scc.cmd"
            },
            "run-unittests": {
                command: "scc.cmd run-unittests"
            },
            "generate-unittests": {
                command: "scc.cmd generate-unittests"
            },
            "generate-code": {
                command: "scc.cmd generate-code"
            },
            "sync-website": {
                command: "scc.cmd sync-website"
            },
            "validate-website": {
                command: "scc.cmd validate-website"
            }
        }
    });

    grunt.registerTask("build-project", ["shell:build-project"]);
    grunt.registerTask("run-unittests", ["shell:run-unittests"]);
    grunt.registerTask("generate-unittests", ["shell:generate-unittests"]);
    grunt.registerTask("generate-code", ["shell:generate-code"]);
    grunt.registerTask("sync-website", ["shell:sync-website"]);
    grunt.registerTask("validate-website", ["shell:validate-website"]);

    grunt.loadNpmTasks("grunt-shell");
};
```

10. In Task Runner Explorer, right-click 'build-project' and select Bindings | After Build. This will run Pathfinder after each build.
10. If you want to use code generate, right-click 'generate-code' and select Binding | Before Build.


## Sitecore toolbox
As a Sitecore developer, what should be in your development toolbox? 

Application   | Description | Difficulty
------------- | ------------| ----------
[SIM (Sitecore Instance Manager](https://marketplace.sitecore.net/modules/sitecore_instance_manager.aspx) | Sitecore website installer and more | Low to medium
[Sitecore Powershell Extensions](https://marketplace.sitecore.net/en/Modules/Sitecore_PowerShell_console.aspx) | Run Powershell scripts in a Sitecore website | High
[Sitecore Rocks Visual Studio](https://visualstudiogallery.msdn.microsoft.com/44a26c88-83a7-46f6-903c-5c59bcd3d35b/) | Visual Studio plugin for working with Sitecore | Low to high
[Sitecore Rocks Windows](https://github.com/JakobChristensen/Sitecore.Rocks.Docs) | Sitecore Rocks version that does not require Visual Studio | Low to high
[Sitecore Pathfinder](https://github.com/JakobChristensen/Sitecore.Pathfinder) | Sitecore build toolchain | Low

