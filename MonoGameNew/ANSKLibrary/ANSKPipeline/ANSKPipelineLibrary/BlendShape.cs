using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class BlendShape
    {
        private List<Vector3> _verts;
        private List<int> _ind;
        private List<Vector3> _norms;
        private string _name;
        private bool _keyed;

        public string Name { get { return _name; } }
        public List<int> Indices { get { return _ind; } }
        public List<Vector3> Vertices { get { return _verts; } }

        public BlendShape(List<Vector3> verts, List<int> ind, List<Vector3> norms, string name, bool keyed)
        {
            _verts = verts;
            _ind = ind;
            _norms = norms;
            _name = name;
            _keyed = keyed;
        }

        public BlendShape(BlendShapeContent content)
        {
            content.GrabContent(ref _verts, ref _ind, ref _norms, ref _name, ref _keyed);
        }
    }
}
