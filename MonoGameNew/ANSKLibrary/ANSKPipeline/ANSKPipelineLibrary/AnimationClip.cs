#region File Description
//-----------------------------------------------------------------------------
// AnimationClip.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
#endregion

namespace ANSKPipelineLibrary
{
    /// <summary>
    /// An animation clip is the runtime equivalent of the
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent type.
    /// It holds all the keyframes needed to describe a single animation.
    /// </summary>
    public class AnimationClip
    {
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
            Name = null;
        }

        /// <summary>
        /// Constructs a new animation clip object.
        /// </summary>
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes, string animName)
        {
            Duration = duration;
            Keyframes = keyframes;
            Name = animName;
        }

        public void ExportToXML(XmlWriter file)
        {
            file.WriteElementString("Name", Name);
            file.WriteElementString("Timespan", Duration.Ticks.ToString());

            file.WriteStartElement("Keyframes");
            for (int i = 0; i < Keyframes.Count; i++)
            {
                file.WriteStartElement("Keyframe");
                Keyframes[i].ExportToXML(file);
                file.WriteEndElement();
            }
            file.WriteEndElement();
        }


        /// <summary>
        /// Private constructor for use by the XNB deserializer.
        /// </summary>
        private AnimationClip()
        {
        }


        /// <summary>
        /// Gets the total length of the animation.
        /// </summary>
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


        /// <summary>
        /// Gets a combined list containing all the keyframes for all bones,
        /// sorted by time.
        /// </summary>
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }

        [ContentSerializer]
        public string Name { get; private set; }
    }
}
