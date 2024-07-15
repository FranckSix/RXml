# RXml Resources for .Net
 
Generate constants classes to avoid use of magic strings with resources.
 
Can be used as follows:
```csharp
XResIndex.Title
```
To overcome the fact that WebAssembly does not have access to the solution's Resx files.
 
 # Usage
- Install the package [^1]
	```
	Install-Package RXml.Generator
	```
- Define .rxml files
 
	*Example: Index.xres*
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
- In the csproj define rxml like this [^2]
	```xml
	<ItemGroup>
		<AdditionalFiles  Include="<ResourcesPath>\*.rxml" />
	</ItemGroup>
	```
	**AdditionalFiles** - For the Code generator to produce constants classes
 		    
[^1]: To use with RXml.Abstraction the package allows to avoid using magic string for resource usage.
[^2]: The definition here is only for the RXml.Generator, to use with RXml.Abstraction see documentation of the package