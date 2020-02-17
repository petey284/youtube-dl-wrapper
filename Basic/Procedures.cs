using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Basic
{
    public class Procedures
    {
        public IConfiguration Configuration;

        public Procedures() : this(new ConfigurationBuilder()
                .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly())
                .Build())
        {

        }

        internal Procedures(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public List<YoutubePlaylist> PrintVideoIds(string playlistId)
        {
            var manager = new YoutubeDataManager();

            manager.AddPlaylist(playlistId);

            manager.BuildAll();

            var currentPlaylist = manager.Items.First() as PlaylistInfo;

            var listOfVideos = currentPlaylist.VideoIds;

            if (listOfVideos == null)
            {
                Console.WriteLine("Get failed.");
                Environment.Exit(1);
            }
            
            return listOfVideos.AsEnumerable().Get();

        }

        public List<string> ReturnThoseNotDownloaded(string playlistId, List<string> other)
        {
            var onPlaylist = PrintVideoIds(playlistId);

            return onPlaylist.Select(x => x.id).Except(other).ToList();
        }

    }
}
