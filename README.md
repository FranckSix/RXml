# RXml Resources for .Net
 
Allows you to consume resources in a .net or blazor webassembly project.
 
Can be used as follows:
```html, csharp
    @inject ILocalisationService<Index> Localizer
    <span>@Localizer["Tite"]</span>
```
To overcome the fact that WebAssembly does not have access to the solution's Resx files.
 
 # Usage
- Install the package [^1]
	```
	Install-Package RXml.Abstraction
	Install-Package RXml.Generator
	```
- Define .rxml files
 
	*Example:*
	```xml
	<?xml  version="1.0"  encoding="utf-8" ?>
	<Resources>
		<Res  Key="Title">
			<Value  lang="fr">Indexe</Value>
			<Value  lang="en">Index</Value>
		</Res>
		<Res  Key="HelloWorld">
			<Value  lang="fr">Bonjour monde !</Value>
			<Value  lang="en">Hello, world!</Value> 
		</Res>
		<Res  Key="Bienvenue">
			<Value  lang="fr">Bienvenue dans votre application.</Value>
			<Value  lang="en">Welcome to your new app.</Value> 
		</Res>
		<Res  Key="Survey">
			<Value  lang="fr">Comment Blazor fonctionne pour vous ?</Value>
			<Value  lang="en">How is Blazor working for you?</Value>
		</Res> 
	</Resources>
	```
- In the csproj define rxml like this
	```xml
	<ItemGroup>
		<AdditionalFiles  Include="<ResourcesPath>\*.rxml" />
		<EmbeddedResource  Include="<ResourcesPath>\*.rxml" />
	</ItemGroup>
	```
	**AdditionalFiles** - For the Code générator to générate constants classes
	**EmbeddedResource** - For ILocalisationService can consume resource file
 
## Add Interface service in App
The package also include a tools to register Interfaces.
in **Program.cs** 
 
```csharp
using RXml.Abstraction.Service;
builder.Services.AddLocalizationService();
```
 
There are two ways to use it.
 
*I'm a bit allergic to magic strings *😉*. That's kind of why I created the RXml.Generator package. This package is optional. If you choose to use it, here's what your code might look like..*
```html
@inject ILocalisationService<Index> Localizer
<span>@Localizer[XResIndex.Title]</span>
```
		    
[^1]: The package *RXml.Generator* allows to avoid using magic string for resource usage. Both ways to do it with or without this package are detailed in this document
