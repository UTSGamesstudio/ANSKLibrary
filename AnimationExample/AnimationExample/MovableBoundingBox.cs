using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectSneakyGame
{
    public class MovableBoundingBox
    {
        private Vector3 _originalMin, _originalMax;
        private BoundingBox _bBox;
        private float _left, _right, _top, _bottom, _near, _far;
        private float _radiusX, _radiusY, _radiusZ;
        private Vector3 _midPoint;

        public BoundingBox BBox { get { return _bBox; } set { _bBox = value; } }
        public float Left { get { return _left; } }
        public float Right { get { return _right; } }
        public float Top { get { return _top; } }
        public float Bottom { get { return _bottom; } }
        public float Near { get { return _near; } }
        public float Far { get { return _far; } }
        public float RadiusX { get { return _radiusX; } }
        public float RadiusY { get { return _radiusY; } }
        public float RadiusZ { get { return _radiusZ; } }
        public Vector3 MidPoint { get { return _midPoint; } }

        public MovableBoundingBox(MovableBoundingBox mbbox) : this(mbbox._bBox) { }

        public MovableBoundingBox(BoundingBox box)
        {
            _originalMax = box.Max;
            _originalMin = box.Min;
            _bBox = box;
            SetEdges(_originalMax, _originalMin);
            CalculateRadius();
            CalculateMidPoint();
        }

        public void Transform(Vector3 trans)
        {
            _bBox.Max = _originalMax + trans;
            _bBox.Min = _originalMin + trans;
            SetEdges(_bBox.Max, _bBox.Min);
            CalculateRadius();
            CalculateMidPoint();
        }

        public void TranslateX(float amount)
        {
            _bBox.Max.X += amount;
            _bBox.Min.X += amount;
            SetEdges(_bBox.Max, _bBox.Min);
            CalculateRadius();
            CalculateMidPoint();
        }

        public void TranslateY(float amount)
        {
            _bBox.Max.Y += amount;
            _bBox.Min.Y += amount;
            SetEdges(_bBox.Max, _bBox.Min);
            CalculateRadius();
            CalculateMidPoint();
        }

        public void TranslateZ(float amount)
        {
            _bBox.Max.Z += amount;
            _bBox.Min.Z += amount;
            SetEdges(_bBox.Max, _bBox.Min);
        }

        private void SetEdges(Vector3 max, Vector3 min)
        {
            _left = min.X;
            _right = max.X;
            _top = max.Y;
            _bottom = min.Y;
            _near = min.Z;
            _far = max.Z;
        }

        private void CalculateRadius()
        {
            _radiusX = (_left - _right) * 0.5f;
            _radiusY = (_bottom - _top) * 0.5f;
            _radiusZ = (_near - _far) * 0.5f;
            _radiusX = Math.Abs(_radiusX);
            _radiusY = Math.Abs(_radiusY);
            _radiusZ = Math.Abs(_radiusZ);
        }

        private void CalculateMidPoint()
        {
            _midPoint = new Vector3(_bBox.Min.X + _radiusX, _bBox.Min.Y + _radiusY, _bBox.Min.Z + _radiusZ);
        }

        public void ChangeMin(Vector3 min)
        {
            _bBox.Min = min;
            ReScanBoundingBox();
        }

        public void ChangeMax(Vector3 max)
        {
            _bBox.Max = max;
            ReScanBoundingBox();
        }

        public void ReScanBoundingBox()
        {
            SetEdges(_bBox.Max, _bBox.Min);
            CalculateRadius();
            CalculateMidPoint();
        }

        public void ResetToOriginalValues()
        {
            SetEdges(_originalMax, _originalMin);
            CalculateRadius();
            CalculateMidPoint();
        }

        public bool IsFullyWithinBounds(MovableBoundingBox box2)
        {
            if (box2.Left < _left)
                return false;
            else if (box2.Right > _right)
                return false;
            else if (box2.Top > _top)
                return false;
            else if (box2.Bottom < _bottom)
                return false;
            else if (box2.Near < _near)
                return false;
            else if (box2.Far > _far)
                return false;
            else
                return true;
        }

        public bool IsPartiallyWithinBounds(MovableBoundingBox box2)
        {
            bool isIn, isOut;
            isIn = isOut = false;

            if (box2.Left < _left || box2.Left > _right)
                isOut = true;
            else
                isIn = true;
            if (box2.Right > _right || box2.Right < _left)
                isOut = true;
            else
                isIn = true;
            if (box2.Top > _top || box2.Top < _bottom)
                isOut = true;
            else
                isIn = true;
            if (box2.Bottom < _bottom || box2.Bottom > _top)
                isOut = true;
            else
                isIn = true;
            if (box2.Near < _near || box2.Near > _far)
                isOut = true;
            else
                isIn = true;
            if (box2.Far > _far || box2.Far < _near)
                isOut = true;
            else
                isIn = true;

            if (!_bBox.Intersects(box2))
                return false;

            if (isIn && isOut)
                return true;
            else if (isIn)
                return true;
            else
                return false;
        }

        static public implicit operator BoundingBox(MovableBoundingBox mBox)
        {
            return mBox._bBox;
        }
    }
}
