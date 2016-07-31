using System;

namespace Core.Clang.Tests
{
    internal unsafe class Disposables : IDisposable
    {
        public Index Index { get; }

        public TranslationUnit Add { get; }

        public TranslationUnit Multiply { get; }

        public Disposables()
        {
            Index = new Index(true, true);
            Add = Index.ParseTranslationUnit(
                TestFiles.AddSource,
                null,
                options: TranslationUnitCreationOptions.DetailedPreprocessingRecord);
            Multiply = Index.ParseTranslationUnit(
                TestFiles.MultiplySource,
                null,
                options: TranslationUnitCreationOptions.DetailedPreprocessingRecord);
        }

        public void Dispose()
        {
            Add.Dispose();
            Multiply.Dispose();
            Index.Dispose();
        }
    }
}
