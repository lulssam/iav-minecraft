using UnityEngine;

public class Utils : MonoBehaviour
{
    // smooth para alisar mais o terreno
    static float smooth = 0.003f; // quanto mais pequeno, mais suave
    static float smooth3D = 10 * smooth;
    static int maxHeight = 114;
    static int octaves = 6;
    static float persistence = 0.8f;
    static float offset = 32000f;

    /**Função que gera a altura usando o método Map*/
    public static int GenerateHeight(float x, float z)
    {
        return (int)Map(0, maxHeight, 0, 1, fBM(x * smooth, z * smooth, octaves, persistence));
    }

    /** Função que gera pedras a partir de dada altura*/
    public static int GenerateStoneHeight(float x, float z)
    {
        return (int)Map(0, maxHeight - 10, 0, 1, fBM(x * 2 * smooth, z * 2 * smooth, octaves - 1, 1.2f * persistence));
    }

    public static int GenerateCoalHeight(float x, float z)
    {
        return (int)Map(0, maxHeight - 15, 0, 1, fBM(x * 2 * smooth, z * 3 * smooth, octaves - 1, 1.2f * persistence));
    }

    /** Função que mapeia valores do FBM para intervalo desejado*/
    static float Map(float newMin, float newMax, float origMin, float oriMax, float value)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(origMin, oriMax, value));
    }

    public static float fBM3D(float x, float y, float z, int octaves, float persistence)
    {
        float xy = fBM(x * smooth3D, y * smooth3D, octaves, persistence);
        float yx = fBM(y * smooth3D, x * smooth3D, octaves, persistence);
        float xz = fBM(x * smooth3D, z * smooth3D, octaves, persistence);
        float zx = fBM(z * smooth3D, x * smooth3D, octaves, persistence);
        float yz = fBM(y * smooth3D, z * smooth3D, octaves, persistence);
        float zy = fBM(z * smooth3D, y * smooth3D, octaves, persistence);

        return (xy + yx + xz + zx + yz + zy) / 6.0f;

    }

    /** Função que gera um valor de Perlin Noise*/
    static float fBM(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float amplitude = 1;
        float frequency = 1;
        float maxValue = 0;
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequency, (z + offset) * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        return total / maxValue;
    }

}
