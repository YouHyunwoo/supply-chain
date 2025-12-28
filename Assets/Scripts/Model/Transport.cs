using SupplyChain.View.World;

namespace SupplyChain.Model
{
    public class Transport
    {
        public Cargo cargo;
        public int currentInterval = 0;
        public float progress = 0.0f;
    }
}