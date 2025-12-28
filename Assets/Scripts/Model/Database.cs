using UnityEngine;

namespace SupplyChain.Model
{
    public class Database : MonoBehaviour
    {
        public Data.Game Game;
        public Data.Tool[] Tools;
        public Data.Level[] Levels;
        public AudioClip[] BGMClips;
        public AudioClip[] SFXClips;

        private void Awake()
        {
            Globals.Database = this;
        }
    }
}