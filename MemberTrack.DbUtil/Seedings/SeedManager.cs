using MemberTrack.Common;
using MemberTrack.Data;
using MemberTrack.Data.Seedings;
using System;
using System.Collections.Generic;

namespace MemberTrack.DbUtil.Seedings
{
    class SeedManager : SeedBase
    {
        private readonly List<SeedBase> _seedlings;

        public SeedManager(DatabaseContext databaseContext, bool forceReseeding)
            :base(databaseContext, forceReseeding)
        {
            _seedlings = new List<SeedBase>
            {
                new SeedSystemAccount(databaseContext, forceReseeding),
                new SeedPersonCheckListItem(databaseContext, forceReseeding),

                //Quizzes
                new SeedSpiritualGiftsDiscoveryQuiz(databaseContext, forceReseeding),
            };
        }

        public override void Seed()
        {
            //First call our base method to cover any general database seeding 
            try
            {
                base.Seed();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed general database.");
                Console.WriteLine(ex.ToDetail());
            }

            foreach (SeedBase seedling in _seedlings)
            {
                try
                {
                    seedling.Seed();
                }
                catch(Exception ex)
                {
                    var seedlingName = seedling.GetType().Name;
                    var seedTopic = (seedlingName.StartsWith("Seed") ? seedlingName.Substring(4) : seedlingName);
                    Console.WriteLine($"Failed to seed '{seedTopic}'");
                    Console.WriteLine(ex.ToDetail());
                }
            }
        }

        protected override void ClearData()
        {
            Console.WriteLine("Forcing the reseeding of all database data.");
        }

        protected override void PopulateData()
        {

        }

    }
}
  