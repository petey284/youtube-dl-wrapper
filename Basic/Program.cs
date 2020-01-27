using Newtonsoft.Json;
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

    public class VideoInfo : IYoutubeDto
    {
        public VideoInfo(string videoId) { }
        public IYoutubeDto Build() { return this; }
    }

    public class YoutubePlaylist
    {
        public string id;
        public string _type;
        public string ie_key;
        public string title;
        public string url;
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
            this.VideoIds = GetPlaylistVideoIdsJson(this.Id);
            return this;
        }

        // Return list of playlists
        private static List<YoutubePlaylist> GetPlaylistVideoIds(string playlistUrl)
        {
            // var getVideoAsJsonCommand = $"/c youtube-dl.bat -j --flat-playlist {playlistUrl} ^| jq -r \".id\"";
            var getVideoAsJsonCommand = $"/c youtube-dl.exe -j --flat-playlist {playlistUrl}";

            // For now I'm using my own process of getting the ids
            return new Standard("C:\\Windows\\System32\\cmd.exe")
                .RunAndWaitForExit(getVideoAsJsonCommand, true)
                .TakeContent()
                .Split('\n')
                .Select(s => JsonConvert.DeserializeObject<YoutubePlaylist>(s))
                .ToList();
        }

        private static List<string> GetPlaylistVideoIdsJson(string playlistUrl)
        {
            // var getVideoAsJsonCommand = $"/c youtube-dl.bat -j --flat-playlist {playlistUrl} ^| jq -r \".id\"";
            var getVideoAsJsonCommand = $"/c youtube-dl.exe -j --flat-playlist {playlistUrl}";

            //  TODO: { ^| jq -r \".id\" } => convert to json and pull out id

            // For now I'm using my own process of getting the ids
            return new Standard("C:\\Windows\\System32\\cmd.exe")
                .RunAndWaitForExit(getVideoAsJsonCommand, true)
                .TakeContent()
                .Split('\n')
                .ToList();
        }
    }

    public static class Ex
    {
        public static List<YoutubePlaylist> Get(this IEnumerable<string> json)
        {
            return json.Select(s => JsonConvert.DeserializeObject<YoutubePlaylist>(s)).ToList();
        }
    }

    public class YoutubeDataManager
    {
        public IEnumerable<IYoutubeDto> Items = new List<IYoutubeDto>();
        public YoutubeDataManager() { }

        public void AddVideo(string videoId)
        {
            var list = this.Items as List<IYoutubeDto>;

            list.Add(new VideoInfo(videoId));

            this.Items = list;
        }

        public void AddPlaylist(string playlistId)
        {
            var list = this.Items as List<IYoutubeDto>;

            list.Add(new PlaylistInfo(playlistId));
            this.Items = list;
        }

        public void BuildAll()
        {
            foreach (var item in Items)
            {
                item.Build();
            }
        }

        public List<int> GetCounts()
        {
            var counts = new List<int>();
            foreach (var item in Items)
            {
                var count = (item as PlaylistInfo).VideoIds.Count();
                counts.Add(count);
            }

            return counts;
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

            manager.AddPlaylist(playlistId);

            manager.BuildAll();

            var currentPlaylist = manager.Items.First() as PlaylistInfo;

            var listOfVideos = currentPlaylist.VideoIds.AsEnumerable().Get();

            foreach (var ids in listOfVideos)
            {
                if (ids != null) { Console.WriteLine(ids.id); } 
            }

            Console.ReadLine();
        }
    }
}