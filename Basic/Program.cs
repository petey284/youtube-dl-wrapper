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

    public interface IYoutubeDto
    {
        IYoutubeDto Build();
    }

    public class PlaylistInfo : IYoutubeDto
    {
        public string Id;
        public List<string> VideoIds;

        public PlaylistInfo(string playlistId)
        {
            this.Id = playlistId;
        }

        public IYoutubeDto Build()
        {
            this.VideoIds = GetPlaylistVideoIds(this.Id);
            return this;
        }

        private static List<string> GetPlaylistVideoIds(string playlistUrl)
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

    public class YoutubeDataManager
    {
        public List<IYoutubeDto> Items = new List<IYoutubeDto>();
        public YoutubeDataManager() { }
        public void Add(IYoutubeDto item) { 

            if (item.GetType() == typeof(PlaylistInfo))
            {
                this.Items.Add(item as PlaylistInfo);
            }
        }

        public void BuildAll()
        {
            foreach (var item in Items)
            {
                item.Build();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var myArgs = CommandLineProcessor.GetArgs(args);

            // Hardcode the playlist url for now but I need the playlist
            // url as one of the arguments
            var playlistId = "PL4cTJUshV2MCpGQuYti2_x0DLYkflTdNX";

            var manager = new YoutubeDataManager();
            manager.Add(new PlaylistInfo(playlistId));
            manager.BuildAll();

            var first = manager.Items.First();

            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
