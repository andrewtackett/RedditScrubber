using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;
using System.IO;
using RedditSharp.Things;

namespace RedditScrubber
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] creds = File.ReadAllLines("creds.txt");
            var reddit = new Reddit();
            var user = reddit.LogIn(creds[0], creds[1]);
            int limit = 100;
            Listing<RedditSharp.Things.VotableThing> posts = user.GetOverview(Sort.New, limit, FromTime.All);

            //Command line arguments:
            //Cutoff Date
            //Cutoff karma
            //Dryrun, don't actually delete anything
            //Switch, posts only
            //Switch, comments only (can't do both)
            DateTime cutoffDate= DateTime.Now;
            int cutoffScore = int.MaxValue;
            bool dryRun = true;
            bool onlyPosts = false;
            bool onlyComments = false;

            //Try to parse command line arguments.  If that fails revert back to defaults
            if (args.Length > 0)
                if (!DateTime.TryParse(args[0], out cutoffDate))
                    cutoffDate = DateTime.Now;
            if (args.Length > 1)
                if (!int.TryParse(args[1], out cutoffScore))
                    cutoffScore = int.MaxValue;
            if (args.Length > 2)
                if (!bool.TryParse(args[2], out dryRun))
                    dryRun = true;
            if (args.Length > 3)
                if (!bool.TryParse(args[3], out onlyPosts))
                    onlyPosts = false;
            if(args.Length > 4)
                if (! onlyPosts && !bool.TryParse(args[4], out onlyComments))
                    onlyComments = false;

            Console.WriteLine("Configuration: ");
            Console.WriteLine("==============");
            Console.WriteLine("Cutoff date: " + cutoffDate);
            Console.WriteLine("Cutoff score:" + cutoffScore);
            Console.WriteLine("Dry Run: " + dryRun);
            Console.WriteLine("Only deleting posts: " + onlyPosts);
            Console.WriteLine("Only deleting comments: " + onlyComments);

            Console.WriteLine("--------------------------------------------");

            foreach (var thing in posts)
            {
                bool willBeDeleted = false;
                Console.WriteLine("createdUTC: " + thing.CreatedUTC);
                Console.WriteLine("score: " + thing.Score);
                Console.WriteLine("Post or Comment: " + thing.GetType());
                if (thing.Created < cutoffDate && thing.Score < cutoffScore)
                    willBeDeleted = true;
                var comment = thing as Comment;
                if(comment!= null)
                {
                    Console.WriteLine("Subreddit: " + comment.Subreddit);
                    Console.WriteLine("Comment body:" + comment.Body);
                    Console.Write("Deleted? ");
                    if (!onlyPosts && willBeDeleted)
                    {
                        Console.WriteLine("Yes");
                        if(!dryRun)
                            comment.Del();
                    }
                    else
                        Console.WriteLine("No");
                }else
                {
                    var post = thing as Post;
                    Console.WriteLine("Post title: " + post.Title);
                    Console.WriteLine("Post self text: " + post.SelfText);
                    Console.WriteLine("Post url: " + post.Url);
                    Console.Write("Deleted? ");
                    if (!onlyComments && willBeDeleted)
                    {    
                        Console.WriteLine("Yes");
                        if(!dryRun)
                            post.Del();
                    }
                    else
                        Console.WriteLine("No");
                }
                Console.WriteLine("--------------------------------------------");
            }

            Console.WriteLine("Press enter to exit...");
            Console.Read();
        }
    }
}
