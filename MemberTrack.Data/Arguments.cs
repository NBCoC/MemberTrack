namespace MemberTrack.Data
{
    using System;
    using Fclp;

    public class Arguments
    {
        public string Datasource { get; set; } = "localhost";

        public string Catalog { get; set; } = "MemberTrack";

        public bool ForceReseeding { get; set; }

        public Arguments Parse(string[] args)
        {
            if (args.Length == 0)
            {
                return this;
            }

            var parser = new FluentCommandLineParser<Arguments>();

            parser.Setup(arg => arg.Catalog).As('c', "Catalog");

            parser.Setup(arg => arg.Datasource).As('d', "Datasource");

            parser.Setup(arg => arg.ForceReseeding).As('f', "ForceReseeding").SetDefault(true);

            var result = parser.Parse(args);

            if (result.HasErrors)
            {
                throw new InvalidOperationException($"Failed to parse application arguments: {result.ErrorText}");
            }

            return parser.Object;
        }
    }
}