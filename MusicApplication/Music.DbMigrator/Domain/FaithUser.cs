namespace Music.DbMigrator.Domain;

public class FaithUser
{
    public long id { get; set; }
    public string userName { get; set; }
    public string passWord { get; set; }
    public string phoneNumber { get; set; }
    public DateTime createTime { get; set; }
    public string userAddress { get; set; }
    public int isdel { get; set; }
    public string userEmail { get; set; }
}