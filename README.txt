Project to try the IOM Example - Concrete Column with Idea Statica
https://github.com/idea-statica/ideastatica-public/blob/main/docs/rcs/rcs-column.md

How to make the script executable? - By Rayaan Ajouz

1a) Open Command Promt as administrator
1b) Change directory to repos directory, by entering: cd C:\Users\r.ajouz\source\repos\test_IDEA_RCS\idea_RCS_test\bin\Debug
1c) Create symbolic link (virtual link), by entering: mklink /d idea "c:\Program Files\IDEA StatiCa\StatiCa 21.0"
1d) Response should look like this: symbolic link created for idea <<===>> c:\Program Files\IDEA StatiCa\StatiCa 21.0

2a) Go to bin folder: C:\Users\r.ajouz\source\repos\test_IDEA_RCS\idea_RCS_test\bin\Debug
2b) Open test_IDEA_RCS.exe.config with text editor
2c) add text:
	<assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      		<probing privatePath="idea" />
	</assemblyBinding>
2d) Save file

3a) Open idea_RCS_test.sln
3b) Go to references in the solution explorer, right click "IdeaStatiCa.RcssController.dll" and select properties
3c) Set Copy Local to False 

Now you are able to run the script.

