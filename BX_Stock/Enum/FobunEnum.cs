using System.Runtime.Serialization;

namespace BX_Stock.Enum
{
    public enum FobunEventEnum
    {
        連線建立成功 = 100,
        登入成功 = 200,
        登入警示 = 201,
        斷線 = 300,
        未收到連線pong回傳 = 301,
        登出 = 302,
        錯誤 = 500,
    }

    public enum FobunTimeframe
    {
        [EnumMember(Value = "1")] OneMinute,
        [EnumMember(Value = "5")] FiveMinutes,
        [EnumMember(Value = "10")] TenMinutes,
        [EnumMember(Value = "15")] FifteenMinutes,
        [EnumMember(Value = "30")] ThirtyMinutes,
        [EnumMember(Value = "60")] SixtyMinutes,
        [EnumMember(Value = "D")] Day,
        [EnumMember(Value = "W")] Week,
        [EnumMember(Value = "M")] Month
    }

    public enum FobunSortOrder
    {
        [EnumMember(Value = "asc")] Asc,
        [EnumMember(Value = "desc")] Desc
    }
}
