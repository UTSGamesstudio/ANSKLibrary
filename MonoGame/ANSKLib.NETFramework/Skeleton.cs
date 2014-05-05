using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ANSKLibrary
{
    public class Skeleton
    {
        [ContentSerializer]
        private Joint _rootJoint;

        [ContentSerializer]
        private List<Joint> _unTreedJoints;

        public Skeleton()
        {
            _rootJoint = null;
            _unTreedJoints = new List<Joint>();
        }

        public void Init()
        {
            _rootJoint.Init();
        }

        public void AddJoint(Joint joint)
        {
            if (joint.IsRootJoint)
            {
                _rootJoint = joint;
                UpdateChildren();
            }
            else if (_rootJoint == null)
            {
                _unTreedJoints.Add(joint);
                return;
            }
            else
            {
                _unTreedJoints.Add(joint);
                UpdateChildren();
            }
        }

        private void UpdateChildren()
        {
            for (int i = 0; i < _unTreedJoints.Count; i++)
            {
                bool added = _rootJoint.AddChildNode(_unTreedJoints[i]);

                if (added)
                {
                    _unTreedJoints.RemoveAt(i);
                    i--;
                }
            }
        }

        public void FinaliseJointTree()
        {
            UpdateChildren();

            if (_unTreedJoints.Count > 0)
                throw new Exception("Joints still unlinked in final check.");
            else
                _unTreedJoints = null;
        }

        public List<Joint> ToJointList()
        {
            List<Joint> joints = new List<Joint>();
            _rootJoint.RetrieveNodes(ref joints);
            return joints;
        }

        public Matrix[] CollectAbsoluteBoneTransforms()
        {
            List<Matrix> transforms = new List<Matrix>();

            _rootJoint.RetrieveAbsoluteBoneTransforms(ref transforms);

            return transforms.ToArray<Matrix>();
        }
    }
}
