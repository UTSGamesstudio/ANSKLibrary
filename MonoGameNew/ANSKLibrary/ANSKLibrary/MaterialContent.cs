using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ANSKLibrary
{
    public class MaterialContent
    {
        [ContentSerializer]
        public List<Material> Materials { get; set; }
        [ContentSerializer]
        public List<int> MaterialIndicieList { get; set; }

        public MaterialContent(List<Material> mats, List<int> matInd)
        {
            Materials = mats;
            MaterialIndicieList = matInd;
        }

        public MaterialContent() { }
    }
}
