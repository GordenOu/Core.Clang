# Core.Clang

|Windows|Ubuntu|
|:--:|:--:|
|[![Build Status](https://ci.appveyor.com/api/projects/status/github/GordenOu/Core.Clang?svg=true)](https://ci.appveyor.com/project/GordenOu/core-clang)|[![Build Status](http://www.gordenou.net:8111/app/rest/builds/buildType:(id:CoreClang_Build)/statusIcon.svg)](http://www.gordenou.net:8111/viewType.html?buildTypeId=CoreClang_Build&guest=1)|

This project selectively imports some routines from LLVM/libclang via Platform Invoke to facilitate cross platform C/C++ code analysis in .NET Core.
It provides thorough XML documentation,
and tries to follow the framework design guidelines without deviating too far away from the original interfaces.

- [NuGet Package](https://www.nuget.org/packages/Core.Clang/)
- [License](License.txt)

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
