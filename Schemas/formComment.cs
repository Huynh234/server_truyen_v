using server_truyen_v.Models;

namespace server_truyen_v.Schemas
{
    public class formComment

    {

        public int storid { get; set; }

        public List<String> comment { get; set; } = new List<String>();

        public double ratting { get; set; }

    }
}