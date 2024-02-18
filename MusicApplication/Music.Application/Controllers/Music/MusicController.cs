using Microsoft.AspNetCore.Mvc;
using Music.DbMigrator.Domain;
using Music.IService.Music;
using Music.Model.DTO.Music;
using Music.Model.DTO;
using Music.Model.VO.UpLoad;

namespace MusicApplication.Controllers.Music
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController
    {
        private readonly IMusicInfoService _service;
        public MusicController(
        IMusicInfoService _service
        )
        {
            this._service = _service;
        }

        /// <summary>
        /// 获取音频列表
        /// </summary>
        /// <param name="vo">获取列表vo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultDto<MusicInfo>> getMusicListAsync([FromBody] GetMusicDto vo)
        {
            return await _service.getMusicListAsync(vo);
        }

        /// <summary>
        /// 删除音频
        /// </summary>
        /// <param name="musicId">音频主键</param>
        /// <returns></returns>
        [HttpDelete("{musicId}")]
        public async ValueTask<bool> DeleteMusicAsync([FromRoute] long musicId)
        {
            return await _service.DeleteMusicAsync(musicId);
        }

        /// <summary>
        /// 获取音频详情
        /// </summary>
        /// <param name="musicId">音频主键</param>
        /// <returns></returns>
        [HttpGet("{musicId}")]
        public async Task<ResultDto<MusicInfo>> GetMusicAsync([FromRoute] long musicId)
        {
            return await _service.GetMusicAsync(musicId);
        }


        /// <summary>
        /// 上传文件到服务器 信息入库
        /// </summary>
        /// <param name="vo">上传vo</param>
        /// <returns></returns>
        [HttpPut]
        public async ValueTask<bool> upDateMusicAsync([FromBody] UpLoadVo vo)
        {
            return await _service.upDateMusicAsync(vo);
        }
    }
}
