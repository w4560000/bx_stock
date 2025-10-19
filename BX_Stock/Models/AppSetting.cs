namespace BX_Stock.Models
{
    public class AppSetting
    {
        public string Env { get; set; }
        public FobunSetting FobunSetting { get; set; }
    }

    public class FobunSetting
    {
        public string UserId { get; set; }
        public string UserPwd { get; set; }
        public string CAPath { get; set; }
    }
}
