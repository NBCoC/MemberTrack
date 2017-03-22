﻿using MemberTrack.Common;
using System.Collections.Generic;

namespace MemberTrack.Data.RawData
{
    public static class SpiritualGiftsDiscoveryQuizData
    {
        public const string QuizName = "Spiritual Gifts Discovery Quiz";
        public const string TopicCategory = "Spiritual Gifts";

        public static readonly TupleList<string, string, List<string>> Topics = new TupleList<string, string, List<string>>
        {
            { "HELPS", "The gift of helps is the distinctive ability to work with and support other Christian's ministry efforts.",
                        new List<string> { "Mark 15:40-41", "Acts 9:36", "Romans 16:1-2", "1 Corinthians 12:28" } },
            { "LEADERSHIP", "The gift of leadership is the distinctive ability to influence others according to a 'big picture' purpose, mission, or plan.",
                        new List<string> { "Romans 12:8", "1 Timothy 3:1-13", "1 Timothy 5:17", "Hebrews 13:1-7" } },
            { "HOSPITALITY", "The gift of hospitality is the distinctive ability to make people feel 'at home,' welcome, cared for, and part of the group.",
                        new List<string> { "Acts 16:14-15", "Romans 12:13", "Romans 16:23", "Hebrews 13:1-2", " 1 Peter 4:9" } },
            { "SERVICE", "The gift of service is the distinctive ability to identify and meet the practical needs of others.",
                        new List<string> { "Acts 6:1-7", "Romans 12:7", "Galatians 6:10", "2 Timothy 1:16-18", " Titus 3:14" } },
            { "ADMINISTRATION", "The gift of administration is the distinctive ability to coordinate and organize people and projects.",
                        new List<string> { "Luke 14:28-30", "Acts 6:1-7", "1 Corinthians 12:28" } },
            { "DISCERNMENT", "The gift of discernment is the distinctive ability to perceive whether a person's actions originate from Godly, satanic, or merely human sources.",
                        new List<string> { "Matthew 16:21-23", "Acts 5:1-11", "Acts 16:16-18", "1 Corinthians 12:10", "1 John 4:1-6" } },
            { "FAITH", "The gift of faith is the distinctive ability to believe God with confidence for things unseen, spiritual growth and the will of God.",
                        new List<string> { "Acts 11:22-24", "Romans 4:18-21", "1 Corinthians 12:9", "Hebrews 11" } },
            { "MUSIC", "The gift of music is the distinctive ability to make a significant contribution to a worship experience through singing or playing a musical instrument.",
                        new List<string> { "Deuteronomy 31:22", "1 Samuel 16:16", "1 Chronicles 16:41-42", "2 Chronicles 5:12-13", "2 Chronicles 34:12", "Psalm 50" } },
            { "CRAFTSMANSHIP", "The gift of craftsmanship is the distinctive ability to use your hands and mind to benefit other believers through artistic, creative or a wide variety of construction arenas.",
                        new List<string> { "Exodus 30:22-25", "Exodus 31:3-11", "2 Chronicles 34:9-13", "Acts 18:2-3" } },
            { "GIVING", "The gift of giving is the distinctive ability to cheerfully and generously contribute personal resources to God's work.",
                        new List<string> { "Mark 12:41-42", "Romans 12:18", "2 Corinthians 8:1-7", "2 Corinthians 9:2-7" } },
            { "MERCY", "The gift of mercy is the distinctive ability to feel sincere empathy and compassion in a way that results in practical relief for people's hurts, pain and suffering.",
                        new List<string> { "Matthew 9:35,36", "Mark 9:41", "Romans 12:8", "1 Thessalonians 5:14" } },
            { "WISDOM", "The gift of wisdom is the distinctive ability to discern the mind of Christ and apply scriptural truth to a specific situation in order to make the right choices and help others move in the right direction.",
                        new List<string> { "Acts 6:3,10", "1 Corinthians 2:6-13", "1 Corinthians 12:8" } },
            { "KNOWLEDGE", "The gift of knowledge is the distinctive ability to seek out, gather, organize, and clarify facts and ideas on a number of diverse subjects.",
                        new List<string> { "Acts 5:1-11", "1 Corinthians 12:8", "Colossians 2:2-3" } },
            { "EXHORTATION", "The gift of exhortation is the distinctive ability to appropriately communicate words of encouragement, challenge, or rebuke in the body of Christ.",
                        new List<string> { "Acts 14:22", "Romans 12:8", "1 Timothy 4:13", "Hebrews 10:24-25" } },
            { "TEACHING", "The gift of teaching is the distinctive ability to employ a logical, systematic approach to biblical study in preparation for clearly communicating practical truth to the body of Christ.",
                        new List<string> { "Acts 18:24-28", "Acts 20:20-21", "1 Corinthians 12:28", "Ephesians 4:11- 14" } },
            { "PASTOR/SHEPHERD", "The gift of pastor/shepherd is the distinctive ability to assume responsibility for the spiritual growth and Christian community of a group of believers.",
                        new List<string> { "John 10:1-18", "Ephesians 4:11-14", "1 Timothy 3:1-7", "1 Peter 5:1-3" } },
            { "APOSTLESHIP", "The gift of apostleship is the distinctive ability to provide spiritual leadership over a number of pastors and churches that results in tangible fruit in ministry.",
                        new List<string> { "Acts 15:22-35", "1 Corinthians 12:28", "2 Corinthians 12:12", "Galatians 2:7-10", "Ephesians 4:11-14" } },
            { "MISSIONARY", "The gift of missionary is the distinctive ability to minister effectively in cultures beyond your own.",
                        new List<string> { "Acts 8:4", "Acts 3:2-3", "Acts 22:21", "Romans 10:15" } },
            { "PROPHECY", "The gift of prophecy is the distinctive ability to boldly declare the truth of God, regardless of the consequences, calling people to righteous living.",
                        new List<string> { "Acts 2:37-40", "Acts 7:51-53", "Acts 26:24-29", "1 Corinthians 14:1-4", "1 Thessalonians 1:5" } },
            { "EVANGELISM", "The gift of evangelism is the distinctive ability to share the good news of Jesus Christ with others in such a way that many non-Christians believe in Christ and are converted to Christianity.",
                        new List<string> { "Acts 8:5-6", "Acts 8:26-40", "Acts 14:21", "Acts 21:8", "Ephesians 4:11-14" } },
            { "INTERCESSION", "The gift of intercession is the distinctive ability to pray for significant lengths of time, on a regular basis, and often observe specific answers to those prayers.",
                        new List<string> { "Colossians 1:9-12", "Colossians 4:12-13", "James 5:14-16" } },
        };

        //The other properties of a question are all the same in this quiz, so just make a list of the question text only
        public static readonly List<string> QuizQuestionText = new List<string>
        {
            "I enjoy working behind the scenes, taking care of little details.",
            "I usually step forward and assume leadership In a group where none exists.",
            "When in a group, I tend to notice and approach those who are alone to help them feel part of the group.",
            "I have the ability to recognize a need and to get the job done, no matter how small the task.",
            "I have the ability to organize ideas, people and projects to reach a specific goal.",
            "People often say I have good spiritual judgment.",
            "I am very confident of achieving great things for the glory of God.",
            "I am asked to sing or play a musical instrument at church functions.",
            "I have an ability to use my hands in a creative way to design and build things.",
            "I enjoy giving money to those in serious financial need.",
            "I enjoy ministering to people in hospitals, prisons, or rest homes to comfort them.",
            "I often have insights that offer practical solutions to difficult problems.",
            "I have often understood issues or problems in the church and have seen answers when others didn't.",
            "I enjoy encouraging and giving counsel to those who are discouraged.",
            "I have an ability to thoroughly study a passage of scripture, and then share it with others.",
            "I presently have the responsibility for the spiritual growth of one or more young Christians.",
            "Other people respect me as an authority in spiritual matters.",
            "I have an ability to learn foreign languages.",
            "God often reveals to me the direction He desires the body of Christ to move.",
            "I enjoy developing relationships with non-Christians, especially with hopes of telling them about Jesus.",
            "Whenever I hear reports on the news or in conversations about needy situations, I am burdened to pray.",
            "I would like to assist pastors or other leaders so they will have more time to accomplish their essential and priority ministries.",
            "When I ask people to help me accomplish an important ministry for the church, they usually say yes.",
            "I enjoy entertaining guests and making them feel 'at home' when they visit.",
            "I take initiative to serve and enjoy serving others, no matter how small the task.",
            "I am a very organized person who sets goals, makes plans, and achieves the goals.",
            "I am a good judge of character and can spot a spiritual phony.",
            "I often step out and start projects that other people won't attempt, and the projects are usually successful.",
            "I believe I could make a positive difference on the music or worship team.",
            "I enjoy doing things like woodworking, crocheting, sewing, metal work, stained glass, etc.",
            "I joyfully give money to the church well above my tithe.",
            "I feel compassion for people who are hurting and lonely, and like to spend considerable time with them to cheer them up.",
            "God has enabled me to choose correctly between several complex options in an important decision, when others didn't seem to know what to do.",
            "I enjoy studying difficult questions about God's Word, and I am able to find answers more easily and more quickly than others.",
            "I'm very fulfilled when I encourage others, especially if it's about their spiritual growth.",
            "When a question arises from a difficult Bible passage, I am motivated to research the answer.",
            "I enjoy being involved in people's lives and helping them grow spiritually.",
            "I would be willing and excited to be part of a new church plant.",
            "I can adapt easily to cultures, languages, and lifestyles, other than my own, and I would like to use my adaptability to minister in foreign countries.",
            "I will always speak up for Christian principles with conviction, even when what I say isn't popular.",
            "I find it easy to invite a person to accept Jesus as their Savior.",
            "I have a passion to pray for the significant issues of God's kingdom and His will for Christians.",
            "I enjoy relieving others of routine tasks so they can get important projects done.",
            "I can guide and motivate a group of people toward achieving a specific goal.",
            "I enjoy meeting new people and introducing them to others in the group.",
            "I am very dependable for getting things done on time, and I don't need much praise or thanks.",
            "I easily delegate significant responsibilities to other people.",
            "I am able to distinguish between right and wrong in complex spiritual matters, seemingly more easily than others.",
            "I trust in God's faithfulness for a bright future, even when facing significant problems.",
            "I enjoy singing, and people say I have a good voice.",
            "I find satisfaction in meeting the needs of people by making something for them.",
            "I wouldn't mind lowering my standard of living to give more to the church, and others in need.",
            "I want to do whatever I can for the needy people around me, even if I have to give up something.",
            "People often seek my advice when they don't know what to do in a situation.",
            "I have an ability to gather information from several sources to discover the answer to a question, or learn more about a subject.",
            "I feel a need to challenge others to better themselves, especially in their spiritual growth, in an uplifting, rather than condemning way.",
            "Others listen and enjoy my teaching of scriptures.",
            "I care about the spiritual welfare of people, and do my best to guide them toward a Godly lifestyle.",
            "I am accepted as a spiritual authority in other parts of the country or world.",
            "I would like to present the gospel in a foreign language, in a country whose culture and lifestyle is different than my own.",
            "I feel a need to speak God's messages from the Bible so people will know what God expects of them.",
            "I would like to tell others how to become a Christian, and give them an invitation to receive Jesus into their life.",
            "Many of my prayers for others have been answered by the Lord.",
            "I enjoy helping others get their work done, and don't need a lot of public recognition.",
            "People respect my opinion and follow my direction.",
            "I would like to use my home to get acquainted with newcomers and visitors to the church.",
            "I enjoy helping people in any type of need, and feel a sense of satisfaction in meeting that need.",
            "I am comfortable making important decisions, even under pressure.",
            "People come to me for help in distinguishing between spiritual truth and error.",
            "I often exercise my faith through prayer, and God answers my prayers in powerful ways.",
            "I believe the Lord could use me to deliver a message through song.",
            "People say I am gifted with my hands.",
            "When I give money to someone, I don't expect anything in return, and often give anonymously.",
            "When I hear of people without jobs who can't pay their bills, I do what I can to help them.",
            "God enables me to make appropriate application of biblical truth to practical situations.",
            "I can discover difficult biblical truths and principles on my own, and I enjoy this.",
            "People find it easy to talk with me and respond well to my encouragement to become all they can for God.",
            "I am organized in my thinking and systematic in my approach to presenting Bible lessons to a group of people.",
            "I help Christians who have wandered away from the Lord find their way back to a growing relationship with Him and get involved in a local church.",
            "I would be excited to share the gospel and form new groups of Christians in areas where there aren't many churches.",
            "I have no racial prejudice, and have a sincere appreciation for people very different from myself.",
            "I find it relatively easy to apply biblical promises to present day situations, and I'm willing to confront in love if necessary.",
            "I have a strong desire to help non-Christians find salvation through Jesus Christ.",
            "Prayer is my favorite ministry in the church and I consistently spend a great deal of time at it.",
        };

        public static readonly List<string> AnswersText = new List<string>
        {
            "not at all",
            "little",
            "moderately",
            "considerably",
            "strongly",
        };
    }
}