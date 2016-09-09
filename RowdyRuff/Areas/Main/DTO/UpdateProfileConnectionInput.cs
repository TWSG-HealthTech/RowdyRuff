using System.Collections.Generic;

namespace RowdyRuff.Areas.Main.DTO
{
    public class UpdateProfileConnectionInput
    {
        public string Name { get; set; }
        public List<string> Aliases { get; set; }
    }
}
