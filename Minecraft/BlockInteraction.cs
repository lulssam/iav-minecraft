using UnityEngine;
using System.Collections.Generic;

public class BlockInteraction : MonoBehaviour
{
    public Camera cam;
    enum InteractionType { DESTROY, BUILD };
    InteractionType interactionType;

    // referencia do player
    GameObject player;


    private void Awake()
    {
        // encontrar tag do player
        player = GameObject.FindWithTag("Player");

    }

    private void Update()
    {
        bool interaction = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1); // Detecta apenas o clique inicial
        //Debug.Log("Interação: " + interaction);
        if (interaction)
        {
            interactionType = Input.GetMouseButtonDown(0) ? InteractionType.DESTROY : InteractionType.BUILD;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4))
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
                    if (Vector3.Distance(hitBlock, player.transform.position) < 1f) // se posição do player for a mesma do que o hitblock
                    {
                        Debug.Log("Não podes construir aqui!");
                        return;
                    }
                }

                int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
                int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
                int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);

                Chunk c;
                Vector3 playerPos = player.transform.position;
                if (World.chunkDict.TryGetValue(chunkName, out c))
                {
                    if (blockx >= 0 && blockx < World.chunkSize &&
                        blocky >= 0 && blocky < World.chunkSize &&
                        blockz >= 0 && blockz < World.chunkSize)
                    {
                        if (interactionType == InteractionType.DESTROY)
                        {
                            c.chunkData[blockx, blocky, blockz].SetType(Block.BlockType.AIR);
                        }
                        else
                        {
                            c.chunkData[blockx, blocky, blockz].SetType(Block.BlockType.DIRT);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Block indices out of bounds: ({blockx}, {blocky}, {blockz})");
                    }
                }

                List<string> updates = new List<string>();
                updates.Add(chunkName);
                if (blockx == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx - World.chunkSize, chunky, chunkz)));
                }
                if (blockx == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx + World.chunkSize, chunky, chunkz)));
                }
                if (blocky == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky - World.chunkSize, chunkz)));
                }
                if (blocky == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky + World.chunkSize, chunkz)));
                }
                if (blockz == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz - World.chunkSize)));
                }
                if (blockz == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz + World.chunkSize)));
                }

                foreach (string cname in updates)
                {
                    if (World.chunkDict.TryGetValue(cname, out c))
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


