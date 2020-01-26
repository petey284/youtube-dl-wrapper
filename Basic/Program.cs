using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class CommandLineProcessor
    {
        public static List<string> GetArgs(string[] args)
        {
            return new List<string>();
        }
    }
    public class YoutubeApiController
    {
        public static List<string> GetPlaylistVideoIds(string playlistUrl)
        {
            var getVideoAsJsonCommand = $"/c youtube-dl.bat -j --flat-playlist {playlistUrl} ^| jq -r \".id\"";

            // For now I'm using my own process of getting the ids
            return new Standard("C:\\Windows\\System32\\cmd.exe")
                .RunAndWaitForExit(getVideoAsJsonCommand, true)
                .TakeContent()
                .Split('\n')
                .ToList();
        }
    }

    public class YoutubeDownloadManager
    {
        public string PlaylistId;
        public List<string> AllVideosList;
        public YoutubeDownloadManager() { }

        public YoutubeDownloadManager(string playlistId)
        {
            this.PlaylistId = playlistId;
        }

        public void GetValues()
        {
            // Get all the videos
            this.AllVideosList = YoutubeApiController.GetPlaylistVideoIds(this.PlaylistId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var myArgs = CommandLineProcessor.GetArgs(args);

            // Hardcode the playlist url for now but I need the playlist
            // url as one of the arguments
            var playlistUrl = "PL4cTJUshV2MCpGQuYti2_x0DLYkflTdNX";

            var manager = new YoutubeDownloadManager(playlistUrl);
            manager.GetValues();

            Console.WriteLine(manager.AllVideosList.Count());

            Console.ReadLine();
        }
    }
}
