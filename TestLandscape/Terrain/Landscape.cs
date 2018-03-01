﻿using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using engenious;
using engenious.Graphics;
using engenious.UserDefined;
using TestLandscape.Models.Grass;
using Color = engenious.Color;

namespace TestLandscape.Terrain
{
    public class Landscape : GameObject<Landscape>
    {   
        private bool isDirty = true;

        private VertexBuffer vb;
        private IndexBuffer ib;

        private TerrainEffect terrainEffect;

        public uint Height { get; private set; }
        public uint Width { get; private set; }
        public ColorByte[] HeightMap { get; private set; }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetIndex(uint x, uint y)
        {
            x -= 1;
            y -= 1;
            return x * (Height -2) + y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetHeight(uint x, uint y)
        {
            return (HeightMap[y * Width + x].R / 255.0f - 0.5f);
        }

        protected override unsafe void OnLoad()
        {
            Random r = new Random(1024);
            
            CreateComponent<TerrainComponent>();
            
            if (!isDirty)
                return;
            
            terrainEffect = Manager.Load<TerrainEffect>("TerrainEffect");
            
            var bitmap = (Bitmap)Bitmap.FromFile("Content/maps/map.png");
            var heightTexture = Texture2D.FromBitmap(GraphicsDevice,bitmap);
            var heightmap = new ColorByte[heightTexture.Width * heightTexture.Height];
            heightTexture.GetData(heightmap);
            HeightMap = heightmap;
            
            Height = (uint)bitmap.Height;
            Width = (uint)bitmap.Width;

            var vertex = new VertexPositionNormalColor[(Width-2)*(Height-2)];
            var indexes = new uint[(Width-3)*(Height-3)*6];
            
            //for (uint x = 1; x < Width-1; x++)
            Parallel.For(1, Width - 1, (xs) =>
            {
                try
                {
                    uint x = (uint) xs;
                    uint index = (x - 1) * (Height - 2);
                    uint indicesIndex = (x - 1) * (Height - 3)*6;
                    for (uint y = 1; y < Height - 1; y++, index++)
                    {
                        var height = GetHeight(x, y);

                        Color color = Color.SandyBrown;
                        if (height > 0)
                            color = Color.SpringGreen;
                        
                        var vectors = stackalloc Vector3[6];
                        vectors[0] = new Vector3(x, y - 1, GetHeight(x, y - 1));
                        vectors[1] = new Vector3(x - 1, y - 1, GetHeight(x - 1, y - 1));
                        vectors[2] = new Vector3(x - 1, y, GetHeight(x - 1, y));
                        vectors[3] = new Vector3(x, y + 1, GetHeight(x, y + 1));
                        vectors[4] = new Vector3(x + 1, y + 1, GetHeight(x + 1, y + 1));
                        vectors[5] = new Vector3(x + 1, y, GetHeight(x + 1, y));

                        Vector3 normal = Vector3.Cross(vectors[0], vectors[5]);
                        for (int i = 1; i < 6; i++)
                            normal += Vector3.Cross(vectors[i], vectors[i - 1]);

                        normal.Normalize();

                        if (x < bitmap.Width - 2 && y < bitmap.Height - 2)
                        {
                            indexes[indicesIndex++] = GetIndex(x, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y + 1);

                            indexes[indicesIndex++] = GetIndex(x, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y + 1);
                            indexes[indicesIndex++] = GetIndex(x, y + 1);
                        }


                        vertex[index] = (new VertexPositionNormalColor(new Vector3(x, y, height), normal, color));
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });    
            
            
            
            vb = new VertexBuffer(GraphicsDevice,typeof(VertexPositionNormalColor),vertex.Length);
            vb.SetData(vertex);

            ib = new IndexBuffer(GraphicsDevice,DrawElementsType.UnsignedInt,indexes.Length);
            ib.SetData(indexes);
            
            Grass1ModelObject.Create(this, new Vector3(0, 0, 0));

            int count = 0;
            
            Parallel.For(1, Width - 1, (xs) =>
            {
                try
                {
                    uint x = (uint) xs;
                    uint index = (x - 1) * (Height - 2);
                    uint indicesIndex = (x - 1) * (Height - 3)*6;
                    for (uint y = 1; y < Height - 1; y++, index++)
                    {
                        var height = GetHeight(x, y);

                        if (height > 0 && r.Next(2) == 0 && Interlocked.Increment(ref count) <10000)
                        {
                            Grass1ModelObject.Create(this, new Vector3(x, y, height));
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });    
            
            isDirty = false;
        }

        public void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun,
            Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            if (pass == RenderPass.Shadow)
            {
                DrawShadow(world,camera.View,camera.Projection);
            }
            else if (pass == RenderPass.Normal)
            {
                DrawNormal(world,camera.View,camera.Projection,
                    shadowProjView,shadowMap,sun.AmbientColor,sun.DiffuseColor,sun.DiffuseDirection );
            }
            
            
        }
        
        
        private void DrawNormal(Matrix world, Matrix view, Matrix proj
            , Matrix shadowViewProj
            , RenderTarget2D shadowMap
            ,Color ambientColor,Color diffuseColor,Vector3 diffuseDirection)
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            GraphicsDevice.VertexBuffer = vb;
            GraphicsDevice.IndexBuffer = ib;

            terrainEffect.Main.Pass1.Apply();
            terrainEffect.Main.Pass1.World = world;
            terrainEffect.Main.Pass1.View = view;
            terrainEffect.Main.Pass1.Proj = proj;
            terrainEffect.Main.Pass1.shadowViewProj = shadowViewProj;
            terrainEffect.Main.Pass1.ShadowMap = shadowMap;
            
            terrainEffect.Main.Pass1.AmbientColor = ambientColor;
            terrainEffect.Main.Pass1.DiffuseColor = diffuseColor;
            terrainEffect.Main.Pass1.DiffuseDirection = diffuseDirection;
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles,0,0,vb.VertexCount,0,ib.IndexCount/3);
        }

        
        private void DrawShadow(Matrix world, Matrix view, Matrix proj)
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.VertexBuffer = vb;
            GraphicsDevice.IndexBuffer = ib;
            
            terrainEffect.Shadow.Pass1.Apply();
            terrainEffect.Shadow.Pass1.Proj = proj;
            terrainEffect.Shadow.Pass1.View = view;
            terrainEffect.Shadow.Pass1.World = world;
            
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles,0,0,vb.VertexCount,0,ib.IndexCount/3);
        }
        


    }
}