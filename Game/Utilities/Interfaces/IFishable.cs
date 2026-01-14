using Godot;
using System;

namespace WaffleStock
{
    public interface IFishable
    {
        public bool IsInDisplay { get; set; }
        public bool IsCaught { get; set; }
        public bool CantGetCaught { get; set; }
        public bool Escape(IFisher fisher);
        public void GetCaughtBy(IFisher fisher);
        public bool EscapeOrGetCaughtBy(IFisher fisher)
        {
            if(Escape(fisher)) return true;
            GetCaughtBy(fisher);
            return false;
        }
    }
}
