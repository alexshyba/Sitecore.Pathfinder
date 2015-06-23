# Sitecore Pathfinder

Get started, get far, get happy!

Watch the videos:
* [01 - Idea and concepts](https://www.youtube.com/watch?v=TcJ0IoI7sVM)
* [02 - HelloWorld](https://www.youtube.com/watch?v=jQz5hAVOTzU)
* [03 - Unit Testing](https://www.youtube.com/watch?v=DWU6D7L8ykg)

Introduction
============
Sitecore Pathfinder is a toolchain for Sitecore, that allows developers to use their favorite tools 
in a familiar fashion to develop Sitecore websites.

The toolchain creates a deliverable package from the source files in a project directory and deploys 
the package to a website where an installer installs the new files and Sitecore items into the website.

The developer process is familiar; edit sourcefiles, build and install the package, review the changes on website.

## Getting started

Pathfinder makes it easy to start working with Sitecore.

1. Install a clean Sitecore (e.g. using Sitecore Instance Manager)
2. Create an empty folder and xcopy the Pathfinder files to the .sitecore.tools subfolder
3. Execute the scc.exe in the .sitecore.tools folder
4. Edit the scconfig.json file to setup 'project-unique-id', 'wwwroot' and 'host-name'
5. Done - you are now ready

In step 3 Pathfinder creates a blank project for you. It consists of a number of directories and files, 
including an scc.cmd file which is a shortcut to the .sitecore.tools\scc.exe file.

# Features

## Unit testing
Unit testing in Sitecore can be tricky for a number of reasons. One reason is that sometimes you want your 
unit test to be executed within the Sitecore web context. Unless you have advanced mocking capabilities, this
requires you to make a request to a Sitecore website and run the tests.

Pathfinder installs a Web Test Runner in your Sitecore website. When you run the `run-unitests` task, Pathfinder
copies the unit test C# files to the server, compiles them and runs the tests.

This makes it easy to write server-side unit tests in you project and execute the in a Sitecore web context.

## Layout Engines
### Sitecore rendering engine
Pathfinder supports the Sitecore Rendering Engine by supporting a special format for the __Rendering field. 
The format is similar to Html and Xaml, and is parsed when the package is installed into Xml format, that 
Sitecore expects. 

Here is an example of the format in Json.
```
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

### Html templating
Pathfinder also supports Html Templating which is simpler way of working with layouts. It resembles working with Mustache
Html Templates in JavaScript. However the Html Templates are resolved on the server and adapted to the Sitecore 
rendering engine.

When using Html Template, you do not have to specify a layout definition or use MVC views. Html Templates are not as 
powerful as the full Sitecore Rendering Engine, placeholders or using MVC views.

On an item, you specify the file name of the Html Template, you want to use, in the HtmlTemplate property.
```
{
  "Item": {
    "HtmlTemplate": "/layout/renderings/HtmlTemplate.html",
    ]
  }
}
```

The Html Template is a Html file that also supports Mustache like tags.

```
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

Please notice that Mustache lists and partials are supported (as shown in the last line). Pathfinder extends the 
Mustache syntax to support Sitecore placeholders - the tag `{{% placeholder-name}}` will be rendered as a Sitecore
placeholder.

Pathfinder creates .html as View renderings and these renderings can used as any other Sitecore rendering.

# Environments

## Notepad
Everything in Pathfinder is a file, so you can use Notepad to edit any file.

## Visual Studio Code

[Visual Studio Code](https://code.visualstudio.com/) is a nice code editor and Pathfinder contains default
configuration files for Code in the .settings directory. The default build task in Code has been configured
to execute the build pipeline in Pathfinder. In Code the build task can be executed by pressing Ctrl+Shift+B.

