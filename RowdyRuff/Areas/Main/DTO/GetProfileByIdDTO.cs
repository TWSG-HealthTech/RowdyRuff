using System.Collections.Generic;

namespace RowdyRuff.Areas.Main.DTO
{
    public class GetProfileByIdDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public IEnumerable<SocialConnectionDTO> Connections { get; set; }

        public class SocialConnectionDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> Aliases { get; set; }
        }
    }
}
