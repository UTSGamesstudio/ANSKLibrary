using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//public enum JointRetrievalType { BindPose, InverseBindPose, SkeletonHierarchy }

namespace ModelAnimationLibrary
{
    public class Joint
    {
        [ContentSerializer]
        private string _name;
        [ContentSerializer]
        private int _id;
        [ContentSerializer]
        private int _parentId;
        [ContentSerializer]
        private Matrix _translation;
        [ContentSerializer]
        private Matrix _rotation;
        [ContentSerializer]
        private Matrix _scale;
        [ContentSerializer]
        private Matrix _transformation;
        //[ContentSerializer]
        private Joint _parent;
        [ContentSerializer]
        private List<Joint> _children;
        // The order of indices and weights must match.
        [ContentSerializer]
        private List<int> _indices;
        [ContentSerializer]
        private List<float> _weights;

        public int Id { get { return _id; } }
        public int ParentId { get { return _parentId; } }
        public bool IsRootJoint { get { return (_id == _parentId); } }

        public Joint(string name, int id, int parentId, Vector3 translation, Vector3 rotation, Vector3 scale, List<int> indices, List<float> weights)
        {
            _name = name;
            _id = id;
            _parentId = parentId;
            _parent = null;
            _children = new List<Joint>();
            _translation = Matrix.CreateTranslation(translation);
            _rotation = Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X));
            _rotation *= Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y));
            _rotation *= Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z));
            _scale = Matrix.CreateScale(scale);
            _transformation = _scale * _rotation * _translation;
            _indices = indices;
            _weights = weights;
        }

        public Joint() { }

        public void Init()
        {
            _parent = this;

            foreach (Joint j in _children)
                j.Init(this);
        }

        private void Init(Joint parent)
        {
            _parent = parent;

            foreach (Joint j in _children)
                j.Init(this);
        }

        public bool AddChildNode(Joint child)
        {
            // If the child Joint is the parent of the child we need to add.
            if (_id == child.ParentId)
            {
                child._parent = this;
                _children.Add(child);
                return true;
            }

            // If the child Joint is not this Joint's child, check through the current children
            // of this Joint.
            for (int i = 0; i < _children.Count; i++)
            {
                bool done = _children[i].AddChildNode(child);

                if (done)
                    return true;
            }

            // If this Joint and it's children are not the parent or we hit the end Joint,
            // just go back up the tree.
            return false;
        }

        public void RetrieveNodes(ref List<Joint> joints)
        {
            joints.Add(this);

            foreach (Joint j in _children)
                j.RetrieveNodes(ref joints);
        }

        public void RetrieveAbsoluteBoneTransforms(ref List<Matrix> trans)
        {
            if (!IsRootJoint)
            _transformation = _parent._transformation * _transformation;

            trans.Add(_transformation);

            foreach (Joint j in _children)
                j.RetrieveAbsoluteBoneTransforms(ref trans);
        }

        public bool IsIndicePartOfThisJoint(int indice)
        {
            bool isPartOf = false;

            for (int i = 0; i < _indices.Count; i++)
            {
                if (_indices[i] == indice)
                    isPartOf = true;
            }

            return isPartOf;
        }

        public float GetWeight(int indice)
        {
            float num = -1;

            for (int i = 0; i < _indices.Count; i++)
            {
                if (_indices[i] == indice)
                    num = _weights[i];
            }

            return num;
        }
    }
}
