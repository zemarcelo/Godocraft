/* Mundo: Arquivo que instancia o mundo de cubos.
 * Ele determina a quantidade chunks, grupos de cubos, e a altura da
 * coluna de cubos.
 */
using Godot;
using System;
using System.Collections.Generic;

public class Mundo : Spatial
{
    // Declaração das variáveis principais
    [Export] int tamanhoDoChunk = 4;
    [Export] int altura = 2;
    [Export] int tamamanhoDoMundo = 2;
    public static int chunkSize;                //quantidade de cubos por chunk
    public static int worldSize;
    public static int alturaDaColuna;           //altura da coluna de chunks
    public static Dictionary<String, Chunk> chunks;  //Esse Dictionary é um mapa mundo. 
                                                     //O string corresponde à posição do chunk no mundo.

    //Declarando o Material para os cubos
    public SpatialMaterial materialDoMundo = new SpatialMaterial();

    // método que é executado toda vez que um node entre na scene
    public override void _Ready()
    {
        chunkSize = tamanhoDoChunk;
        alturaDaColuna = altura;
        worldSize = tamamanhoDoMundo;

        //Prepara o Material do Cubo carregando a textura atlas.
        materialDoMundo.AlbedoTexture = (Texture)GD.Load("res://imagens/blockatlas.png");

        chunks = new Dictionary<string, Chunk>(); // Cria o dictionary que é o mapa de chunks que compõem o mundo
        this.SetIdentity();  //Antes de contruir o mundo coloca o node principal no centro do grid
        BuildWorld();  //Chama o script que inicia a contrução da coluna
    }

    //Constrói a coluna de chunks
    void BuildChunkColumn()
    {
        for(int i = 0; i < alturaDaColuna; i++)
        {
            //A posição do chunk é determinada pela posição do node principal.
            //A única alteração é no y que será incrementado pelo indice do for
            //até a altura máxima.
            Vector3 chunkPosition = new Vector3(this.Translation.x, i * chunkSize, this.Translation.z);
            Chunk c = new Chunk(chunkPosition, materialDoMundo); //Declara o chunk na posição calculada
            this.AddChild(c.chunk); //adiciona o chunk como filho do nó principal
            chunks.Add(c.Name, c);  //adiciona o chunk ao mapa
           
        }

        //passa por todos os itens do mapa e os desenha
        foreach (KeyValuePair<string, Chunk> c in chunks)
        {

            c.Value.DrawChunk(); //desenha o chunk

        }

    }

    //Constrói o MUNDO
    void BuildWorld()
    {
        for (int z = 0; z < worldSize; z++)
            for (int x = 0; x < worldSize; x++)
                for (int y = 0; y < alturaDaColuna; y++)
        {
            //A posição do chunk é determinada pela posição do node principal.
            //A única alteração é no y que será incrementado pelo indice do for
            //até a altura máxima.
            Vector3 chunkPosition = new Vector3( x * chunkSize, y * chunkSize, z * chunkSize );
            Chunk c = new Chunk(chunkPosition, materialDoMundo); //Declara o chunk na posição calculada
            this.AddChild(c.chunk); //adiciona o chunk como filho do nó principal
            chunks.Add(c.Name, c);  //adiciona o chunk ao mapa

        }

        //passa por todos os itens do mapa e os desenha
        foreach (KeyValuePair<string, Chunk> c in chunks)
        {

            c.Value.DrawChunk(); //desenha o chunk

        }

    }

    //Script que gera o nome do chunk com a sua posição para identificá-lo
    //no mapa
    public static String MontaNomeDoChunk(Vector3 pos)
    {
        return (int)pos.x + "_" + (int)pos.y + "_" + (int)pos.z;
    }
}
