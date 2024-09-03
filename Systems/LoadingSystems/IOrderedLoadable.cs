namespace CrystalMoon.Systems.LoadingSystems
{
    interface IOrderedLoadable
    {
        void Load();
        void Unload();
        float Priority { get; }
    }
}
