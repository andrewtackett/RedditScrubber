# RedditScrubber

Quick and dirty program to delete posts and comments from a user's reddit history.

# Usage

The program will look for a file named "creds.txt" in the same directory.  This file should contain your reddit username on the first line and your password on the second.  There are five optional arguments you may pass to the program:

* Cutoff Date - Date string e.g. "09/28/2015", default today
* Cutoff Karma - integer, default infinite
* Dryrun - true/false, default true
* Posts only - true/false, default false
* Comments only - true/false, default false

*Cutoff Date* specifies up to what date you wish to delete comments/posts.  Any made after this date won't be deleted.  **Note**: This is specified in UTC.

*Cutoff Karma* specifies a maximum amount of karma you want deleted items to have.

*Dryrun* will make the program output information about what would be deleted but not actually delete it - this could be useful for testing out what posts will get deleted with a given combination of arguments without any danger.

*Posts only* will make the program only delete posts, not comments if set to true.

*Comments only* will make the program only delete comments, not posts if set to true.

Example usage: RedditScrubber.exe 09/20/2015 25 true true false
