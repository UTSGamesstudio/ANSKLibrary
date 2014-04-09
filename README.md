ANSKLibrary
===========

The repository for the Graphics Project by Joshua Klaser. The library revolves around the importing, processing, controlling and drawing of 3D animated models in XNA and MonoGame.

In order to use the library, you do as followed (This will update if anything changes):

XNA Visual Studio 2010:

-- Referencing the libraries --

1. Download/Pull the files found on this repository.
2. Within the folder 'ANSKLibrary/AnimationExample', copy the folders 'ModelAnimationLibrary' and 'ModelAnimationPipeline', and paste them in a desired location.
3. If you wish to edit the source code or use the source project in your own project, follow step 4. Otherwise move to step 5.
4. In your Visual Studio 2010 project, on the right hand side right click on your project name and select 'add existing project', then navigate to the .csproj file found within each of the 2 folders you pasted (which should be called 'ModelAnimationLibrary.csproj' and 'ModelAnimationPipeline.csproj').
5. In your XNA project, on the right hand side right click of 'references' and click 'add reference'.
6. In the 'add reference' dialog click on the tab 'Browse' and locate the .dll of the 'ModelAnimationLibrary'. This can be found in the folder you pasted in the folder location 'ModelAnimationLibrary\bin\x86\Debug'. Then click ok until the 'add reference' dialog closes.
7. In your XNA Content project repeat the same steps as 5-6, except locate the .dll of 'ModelAnimationPipeline'.
8. Your project now has the library linked, set up and ready to use.

-- Using the libraries --

1. In order to use this library you need to have a 3D animated model that you need to use. You will find the page '3D Animated Model Setup' page in the wiki if you do not know how to provide this type of model.
2. Add the .fbx file into the content project as you would any other piece of content.
3. In the content file properties which appear on the right underneath the 'solution explorer' (if you can't see this, then click on your .fbx file in the 'solution explorer' and press alt + enter on your keyboard to bring it up), change the 'Content Importer' option to 'Model Animation Importer' and change the 'Content Processor' option to 'ModelAnimationProcessor' (If these options do not show, then check to see if you have referenced the 'ModelAnimationPipeline' and rebuild/clean the project to refresh it).
4. To use the contents of the 'ModelAnimationLibrary', you need to write the line "using ModelAnimationLibrary" alongside your using statements.
5. The rest of the explanation requires examples of code and images, so this explanation is continued in the wiki page 'Using The Model Animation Library'. 
