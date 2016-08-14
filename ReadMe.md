# Core.Clang
This project selectively imports some routines from LLVM/libclang via Platform Invoke to facilitate cross platform C/C++ code analysis in .NET Core.
It provides thorough XML documentation,
and tries to follow the framework design guidelines without deviating too far away from the original interfaces.

# Example
```C#
string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
Directory.CreateDirectory(tempPath);
string fileName = Path.Combine(tempPath, "main.cpp");
File.WriteAllText(fileName, "void HelloWorld();");
using (var index = new Index(true, true))
using (var translationUnit = index.ParseTranslationUnit(fileName, new[] { "-v" }))
{
    var location = translationUnit.GetFile(fileName).GetLocationFromOffset(0);
    Console.WriteLine(translationUnit.GetCursor(location).GetSpelling());
}
```

#Remarks
This project ignores:
- x86
- CLS compliance
- Doxygen
- Objective-C
