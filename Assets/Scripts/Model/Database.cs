using UnityEngine;

namespace SupplyChain.Model
{
    public class Database : MonoBehaviour
    {
        public Data.Tool[] Tools;
        public Data.Region[] Regions;
        public AudioClip[] BGMClips;
        public AudioClip[] SFXClips;

        private void Awake()
        {
            Globals.Database = this;
        }
    }
}