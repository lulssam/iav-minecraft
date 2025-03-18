using UnityEngine;
using System.Collections.Generic;

public class BlockInteraction : MonoBehaviour
{
    public Camera cam;
    enum InteractionType { DESTROY, BUILD };
    InteractionType interactionType;
    // Update is called once per frame
    void Update()
    {
        bool interaction = Input.GetMouseButton(0) || Input.GetMouseButton(1);
        if (Input.GetMouseButton(0))
        {
            interactionType = Input.GetMouseButton(0) ? InteractionType.DESTROY : InteractionType.BUILD;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
            {
                string chunkName = hit.collider.gameObject.name;
                float chunkx = hit.collider.gameObject.transform.position.x;
                float chunky = hit.collider.gameObject.transform.position.y;
                float chunkz = hit.collider.gameObject.transform.position.z;

                Vector3 hitBlock;

                if (interactionType == InteractionType.DESTROY)
                {
                    hitBlock = hit.point - hit.normal / 2f;
                }
                else
                {
                    hitBlock = hit.point + hit.normal / 2f;
                }
                int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
                int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
                int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);

                Chunk c;
                if (World.chunkDict.TryGetValue(chunkName, out c))
                {
                    if (interactionType == InteractionType.DESTROY)
                        c.chunkData[blockx, blocky, blockz].SetType(Block.BlockType.AIR);
                    else
                        c.chunkData[blockx, blocky, blockz].SetType(Block.BlockType.STONE);
                }

                List<string> updates = new List<string>();
                updates.Add(chunkName);

                if (blockx == 0)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx - World.chunkSize, chunky, chunkz)));
                if (blockx == World.chunkSize - 1)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx + World.chunkSize, chunky, chunkz)));
                if (blocky == 0)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky - World.chunkSize, chunkz)));
                if (blocky == World.chunkSize - 1)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky + World.chunkSize, chunkz)));
                if (blockz == 0)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz - World.chunkSize)));
                if (blockz == World.chunkSize - 1)
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz + World.chunkSize)));

                foreach (string name in updates)
                {
                    if (World.chunkDict.TryGetValue(name, out c))
                    {
                        DestroyImmediate(c.goChunk.GetComponent<MeshFilter>());
                        DestroyImmediate(c.goChunk.GetComponent<MeshRenderer>());
                        DestroyImmediate(c.goChunk.GetComponent<MeshCollider>());
                        c.DrawChunk();
                    }
                }
            }
        }
    }
}
