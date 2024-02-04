using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Model.DTO.Music
{
    public class AudioFileInfo
    {
        public string MusicComposer { get; set; }
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string MusicName { get; set; }
        /// <summary>
        /// 歌曲专辑
        /// </summary>
        public string MusicAlbum { get; set; }
        /// <summary>
        /// 星级
        /// </summary> 
        public string MusicStar { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public string MusicDuration { get; set; }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string MusicSingerName { get; set; }
    }
}
