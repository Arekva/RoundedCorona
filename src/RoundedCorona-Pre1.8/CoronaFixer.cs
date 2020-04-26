using UnityEngine;

namespace Arthur
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CoronaFixer : MonoBehaviour
    {
        public const float SmallRingDistance = 6.6F;

        public static float BigRingDistance { get; set; } = 11.5F;
        public static string CoronaName { get; set; } = "Corona";

        public static CoronaFixer Instance { get; private set; } = null;

        private void Start()
        {
            if(Instance)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
                Instance = this;
            }
        }

        private void Update() //Loop until a corona is found
        {
            GameObject go = GameObject.Find(CoronaName);

            if (go)
            {
                Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

                if (m)
                {
                    /* Basically, the inner circle of the corona is at a distance of 6.6F
                        * from the center of the model.
                        *
                        * So for every vertice farther than this distance, force them to be
                        * at 11.5F from the center. I've noticed this value was a good
                        * average value.
                        * 
                        */

                    Vector3[] verts = m.vertices;

                    float sqrMinDistance = SmallRingDistance * SmallRingDistance;

                    for (int i = 0; i < verts.Length; i++)
                    {
                        // If outside vertices
                        if (Vector3.SqrMagnitude(verts[i]) > sqrMinDistance)
                        {
                            // Force the outside vertices to be at a precise distance.
                            verts[i] = verts[i].normalized * BigRingDistance;
                        }
                    }

                    m.vertices = verts;

                    // Destroy this once done.
                    Instance = null;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
