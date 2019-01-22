using System;

namespace Core.Clang
{
    public sealed unsafe class Comment
    {
        internal CXComment Struct { get; }

        private Comment(CXComment cxComment)
        {
            Struct = cxComment;
        }

        internal static Comment Create(CXComment cxComment)
        {
            return new Comment(cxComment);
        }

        public CommentKind Kind
        {
            get
            {
                return (CommentKind)NativeMethods.clang_Comment_getKind(Struct);
            }
        }

        public int GetNumChildren()
        {
            return (int)NativeMethods.clang_Comment_getNumChildren(Struct);
        }

        public Comment GetChild(int index)
        {
            CXComment cxComment = NativeMethods.clang_Comment_getChild(Struct, (uint)index);
            return Create(cxComment);
        }

        public string GetText()
        {
            if (this.Kind != CommentKind.Text)
            {
                throw new InvalidOperationException();
            }

            CXString cxString = NativeMethods.clang_TextComment_getText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        public string GetParamName()
        {
            if (this.Kind != CommentKind.ParamCommand)
            {
                throw new InvalidOperationException();
            }

            CXString cxString = NativeMethods.clang_ParamCommandComment_getParamName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        public string GetCommandName()
        {
            if(this.Kind != CommentKind.BlockCommand)
            {
                throw new InvalidOperationException();
            }

            CXString cxString = NativeMethods.clang_BlockCommandComment_getCommandName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
