using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AST_Intranet.Models
{
    public class GalleryViewModel
    {
       
            public List<string> Images { get; set; }
            public int TotalPages { get; set; }
            public int CurrentPage { get; set; }
        
    }
}