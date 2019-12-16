using Godot;
using System;


public class Cubo : Spatial
{

    Texture texturaDoQuad;

    enum LadoDoCubo { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    public enum TipoDeBloco { GRASS, DIRT, STONE, TESTE };

    public SpatialMaterial quadMaterial = new SpatialMaterial();

    [Export] public TipoDeBloco tBloco; // = TipoDeBloco.GRASS;

    readonly Vector2[,] blocosUVs =
    {
        /* GRASS TOP   */   {new Vector2( 0.1250f, 0.6250f ), new Vector2( 0.1875f, 0.6250f ),
                             new Vector2( 0.1250f, 0.5625f ), new Vector2( 0.1875f, 0.5625f ) },
        /* GRASS SIDE  */   {new Vector2( 0.1875f, 0.0625f ), new Vector2( 0.2500f, 0.0625f ),
                             new Vector2( 0.1875f, 0.0000f ), new Vector2( 0.2500f, 0.0000f ) },

        /* DIRT        */   {new Vector2( 0.1250f, 0.0000f ), new Vector2( 0.1875f, 0.0000f ),
                             new Vector2( 0.1250f, 0.0625f ), new Vector2( 0.1875f, 0.0625f ) }, 

        /* STONE       */   {new Vector2( 0.0000f, 0.1250f ), new Vector2( 0.0625f, 0.1250f ),
                             new Vector2( 0.0000f, 0.0625f ), new Vector2( 0.0625f, 0.0625f ) },
                                  
        /* TESTE       */   {new Vector2( 0.1250f, 0.0000f ), new Vector2( 0.1875f, 0.0000f ),
                             new Vector2( 0.1250f, 0.0625f ), new Vector2( 0.1875f, 0.0625f )


                             }

    };

    public override void _Ready()
    {

        CriaCubo();

    }


    void CriaQuad(LadoDoCubo lado)
    {

        ArrayMesh quadArray;
        MeshInstance quad = new MeshInstance
        {
            Name = "Quad"
        };

        //Criando os Arrays
        Vector3[] normalArray = new Vector3[4];
        Vector2[] uvArray = new Vector2[4];
        Vector3[] vertexArray = new Vector3[4];
        int[] indexArray = new int[6];

        //Prepara o Material do Cubo
        texturaDoQuad = (Texture)GD.Load("res://imagens/blockatlas.png");
        quadMaterial.AlbedoTexture = texturaDoQuad;


        //calculando UVs no atlas
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11; 

        if(tBloco == TipoDeBloco.GRASS && lado == LadoDoCubo.TOP)
        {
            uv00 = blocosUVs[0, 0];
            uv10 = blocosUVs[0, 1];
            uv01 = blocosUVs[0, 2];
            uv11 = blocosUVs[0, 3];
        }
        else if (tBloco == TipoDeBloco.GRASS && lado == LadoDoCubo.BOTTOM)
        {
            uv00 = blocosUVs[(int)(TipoDeBloco.DIRT + 1), 0];
            uv10 = blocosUVs[(int)(TipoDeBloco.DIRT + 1), 1];
            uv01 = blocosUVs[(int)(TipoDeBloco.DIRT + 1), 2];
            uv11 = blocosUVs[(int)(TipoDeBloco.DIRT + 1), 3];
        }
        else
        {
            uv00 = blocosUVs[(int)(tBloco + 1), 0];
            uv10 = blocosUVs[(int)(tBloco + 1), 1];
            uv01 = blocosUVs[(int)(tBloco + 1), 2];
            uv11 = blocosUVs[(int)(tBloco + 1), 3];
        }


        //Todos os Vertices Possiveis
        Vector3 p0 = new Vector3(-0.5f, -0.5f,  0.5f);
        Vector3 p1 = new Vector3( 0.5f, -0.5f,  0.5f);
        Vector3 p2 = new Vector3( 0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f,  0.5f,  0.5f);
        Vector3 p5 = new Vector3( 0.5f,  0.5f,  0.5f);
        Vector3 p6 = new Vector3( 0.5f,  0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f,  0.5f, -0.5f);

        switch (lado)
        {
            case LadoDoCubo.BOTTOM:
                vertexArray = new Vector3[] { p0, p1, p2, p3 };
                normalArray = new Vector3[] { Vector3.Down, Vector3.Down, Vector3.Down, Vector3.Down };
                break;
            case LadoDoCubo.TOP:
                vertexArray = new Vector3[] { p7, p6, p5, p4 };
                normalArray = new Vector3[] { Vector3.Up, Vector3.Up, Vector3.Up, Vector3.Up };
                break;
            case LadoDoCubo.LEFT:
                vertexArray = new Vector3[] { p7, p4, p0, p3 };
                normalArray = new Vector3[] { Vector3.Left, Vector3.Left, Vector3.Left, Vector3.Left };
                break;
            case LadoDoCubo.RIGHT:
                vertexArray = new Vector3[] { p5, p6, p2, p1 };
                normalArray = new Vector3[] { Vector3.Right, Vector3.Right, Vector3.Right, Vector3.Right };
                break;
            case LadoDoCubo.FRONT:
                vertexArray = new Vector3[] { p4, p5, p1, p0 };
                normalArray = new Vector3[] { Vector3.Forward, Vector3.Forward, Vector3.Forward, Vector3.Forward };
                break;
            case LadoDoCubo.BACK:
                vertexArray = new Vector3[] { p6, p7, p3, p2 };
                normalArray = new Vector3[] { Vector3.Back, Vector3.Back, Vector3.Back, Vector3.Back };
                break;
        }

        uvArray = new Vector2[] { uv11, uv01, uv00, uv10};
        indexArray = new int[] { 0, 1, 2, 0, 2, 3 };


        quadArray = new ArrayMesh();
        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)ArrayMesh.ArrayType.Max);
        arrays[(int)ArrayMesh.ArrayType.Vertex] = vertexArray;
        arrays[(int)ArrayMesh.ArrayType.Normal] = normalArray;
        arrays[(int)ArrayMesh.ArrayType.TexUv] = uvArray;
        arrays[(int)ArrayMesh.ArrayType.Index] = indexArray;

        quadArray.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);



        quad.Mesh = quadArray;
        quad.SetSurfaceMaterial(0, quadMaterial);

        AddChild(quad);
    }

    void CombinaQuads(String nome)
    {
        MeshInstance cube = new MeshInstance
        {
            Name = nome
        };  

        ArrayMesh cubeArray = new ArrayMesh();
        
        Godot.Collections.Array kids = GetChildren();
        Material[] materiais = new Material[kids.Count];

        int contador = 0;

        foreach (MeshInstance meshI in kids)
        {

            cubeArray.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, meshI.Mesh.SurfaceGetArrays(0));
            materiais[contador] = meshI.GetSurfaceMaterial(0);

            meshI.QueueFree();
            contador++;

        }

        cube.Mesh = cubeArray;
        for (int i = 0; i < materiais.Length; i++) cube.SetSurfaceMaterial(i, materiais[i]);

        AddChild(cube);

    }

    public void CriaCubo()
    {
        CriaQuad(LadoDoCubo.FRONT);
        CriaQuad(LadoDoCubo.BACK);
        CriaQuad(LadoDoCubo.TOP);
        CriaQuad(LadoDoCubo.BOTTOM);
        CriaQuad(LadoDoCubo.LEFT);
        CriaQuad(LadoDoCubo.RIGHT);
        CombinaQuads("Cubo");

    }

}



