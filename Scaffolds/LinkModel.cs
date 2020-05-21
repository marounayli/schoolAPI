using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace AppDist.Scaffolds
{
    [NotMapped]
    public class LinkModel
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }
        public LinkModel(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;

        }
    }
}