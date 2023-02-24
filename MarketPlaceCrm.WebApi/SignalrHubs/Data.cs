namespace MarketPlaceCrm.WebApi.SignalrHubs
{
    public class Data
    {
        public Data(int id, string msg)
        {
            Id = id;
            Message = msg;
        }
        public int Id { get; set; }
        public string Message { get; set; }
    }
}