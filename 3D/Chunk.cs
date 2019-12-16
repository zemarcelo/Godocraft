using System;
using System.Collections;
using Godot;

public class Chunk 
{
    public Block[,,] chunkData;
    public SpatialMaterial material;
    public Spatial chunk;
    public string Name;


    Random rnd = new Random();

    /*Testes 
    public string teste = "oi teste";
    public int testeint = 42;
    public int[] testearray = new int[] { 1, 2, 3 }; */

    void BuildChunk()
    {
        chunkData = new Block[Mundo.chunkSize, Mundo.chunkSize, Mundo.chunkSize];
        for (int z = 0; z < Mundo.chunkSize; z++)
            for (int y = 0; y < Mundo.chunkSize; y++)
                for (int x = 0; x < Mundo.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)(x + chunk.Translation.x);
                    int worldY = (int)(y + chunk.Translation.y);
                    int worldZ = (int)(z + chunk.Translation.z);
                    GD.Print(Utils.Draw3DStones(worldX, worldY, worldZ));
                    if (Utils.Draw3DStones(worldX, worldY, worldZ) < 0.45f)
                    {
                        chunkData[x, y, z] = new Block(Block.TipoDeBloco.AIR, pos, chunk, material, this, ref chunkData);

                    }
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.TipoDeBloco.STONE, pos, chunk, material, this, ref chunkData);
                    else if (worldY < Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.TipoDeBloco.DIRT, pos, chunk, material, this, ref chunkData);
                    else if (worldY == Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.TipoDeBloco.GRASS, pos, chunk, material, this, ref chunkData);
                    else
                        chunkData[x, y, z] = new Block(Block.TipoDeBloco.AIR, pos, chunk, material, this, ref chunkData);
                }
    }

    public void DrawChunk()
    {

        for (int z = 0; z < Mundo.chunkSize; z++)
            for(int y = 0; y < Mundo.chunkSize; y++)
                for(int x = 0; x < Mundo.chunkSize; x++)
                {

                    chunkData[x, y, z].Draw();
                }

        CombinaQuads();
    }

    public Chunk(Vector3 position, SpatialMaterial m)
    {
        chunk = new Spatial
        {
            Name = Mundo.MontaNomeDoChunk(position)
        };
        Name = chunk.Name;       
        chunk.Translation = position;
        material = m;
        BuildChunk();
    }

    void CombinaQuads()
    {
        MeshInstance cube = new MeshInstance
        {
            Name = "Chunk"
        };

        ArrayMesh cubeArray = new ArrayMesh();

        Godot.Collections.Array kids = chunk.GetChildren();
        Material[] materiais = new Material[kids.Count];

        int contador = 0;

        foreach (MeshInstance meshI in kids)
        {

            Vector3[] vertLocal = (Vector3[])meshI.Mesh.SurfaceGetArrays(0)[(int)ArrayMesh.ArrayType.Vertex];
            Vector3[] vertGlobal = new Vector3[vertLocal.Length];

            //convertendo as coordenads de locais para globais dos quads
            for (int i = 0; i < vertLocal.Length; i++)
            {
                vertGlobal[i].x = vertLocal[i].x + meshI.Translation.x;
                vertGlobal[i].y = vertLocal[i].y + meshI.Translation.y;
                vertGlobal[i].z = vertLocal[i].z + meshI.Translation.z;

            }

            ArrayMesh combineArray = new ArrayMesh();
            var arrays = new Godot.Collections.Array();
            arrays.Resize((int)ArrayMesh.ArrayType.Max);
            arrays[(int)ArrayMesh.ArrayType.Vertex] = vertGlobal;
            arrays[(int)ArrayMesh.ArrayType.Normal] = meshI.Mesh.SurfaceGetArrays(0)[(int)ArrayMesh.ArrayType.Normal];
            arrays[(int)ArrayMesh.ArrayType.TexUv] = meshI.Mesh.SurfaceGetArrays(0)[(int)ArrayMesh.ArrayType.TexUv];
            arrays[(int)ArrayMesh.ArrayType.Index] = meshI.Mesh.SurfaceGetArrays(0)[(int)ArrayMesh.ArrayType.Index];

            cubeArray.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
            materiais[contador] = meshI.GetSurfaceMaterial(0);

            meshI.QueueFree();
            contador++;

        }

        cube.Mesh = cubeArray;
        for (int i = 0; i < materiais.Length; i++) cube.SetSurfaceMaterial(i, materiais[i]);

        chunk.AddChild(cube);

    }
    
}