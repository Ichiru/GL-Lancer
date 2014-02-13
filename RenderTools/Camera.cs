/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is RenderTools code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using OpenTK;
using FLCommon;

namespace RenderTools
{
    public class Camera
    {
        private const float MOVE_SPEED = 400;

        public Viewport Viewport { get; private set; }

        private Vector3 currentTarget, selectedTarget;
        public Vector3 Target
        {
            set { selectedTarget = value; }
        }

        public Vector2 Rotation { get; set; }
        public float Zoom { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 MoveVector { get; set; }

        public Matrix4 Projection { get; private set; }
        public Matrix4 View { get; private set; }

        //public Plane ReflectionPlane { get; set; }
        //public Vector3 ReflectionPosition { get; private set; }
        //public Matrix4 ReflectionView { get; private set; }

        public bool Free { get; set; }

        public Camera(Viewport viewport)
        {
            this.Viewport = viewport;
        }

        public void UpdateProjection()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathConvert.ToRadians(35f), Viewport.AspectRatio, 0.3f, 100000000f);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        public void Update()
        {
            if (Free)
            {
                Matrix4 rotationMatrix4 = Matrix4.CreateRotationX(Rotation.Y) * Matrix4.CreateRotationY(Rotation.X);

                Vector3 rotatedVector = Vector3.Transform(MoveVector, rotationMatrix4);
                Position += MOVE_SPEED * rotatedVector;

                Vector3 originalTarget = VectorMath.Forward;
                Vector3 rotatedTarget = Vector3.Transform(originalTarget, rotationMatrix4);
                Vector3 target = Position + rotatedTarget;

                Vector3 upVector = Vector3.Transform(VectorMath.Up, rotationMatrix4);

                View = Matrix4.LookAt(Position, target, upVector);

                MoveVector = Vector3.Zero;
            }
            else
            {
                if (currentTarget != selectedTarget)
                {
                    Vector3 direction = selectedTarget - currentTarget;

                    if (direction.Length >= MOVE_SPEED)
                    {
                        direction.Normalize();
                        currentTarget += direction * MOVE_SPEED;
                    }
                    else currentTarget = selectedTarget;
                }

                Matrix4 rotationMatrix4 = Matrix4.CreateRotationX(Rotation.Y) * Matrix4.CreateRotationY(Rotation.X);

                Vector3 position = new Vector3(0, 0, Zoom);
                position = Vector3.Transform(position, rotationMatrix4);
                Position = currentTarget + position;

                Vector3 upVector = Vector3.Transform(VectorMath.Up, rotationMatrix4);

                View = Matrix4.LookAt(Position, currentTarget, upVector);
            }

            // Reflection
            /*Matrix4 reflectionMatrix4 = Matrix4.CreateReflection(ReflectionPlane);
            ReflectionPosition = Vector3.Transform(Position, reflectionMatrix4);
            Vector3 rtar = Vector3.Transform(target, reflectionMatrix4);
            Vector3 rup = Vector3.Cross(Vector3.Transform(Vector3.Right, rotationMatrix4), rtar - ReflectionPosition);

            ReflectionView = Matrix4.CreateLookAt(ReflectionPosition, rtar, rup);*/
        }
    }
}