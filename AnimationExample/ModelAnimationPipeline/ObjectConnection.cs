using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelAnimationPipeline
{
    public enum ObjectConnectionType { None = -1, IsRootJoint, BoneJointChildParent }

    public class ObjectConnection
    {
        private ObjectConnectionType _type;
        private int _id1;
        private string _object1Name;
        private int _id2;
        private string _object2Name;

        public ObjectConnectionType Type { get { return _type; } }
        public int Id1 { get { return _id1; } }
        public int Id2 { get { return _id2; } }
        public string Object1Name { get { return _object1Name; } }
        public string Object2Name { get { return _object2Name; } }

        public ObjectConnection(int id1, string object1Name, int id2, string object2Name)
        {
            _id1 = id1;
            _id2 = id2;
            _object1Name = object1Name;
            _object2Name = object2Name;
        }
    }
}
