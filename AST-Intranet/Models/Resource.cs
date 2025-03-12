using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AST_Intranet.Models
{
    public class Resource
    {
        public string Name { get; set; }
        public List<string> Files { get; set; }  // Files directly under this resource
        public List<Subcategory> SubCategories { get; set; }  // Subcategories (folders within this resource)


    }

    public class Subcategory
    {
        public string Name { get; set; }
        public List<string> Files { get; set; }  // Files directly under this subcategory
        public List<Subcategory> SubCategories { get; set; }  // Sub-subcategories
    }

}