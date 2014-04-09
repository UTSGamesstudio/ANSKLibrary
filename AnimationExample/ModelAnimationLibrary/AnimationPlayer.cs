#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace ModelAnimationLibrary
{
    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // Information about the currently playing animation clip.
        private AnimationClip currentClipValue;
        private TimeSpan currentTimeValue;
        private int currentKeyframe;


        // Current animation transform matrices.
        private Matrix[] boneTransforms;
        private Matrix[] worldTransforms;
        private Matrix[] skinTransforms;
        private Matrix[] headTransforms;


        // Backlink to the bind pose and skeleton hierarchy data.
        private SkinningData skinningDataValue;

        private bool _isPaused;
        private float _speed;
        private bool _looped;
        private bool _static;
        private bool _endOfClip;

        public bool IsPaused { get { return _isPaused; } }
        public float Speed { get { return _speed; } set { if (value > currentClipValue.Duration.Ticks) _speed = currentClipValue.Duration.Ticks; else _speed = value; } }
        public bool Looped { get { return _looped; } set { _looped = value; } }
        public bool Static { get { return _static; } set { _static = value; } }
        public bool EndOfClip { get { return _endOfClip; } }

        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
            headTransforms = new Matrix[skinningData.HeadSkeletonHierarchy.Count];
            _isPaused = false;
            _speed = 0;
            _looped = true;
            _static = false;
            _endOfClip = false;
        }


        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;
            _isPaused = false;
            _looped = true;
            _endOfClip = false;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        public void StartClip(AnimationClip clip, bool looped)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;
            _isPaused = false;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            _looped = looped;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            //if (!_isPaused)
            //{
                UpdateBoneTransforms(time, relativeToCurrentTime);
                UpdateWorldTransforms(rootTransform);
                UpdateSkinTransforms();
            //}
        }


        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (_isPaused)
                return;

            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // If we reached the end, loop back to the start.
                if (time >= currentClipValue.Duration)
                {
                    if (!_looped)
                    {
                        _endOfClip = true;
                        return;
                    }
                    if (!_looped)
                    {
                        _isPaused = true;
                        //while (time >= currentClipValue.Duration)
                            //time -= currentClipValue.Duration;
                    }
                    /*else
                        return;*/
                }

                if (!_static && _looped)
                {
                    while (time >= currentClipValue.Duration)
                        time -= currentClipValue.Duration;
                }
            }

            if (!_static)
            {
                if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                    throw new ArgumentOutOfRangeException("time");
            }

            // If the position moved backwards, reset the keyframe index.
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            TimeSpan temp = new TimeSpan(time.Ticks + (long)_speed);

            currentTimeValue = temp;
            //currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTimeValue)
                    break;

                // Use this keyframe.
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            /*if (_static)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[0];

                worldTransforms[0] = boneTransforms[0] *
                                             worldTransforms[0];
            }
            else*/
            //{
            //headTransforms = new Matrix[headTransforms.Length];
            int headNum = 0;

            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                BoneData parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                                worldTransforms[parentBone];

                if (parentBone.Type == BoneData.SpecialBoneType.Head)
                {
                    headTransforms[headNum] = boneTransforms[bone] *
                                                worldTransforms[parentBone];
                    headNum++;
                }
            }
            //}

            // Then update the world tranforms of the head bone transforms
            /*if (headTransforms.Length > 0)
            {
                headTransforms[0] = boneTransforms[0] * rootTransform;

                for (int i = 1; i < headTransforms.Length; i++)
                {
                    BoneData parentBone = skinningDataValue.HeadSkeletonHierarchy[i];

                    headTransforms[i] = headTransforms[i] * worldTransforms[parentBone];
                }
            }*/
        }


        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }

        public Matrix GetParentTransform()
        {
            return worldTransforms[0];
        }

        public Matrix[] GetHeadWorldTransforms()
        {
            return headTransforms;
        }

        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
