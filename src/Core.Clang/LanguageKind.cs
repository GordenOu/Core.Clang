namespace Core.Clang
{
    /// <summary>
    /// Describes the "language" of the entity referred to by a cursor.
    /// </summary>
    [EnumMapping(typeof(CXLanguageKind),
        Prefix = "CXLanguage_",
        Excluded = new object[] { CXLanguageKind.CXLanguage_ObjC })]
    public enum LanguageKind
    {
        /// <summary>
        /// No language information is available.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// C.
        /// </summary>
        C = 1,

        /// <summary>
        /// C++.
        /// </summary>
        CPlusPlus = 3
    }
}
