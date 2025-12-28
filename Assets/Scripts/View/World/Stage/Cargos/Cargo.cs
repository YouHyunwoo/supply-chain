using UnityEngine;

namespace SupplyChain.View.World
{
    public class Cargo : MonoBehaviour
    {
        public Node Origin;
        public float Value;
        public float Amount;

        public virtual void StartTransport() { }

        public virtual void CompleteTransport() { }
    }
}