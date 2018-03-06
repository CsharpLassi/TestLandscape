using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using engenious;
using engenious.Graphics;
using engenious.Helper;
using engenious.UserDefined;
using TestLandscape.Models.Grass;
using TestLandscape.Terrain.Generation;
using Color = engenious.Color;

namespace TestLandscape.Terrain
{
    public class Landscape : GameObject<Landscape>
    {   
        private bool isDirty = true;

        private VertexBuffer vb;
        private IndexBuffer ib;

        private TerrainEffect terrainEffect;

        public uint Side { get; private set; }
        
        public Vector3 Scaling { get; set; } = Vector3.One;

        private float[,] map;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetIndex(uint x, uint y)
        {
            x -= 1;
            y -= 1;
            return x * (Side -2) + y;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetHeight(uint x, uint y)
        {
            return (map[x, y] +1f) *0.5f;
        }

        protected override unsafe void OnLoad()
        {
            
            
            CreateComponent<TerrainComponent>();
            
            if (!isDirty)
                return;
            
            Scaling = new Vector3(1,1,1);

            var freq = 1 / 500f;
            
            var generator = new SimplexNoiseGenerator(1234,freq,freq,freq,freq);

            uint fac = 1;

            Side = 17;

            map = generator.GetTileableNoiseMap2D(0,0, (int)Side, (int)Side, (int)Side, (int)Side);
            
            terrainEffect = Simulation.Game.Content.Load<TerrainEffect>("TerrainEffect");
            
            


            var vertex = new VertexPositionNormalColor[(Side-2)*(Side-2)];
            var indexes = new uint[(Side-3)*(Side-3)*6];
            
            int count = 0;
            
            Parallel.For(1, Side - 1, (xs) =>
            {
                try
                {
                    uint x = (uint) xs;
                    uint index = (x - 1) * (Side - 2);
                    uint indicesIndex = (x - 1) * (Side - 3)*6;
                    for (uint y = 1; y < Side - 1; y++, index++)
                    {
                        var height = GetHeight(x, y);

                        Color color = new Color(height, height, height);
                        
                        
                        var vectors = stackalloc Vector3[6];
                        vectors[0] = new Vector3(x, y - 1, GetHeight(x, y - 1)) * Scaling;
                        vectors[1] = new Vector3(x - 1, y - 1, GetHeight(x - 1, y - 1)) * Scaling;
                        vectors[2] = new Vector3(x - 1, y, GetHeight(x - 1, y)) * Scaling;
                        vectors[3] = new Vector3(x, y + 1, GetHeight(x, y + 1)) * Scaling;
                        vectors[4] = new Vector3(x + 1, y + 1, GetHeight(x + 1, y + 1)) * Scaling;
                        vectors[5] = new Vector3(x + 1, y, GetHeight(x + 1, y)) * Scaling;

                        Vector3 normal = Vector3.Cross(vectors[0], vectors[5]);
                        for (int i = 1; i < 6; i++)
                            normal += Vector3.Cross(vectors[i], vectors[i - 1]);

                        normal.Normalize();

                        if (x < Side - 2 && y < Side - 2)
                        {
                            indexes[indicesIndex++] = GetIndex(x, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y + 1);

                            indexes[indicesIndex++] = GetIndex(x, y);
                            indexes[indicesIndex++] = GetIndex(x + 1, y + 1);
                            indexes[indicesIndex++] = GetIndex(x, y + 1);
                        }

                        
                        var halfSide = Side / 2;
                        
                        var nx = (int)x - (int)halfSide;
                        var ny = (int)y - (int) halfSide;

                        var phi = Math.Atan(ny /(double) nx);


                        Console.WriteLine($"{nx};{ny};{phi}");
                        
                        float fx = nx;
                        float fy = ny;
                        float fz =0;
                        
                        Vector3 coord = new Vector3(fx,fy,fz);
                        
                        vertex[index] = (new VertexPositionNormalColor(coord * Scaling, normal, color));
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });    
            
            
            
            vb = new VertexBuffer(Simulation.GraphicsDevice,typeof(VertexPositionNormalColor),vertex.Length);
            vb.SetData(vertex);

            ib = new IndexBuffer(Simulation.GraphicsDevice,DrawElementsType.UnsignedInt,indexes.Length);
            ib.SetData(indexes);
             
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
            Simulation.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            Simulation.GraphicsDevice.VertexBuffer = vb;
            Simulation.GraphicsDevice.IndexBuffer = ib;

            terrainEffect.Main.Pass1.Apply();
            terrainEffect.Main.Pass1.World = world;
            terrainEffect.Main.Pass1.View = view;
            terrainEffect.Main.Pass1.Proj = proj;
            terrainEffect.Main.Pass1.shadowViewProj = shadowViewProj;
            terrainEffect.Main.Pass1.ShadowMap = shadowMap;
            
            terrainEffect.Main.Pass1.AmbientColor = ambientColor;
            terrainEffect.Main.Pass1.DiffuseColor = diffuseColor;
            terrainEffect.Main.Pass1.DiffuseDirection = diffuseDirection;
            Simulation.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles,0,0,vb.VertexCount,0,ib.IndexCount/3);
        }

        
        private void DrawShadow(Matrix world, Matrix view, Matrix proj)
        {
            Simulation.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            Simulation.GraphicsDevice.VertexBuffer = vb;
            Simulation.GraphicsDevice.IndexBuffer = ib;
            
            terrainEffect.Shadow.Pass1.Apply();
            terrainEffect.Shadow.Pass1.Proj = proj;
            terrainEffect.Shadow.Pass1.View = view;
            terrainEffect.Shadow.Pass1.World = world;
            
            Simulation.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles,0,0,vb.VertexCount,0,ib.IndexCount/3);
        }
        


    }
}