using Music.Base;

namespace Music.Model.DTO;

public class BaseEntity
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long _id { get; set; }
    public long id
    {
        get
        {
            // 在get访问器中，如果_createTime尚未被初始化，则初始化为当前时间
            return _id == null ? SnowflakeIdGenerator.getSnowId() : _id;
        }
        set
        {
            // 在set访问器中，可以添加一些额外的逻辑，如果有需要的话
            _id = value;
        }
    }
    private DateTime _createTime { get; set; }
    public DateTime createTime
    {
        get
        {
            // 在get访问器中，如果_createTime尚未被初始化，则初始化为当前时间
            return _createTime == DateTime.MinValue ? DateTime.Now : _createTime;
        }
        set
        {
            // 在set访问器中，可以添加一些额外的逻辑，如果有需要的话
            _createTime = value;
        }
    }
    public string createBy { get; set; }
    public string _isDel { get; set; }
    public string isDel {
        get
        {
            return string.IsNullOrEmpty(isDel) ? "0" : isDel;
        }
        set
        {
            _isDel = value;
        }
    }
    public DateTime updateTime { get; set; }
    public string updateBy { get; set; }
    public string arr02 { get; set; }
    public string arr03 { get; set; }
    
    
}