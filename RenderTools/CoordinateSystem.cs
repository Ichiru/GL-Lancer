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

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RenderTools
{
    public class CoordinateSystem
    {
        private const int AXIS_VERTEX_COUNT = 6;

        private Camera camera;
        private ContentManager content;
        private SpriteBatch spriteBatch;

        private Vector3 axisMin, axisMax, axisStep;
        private Color xColor, yColor, zColor;

        private BasicEffect effect;
        private SpriteFont uiFont;

        private VertexBuffer axis;
        private VertexBuffer grid;

        public bool Show { get; set; }
        public bool ShowGrid { get; set; }

        public CoordinateSystem(Camera camera, ContentManager content)
        {
            this.camera = camera;
            this.content = content;

            xColor = Color.Red;
            yColor = Color.Green;
            zColor = Color.Blue;
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(camera.GraphicsDevice);

            effect = new BasicEffect(camera.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.VertexColorEnabled = true;

            uiFont = content.Load<SpriteFont>("fonts/AgencyFB");

            SetUpVertices(-Vector3.One, Vector3.One, new Vector3(.1f));
        }

        public void SetUpVertices(Vector3 axisMin, Vector3 axisMax, Vector3 axisStep)
        {
            this.axisMin = axisMin;
            this.axisMax = axisMax;
            this.axisStep = axisStep;

            VertexPositionColor[] axisVertices = new VertexPositionColor[AXIS_VERTEX_COUNT];
            axisVertices[0] = new VertexPositionColor(new Vector3(axisMin.X, 0, 0), xColor);
            axisVertices[1] = new VertexPositionColor(new Vector3(axisMax.X, 0, 0), xColor);
            axisVertices[2] = new VertexPositionColor(new Vector3(0, axisMin.Y, 0), yColor);
            axisVertices[3] = new VertexPositionColor(new Vector3(0, axisMax.Y, 0), yColor);
            axisVertices[4] = new VertexPositionColor(new Vector3(0, 0, axisMin.Z), zColor);
            axisVertices[5] = new VertexPositionColor(new Vector3(0, 0, axisMax.Z), zColor);
            axis = new VertexBuffer(camera.GraphicsDevice, VertexPositionColor.VertexDeclaration, AXIS_VERTEX_COUNT, BufferUsage.WriteOnly);
            axis.SetData<VertexPositionColor>(axisVertices);

            List<VertexPositionColor> gridVertices = new List<VertexPositionColor>();
            for (float x = axisMin.X; x <= axisMax.X; x += axisStep.X)
            {
                for (float y = axisMin.Y; y <= axisMax.Y; y += axisStep.Y)
                {
                    gridVertices.Add(new VertexPositionColor(new Vector3(x, y, axisMin.Z), zColor));
                    gridVertices.Add(new VertexPositionColor(new Vector3(x, y, axisMax.Z), zColor));
                }
                for (float z = axisMin.Z; z <= axisMax.Z; z += axisStep.Z)
                {
                    gridVertices.Add(new VertexPositionColor(new Vector3(x, axisMin.Y, z), yColor));
                    gridVertices.Add(new VertexPositionColor(new Vector3(x, axisMax.Y, z), yColor));
                }
            }
            for (float y = axisMin.Y; y <= axisMax.Y; y += axisStep.Y)
            {
                for (float z = axisMin.Z; z <= axisMax.Z; z += axisStep.Z)
                {
                    gridVertices.Add(new VertexPositionColor(new Vector3(axisMin.X, y, z), xColor));
                    gridVertices.Add(new VertexPositionColor(new Vector3(axisMax.X, y, z), xColor));
                }
            }
            grid = new VertexBuffer(camera.GraphicsDevice, VertexPositionColor.VertexDeclaration, gridVertices.Count, BufferUsage.WriteOnly);
            grid.SetData<VertexPositionColor>(gridVertices.ToArray());
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        public void Update()
        {
            effect.View = camera.View;
            effect.Projection = camera.Projection;
        }

        private void drawString(string s, Vector3 worldPosition, Color color, bool centerX, bool centerY)
        {
            Vector3 projected = camera.GraphicsDevice.Viewport.Project(worldPosition, camera.Projection, camera.View, Matrix.Identity);
            if (projected.Z < 1)
            {
                Vector2 size = uiFont.MeasureString(s);
                Vector2 screenPos = new Vector2(projected.X - (centerX ? size.X / 2 : 0), projected.Y - (centerY ? size.Y / 2 : 0));
                spriteBatch.DrawString(uiFont, s, screenPos, color);
            }
        }

        public void Draw()
        {
            if (Show)
            {
                camera.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                camera.GraphicsDevice.BlendState = BlendState.Opaque;

                effect.CurrentTechnique.Passes[0].Apply();
                if (ShowGrid)
                {
                    camera.GraphicsDevice.SetVertexBuffer(grid);
                    camera.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, grid.VertexCount);
                }
                else
                {
                    camera.GraphicsDevice.SetVertexBuffer(axis);
                    camera.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, axis.VertexCount);
                }

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

                drawString("0", Vector3.Zero, Color.Black, true, false);
                drawString("x", new Vector3(axisMax.X + axisStep.X * 0.5f, 0, 0), xColor, true, false);
                drawString("y", new Vector3(0, axisMax.Y + axisStep.Y * 0.5f, 0), yColor, false, true);
                drawString("z", new Vector3(0, 0, axisMax.Z + axisStep.Z * 0.5f), zColor, true, false);

                for (float f = axisMin.X; f <= axisMax.X; f += axisStep.X)
                {
                    if (f != 0)
                    {
                        drawString(f.ToString(), new Vector3(f, 0, 0), xColor, true, false);
                    }
                }

                for (float f = axisMin.Y; f <= axisMax.Y; f += axisStep.Y)
                {
                    if (f != 0)
                    {
                        drawString(f.ToString(), new Vector3(0, f, 0), yColor, false, true);
                    }
                }

                for (float f = axisMin.Z; f <= axisMax.Z; f += axisStep.Z)
                {
                    if (f != 0)
                    {
                        drawString(f.ToString(), new Vector3(0, 0, f), zColor, true, false);
                    }
                }

                spriteBatch.End();
            }
        }
    }
}
