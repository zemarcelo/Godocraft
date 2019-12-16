using System;
using Godot;


public class Utils
{
    static int          seed = 100;
    static int     maxHeight = 150;
    static float      period = 128f;
    static int       octaves = 4;
    static float persistence = 0.5f;

    public static int GenerateHeight(float x, float z)
    {
        OpenSimplexNoise noise = new OpenSimplexNoise();
        noise.Seed = seed;
        noise.Octaves = octaves;
        noise.Persistence = persistence;
        noise.Period = period;
        float height = Map(0, maxHeight, -1, 1, noise.GetNoise2d( x, z )); 
        return (int)height;
    }

    public static float Draw3DStones(float x, float y, float z)
    {
        OpenSimplexNoise noise = new OpenSimplexNoise();
        noise.Seed        = seed;
        noise.Octaves     = 1; 
        noise.Persistence = persistence;
        noise.Period      = period * 10f;
        float probability = Map(0, 1, -1, 1, noise.GetNoise3d(x, y, z));
        return  probability;
    }

    public static int GenerateStoneHeight(float x, float z)
    {
        OpenSimplexNoise noise = new OpenSimplexNoise();
        noise.Seed = seed;
        noise.Octaves = octaves + 2;
        noise.Persistence = persistence;
        noise.Period = period / 3;
        float height = Map(0, maxHeight - 5, -1, 1, noise.GetNoise2d(x, z));
        return (int)height;
    }



    static float Map(float newMin, float newMax, float origMin, float origMax, float value)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(origMin, origMax, value));
    }

   /* static float fBM(float x, float z, int oct, float pers)
    {
        OpenSimplexNoise noise = new OpenSimplexNoise();
        float total     = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for(int i = 0; i < oct; i++)
        {

            total += noise.GetNoise2d(x * frequency, z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= pers;
            frequency *= 2;
        }
        return total / maxValue;

    } */

}

