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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderTools
{
    public class Camera
    {
        private const float MOVE_SPEED = 1000;

        public GraphicsDevice GraphicsDevice { get; private set; }

        private Vector3 currentTarget, selectedTarget;
        public Vector3 Target
        {
            set { selectedTarget = value; }
        }

        public Vector2 Rotation { get; set; }
        public float Zoom { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 MoveVector { get; set; }

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        //public Plane ReflectionPlane { get; set; }
        //public Vector3 ReflectionPosition { get; private set; }
        //public Matrix ReflectionView { get; private set; }

        public bool Free { get; set; }

        public Camera(GraphicsDevice device)
        {
            this.GraphicsDevice = device;
        }

        public void UpdateProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(35f), GraphicsDevice.Viewport.AspectRatio, 0.3f, 100000000f);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        public void Update()
        {
            if (Free)
            {
                Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.Y) * Matrix.CreateRotationY(Rotation.X);

                Vector3 rotatedVector = Vector3.Transform(MoveVector, rotationMatrix);
                Position += MOVE_SPEED * rotatedVector;

                Vector3 originalTarget = Vector3.Forward;
                Vector3 rotatedTarget = Vector3.Transform(originalTarget, rotationMatrix);
                Vector3 target = Position + rotatedTarget;

                Vector3 upVector = Vector3.Transform(Vector3.Up, rotationMatrix);

                View = Matrix.CreateLookAt(Position, target, upVector);

                MoveVector = Vector3.Zero;
            }
            else
            {
                if (currentTarget != selectedTarget)
                {
                    Vector3 direction = selectedTarget - currentTarget;

                    if (direction.Length() >= MOVE_SPEED)
                    {
                        direction.Normalize();
                        currentTarget += direction * MOVE_SPEED;
                    }
                    else currentTarget = selectedTarget;
                }

                Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.Y) * Matrix.CreateRotationY(Rotation.X);

                Vector3 position = new Vector3(0, 0, Zoom);
                position = Vector3.Transform(position, rotationMatrix);
                Position = currentTarget + position;

                Vector3 upVector = Vector3.Transform(Vector3.Up, rotationMatrix);

                View = Matrix.CreateLookAt(Position, currentTarget, upVector);
            }

            // Reflection
            /*Matrix reflectionMatrix = Matrix.CreateReflection(ReflectionPlane);
            ReflectionPosition = Vector3.Transform(Position, reflectionMatrix);
            Vector3 rtar = Vector3.Transform(target, reflectionMatrix);
            Vector3 rup = Vector3.Cross(Vector3.Transform(Vector3.Right, rotationMatrix), rtar - ReflectionPosition);

            ReflectionView = Matrix.CreateLookAt(ReflectionPosition, rtar, rup);*/
        }
    }
}