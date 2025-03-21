using UnityEngine;

public class Block
{
    enum Cubside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum BlockType { GRASS, DIRT, STONE, COBBLESTONE, BEDROCK, COAL, GOLD, AIR };

    Material material;
    BlockType blockType;

    Chunk owner;
    Vector3 pos;

    bool isSolid;

    static Vector2 GrassSide_LBC = new Vector2(3f, 15f) / 16;
    static Vector2 GrassTop_LBC = new Vector2(8f, 13f) / 16;
    static Vector2 Dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 Stone_LBC = new Vector2(1f, 15f) / 16;
    static Vector2 Cobblestone_LBC = new Vector2(0f, 14f) / 16;
    static Vector2 BedRock_LBC = new Vector2(1f, 13f) / 16;
    static Vector2 Coal_LBC = new Vector2(2f, 13f) / 16;
    static Vector2 Gold_LBC = new Vector2(0, 13f) / 16;

    public Vector2[,] blockUvs = {
        {GrassTop_LBC, GrassTop_LBC  + new Vector2(1f,0f)/16, GrassTop_LBC + new Vector2(0f, 1f) / 16, GrassTop_LBC + new Vector2(1f, 1f) / 16},
        {GrassSide_LBC, GrassSide_LBC + new Vector2(1f,0f)/16, GrassSide_LBC + new Vector2(0f, 1f) / 16, GrassSide_LBC + new Vector2(1f, 1f) / 16},
        {Dirt_LBC, Dirt_LBC + new Vector2(1f,0f)/16, Dirt_LBC + new Vector2(0f, 1f) / 16, Dirt_LBC + new Vector2(1f, 1f) / 16},
        {Stone_LBC, Stone_LBC + new Vector2(1f,0f)/16, Stone_LBC + new Vector2(0f, 1f) / 16, Stone_LBC + new Vector2(1f, 1f) / 16},
        {Cobblestone_LBC, Cobblestone_LBC + new Vector2(1f,0f)/16, Cobblestone_LBC + new Vector2(0f, 1f) / 16, Cobblestone_LBC + new Vector2(1f, 1f) / 16},
        {BedRock_LBC, BedRock_LBC + new Vector2(1f,0f)/16, BedRock_LBC + new Vector2(0f, 1f) / 16, BedRock_LBC + new Vector2(1f, 1f) / 16},
        {Coal_LBC, Coal_LBC + new Vector2(1f, 0f) / 16, Coal_LBC + new Vector2(0f, 1f) / 16, Coal_LBC + new Vector2(1f,1f) / 16},
        {Gold_LBC, Gold_LBC + new Vector2(1f, 0f) / 16, Gold_LBC + new Vector2(0f, 1f) / 16, Gold_LBC + new Vector2(1f, 1f) / 16}
    };

    public Block(BlockType blockType, Vector3 pos, Chunk owner, Material material)
    {
        this.owner = owner;
        this.pos = pos;
        this.material = material;
        SetType(blockType);

    }
    public void SetType(BlockType blockType)
    {
        this.blockType = blockType;
        if (blockType == BlockType.AIR)
            isSolid = false;
        else
            isSolid = true;
    }

    void CreateQuad(Cubside side)
    {
        Mesh mesh = new Mesh();

        Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);

        Vector3 n = new Vector3(0, 0, 1);
        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv11 = new Vector2(1, 1);

        if (blockType == BlockType.GRASS && side == Cubside.TOP)
        {
            uv00 = blockUvs[0, 0];
            uv10 = blockUvs[0, 1];
            uv01 = blockUvs[0, 2];
            uv11 = blockUvs[0, 3];
        }
        else if (blockType == BlockType.GRASS && side == Cubside.BOTTOM)
        {
            uv00 = blockUvs[2, 0];
            uv10 = blockUvs[2, 1];
            uv01 = blockUvs[2, 2];
            uv11 = blockUvs[2, 3];
        }
        else
        {

            uv00 = blockUvs[(int)(blockType + 1), 0];
            uv10 = blockUvs[(int)(blockType + 1), 1];
            uv01 = blockUvs[(int)(blockType + 1), 2];
            uv11 = blockUvs[(int)(blockType + 1), 3];
        }

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        int[] triangles = new int[6];
        Vector2[] uv = new Vector2[4];

        switch (side)
        {
            case Cubside.FRONT:
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;

            case Cubside.BOTTOM:
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;

            case Cubside.TOP:
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;

            case Cubside.LEFT:
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;
            case Cubside.RIGHT:
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;
            case Cubside.BACK:
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
        }

        triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        uv = new Vector2[] { uv11, uv01, uv00, uv10 };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.position = this.pos;
        quad.transform.parent = owner.goChunk.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    int ConvertToLocalIndex(int i)
    {
        if (i == -1) return World.chunkSize - 1;
        if (i == World.chunkSize) return 0;
        return i;
    }

    bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunkData;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
        {
            Vector3 neighChunkPos = owner.goChunk.transform.position + new Vector3(
                (x - (int)pos.x) * World.chunkSize,
                (y - (int)pos.y) * World.chunkSize,
                (z - (int)pos.z) * World.chunkSize);
            string chunkName = World.CreateChunkName(neighChunkPos);

            x = ConvertToLocalIndex(x);
            y = ConvertToLocalIndex(y);
            z = ConvertToLocalIndex(z);

            Chunk neighChunk;

            if (World.chunkDict.TryGetValue(chunkName, out neighChunk))
            {
                chunkData = neighChunk.chunkData;
            }
            else
            {
                return false;
            }
        }
        else
        {
            chunkData = owner.chunkData;
        }
        try
        {
            return chunkData[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
    }

    public void Draw()
    {

        if (blockType == BlockType.AIR) return;

        if (!HasSolidNeighbour((int)pos.x - 1, (int)pos.y, (int)pos.z))
            CreateQuad(Cubside.LEFT);

        if (!HasSolidNeighbour((int)pos.x + 1, (int)pos.y, (int)pos.z))
            CreateQuad(Cubside.RIGHT);

        if (!HasSolidNeighbour((int)pos.x, (int)pos.y - 1, (int)pos.z))
            CreateQuad(Cubside.BOTTOM);

        if (!HasSolidNeighbour((int)pos.x, (int)pos.y + 1, (int)pos.z))
            CreateQuad(Cubside.TOP);

        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z - 1))
            CreateQuad(Cubside.BACK);

        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z + 1))
            CreateQuad(Cubside.FRONT);
    }

}