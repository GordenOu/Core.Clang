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
            CXTranslationUnitImpl* ptr;
            using (var fileName = new CString(TestFiles.AddSource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    Index.Ptr,
                    fileName.Ptr,
                    null, 0,
                    null, 0,
                    (uint)CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord,
                    &ptr).Check();
            }
            Add = new TranslationUnit(ptr, Index);
            using (var fileName = new CString(TestFiles.MultiplySource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    Index.Ptr,
                    fileName.Ptr,
                    null, 0,
                    null, 0,
                    (uint)CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord,
                    &ptr).Check();
            }
            Multiply = new TranslationUnit(ptr, Index);
        }

        public void Dispose()
        {
            Add.Dispose();
            Multiply.Dispose();
            Index.Dispose();
        }
    }
}
