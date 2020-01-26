using System;
using System.Collections.Generic;

namespace Basic
{
    public class Processor
    {
        public static List<string> GetArgs()
        {
            return new List<string>();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var myArgs = Processor.GetArgs();

            // Hardcode the playlist url for now
            var playlistUrl = "PL4cTJUshV2MCpGQuYti2_x0DLYkflTdNX";

            // I need the playlist url as one of the arguments
            var cmdPrompt = new Standard("C:\\Windows\\System32\\cmd.exe");

            // var environmentOutput= cmdPrompt.RunCommand($"/k echo %PATH%");

            var output = cmdPrompt.Run($"/c youtube-dl.bat -j --flat-playlist {playlistUrl} ^| jq -r \".id\"", true);
            Console.WriteLine(output.TakeContent());

            Console.ReadLine();
        }
    }
}
