#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace ANSKPipelineLibrary
{
    /// <summary>
    /// Combines all the data needed to render and animate a skinned object.
    /// This is typically stored in the Tag property of the Model being animated.
    /// </summary>
    public class SkinningData
    {
        /// <summary>
        /// Constructs a new skinning data object.
        /// </summary>
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<BoneData> skeletonHierarchy, List<BoneData> headSkele) // Add a new parameter that takes a List<int> of head bone ints.
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
            HeadSkeletonHierarchy = headSkele;
        }

        public void ExportToXML(XmlWriter file)
        {
            List<AnimationClip> clips = new List<AnimationClip>(AnimationClips.Values);

            file.WriteStartElement("AnimationClips");
            for (int i = 0; i < clips.Count; i++)
            {
                file.WriteStartElement("AnimationClip");

                clips[i].ExportToXML(file);

                file.WriteEndElement();
            }
            file.WriteEndElement();

            file.WriteStartElement("BindPose");
            for (int i = 0; i < BindPose.Count; i++)
            {
                file.WriteElementString("Bone", XmlExporter.MatrixToEntity(BindPose[i]));
            }
            file.WriteEndElement();

            file.WriteStartElement("InverseBindPose");
            for (int i = 0; i < InverseBindPose.Count; i++)
            {
                file.WriteElementString("Bone", XmlExporter.MatrixToEntity(InverseBindPose[i]));
            }
            file.WriteEndElement();

            file.WriteStartElement("SkeletonHierarchy");
            for (int i = 0; i < SkeletonHierarchy.Count; i++)
            {
                file.WriteStartElement("BoneData");
                SkeletonHierarchy[i].ToXML(file);
                file.WriteEndElement();
            }
            file.WriteEndElement();

            if (HeadSkeletonHierarchy.Count > 0)
            {
                file.WriteStartElement("HeadSkeletonHierarchy");
                for (int i = 0; i < HeadSkeletonHierarchy.Count; i++)
                {
                    file.WriteStartElement("BoneData");
                    HeadSkeletonHierarchy[i].ToXML(file);
                    file.WriteEndElement();
                }
                file.WriteEndElement();
            }
        }


        /// <summary>
        /// Private constructor for use by the XNB deserializer.
        /// </summary>
        private SkinningData()
        {
        }

        /// <summary>
        /// Gets a collection of animation clips. These are stored by name in a
        /// dictionary, so there could for instance be clips for "Walk", "Run",
        /// "JumpReallyHigh", etc.
        /// </summary>
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


        /// <summary>
        /// Bindpose matrices for each bone in the skeleton,
        /// relative to the parent bone.
        /// </summary>
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


        /// <summary>
        /// Vertex to bonespace transforms for each bone in the skeleton.
        /// </summary>
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        /// <summary>
        /// For each bone in the skeleton, stores the index of the parent bone.
        /// </summary>
        [ContentSerializer]
        //public List<int> SkeletonHierarchy { get; private set; }
        public List<BoneData> SkeletonHierarchy { get; private set; }

        [ContentSerializer]
        public List<BoneData> HeadSkeletonHierarchy { get; private set; }
    }

    public class BoneData
    {
        public enum SpecialBoneType { Head }

        [ContentSerializer]
        private int _ref;
        [ContentSerializer]
        private SpecialBoneType? _type;

        public SpecialBoneType? Type { get { return _type; } }

        public BoneData(int num)
        {
            _ref = num;
            _type = null;
        }

        public BoneData(int num, SpecialBoneType type)
        {
            _ref = num;
            _type = type;
        }

        public BoneData() { }

        public void ToXML(XmlWriter file)
        {
            file.WriteElementString("Ref", _ref.ToString());
            file.WriteElementString("Type", _type.ToString());
        }

        static public implicit operator int(BoneData data)
        {
            return data._ref;
        }

        static public implicit operator SpecialBoneType?(BoneData data)
        {
            return data._type;
        }
    }
}
