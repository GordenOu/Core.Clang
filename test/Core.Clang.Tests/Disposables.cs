using System;
using Core.Clang.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    internal unsafe class Disposables : IDisposable
    {
        public Index Index { get; }

        public TranslationUnit Add { get; }

        public TranslationUnit Multiply { get; }

        public Disposables()
        {
            Index = new Index(true, false);
            Add = Index.ParseTranslationUnit(
                TestFiles.AddSource,
                null,
                options: TranslationUnitCreationOptions.DetailedPreprocessingRecord);
            Multiply = Index.ParseTranslationUnit(
                TestFiles.MultiplySource,
                null,
                options: TranslationUnitCreationOptions.DetailedPreprocessingRecord);
        }

        public TranslationUnit WriteToEmpty(string source)
        {
            var translationUnit = Index.ParseTranslationUnit(
                   TestFiles.Empty,
                   null,
                   new[] { new UnsavedFile(TestFiles.Empty, source) },
                   TranslationUnitCreationOptions.DetailedPreprocessingRecord);
            var set = DiagnosticSet.FromTranslationUnit(translationUnit);
            if (set.GetNumDiagnostics() != 0)
            {
                Assert.Fail(set.GetDiagnostic(0).ToString());
            }
            return translationUnit;
        }

        public void Dispose()
        {
            Add.Dispose();
            Multiply.Dispose();
            Index.Dispose();
        }
    }
}
