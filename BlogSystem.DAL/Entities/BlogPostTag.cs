using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL.Entities
{
    public class BlogPostTag
    {
        public virtual BlogPost BlogPost { get; set; }
        public int BlogPostId { get; set; }

        public virtual Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
