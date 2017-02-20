namespace MemberTrack.Data
{
    using System;
    using Fclp;

    public class Arguments
    {
        public string Datasource { get; set; }

        public string Catalog { get; set; }

        public Arguments Parse(string[] args)
        {
            var parser = new FluentCommandLineParser<Arguments>();

            parser.Setup(arg => arg.Catalog).As('c', "Catalog").Required();

            parser.Setup(arg => arg.Datasource).As('d', "Datasource").Required();

            var result = parser.Parse(args);

            if (result.HasErrors) throw new InvalidOperationException($"Failed to parse application arguments: {result.ErrorText}");

            return parser.Object;
        }
    }
}