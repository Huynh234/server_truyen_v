

using Microsoft.EntityFrameworkCore.Migrations;

namespace server_truyen_v.Schemas
{

    public class formStory
    {
        public string storyName { get; set; } = "";
        public string storyCode { get; set; } = "";
        public string autho { get; set; } = "";
        public string imgCover { get; set; } = "";
        public string description { get; set; } = "";
        public string typeStory { get; set; } = "";
        public string typeDetailStory { get; set; } = "";

    }

}
