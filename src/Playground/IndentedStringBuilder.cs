using System.Text;
using Core.Diagnostics;

namespace Playground
{
    public class IndentedStringBuilder
    {
        private static readonly string indent = new string(' ', 4);

        private StringBuilder builder;

        public string IndentString { get; }

        private IndentedStringBuilder(StringBuilder builder, string indentString)
        {
            Requires.NotNull(builder, nameof(builder));

            this.builder = builder;
            IndentString = indentString;
        }

        public IndentedStringBuilder()
            : this(new StringBuilder(), string.Empty)
        { }

        public IndentedStringBuilder IncreaseIndent()
        {
            return new IndentedStringBuilder(builder, IndentString + indent);
        }

        public IndentedStringBuilder Append(string value)
        {
            builder.Append(value);
            return this;
        }

        public IndentedStringBuilder AppendLine()
        {
            builder.AppendLine();
            return this;
        }

        public IndentedStringBuilder AppendLine(string line)
        {
            builder.Append(IndentString).AppendLine(line);
            return this;
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
