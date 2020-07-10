namespace GameClientApi.Domain.DTO
{
    public class IGDBNextGameRelease
    {
        public long id { get; set; }
        public long date { get; set; }
        public IGDBGame game { get; set; }
        public string human { get; set; }
        public IGDBPlatform platform { get; set; }
    }
}
