using System.Collections.Generic;

namespace AppDist.Scaffolds
{
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkModel> Links { get; set; } = new List<LinkModel>();
    }
}
