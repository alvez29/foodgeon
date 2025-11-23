namespace Project.Code.Core.Data
{
    [System.Serializable]
    public struct EatenEnemyData
    {
        public string name;
        public Flavor flavor;

        public EatenEnemyData(string name, Flavor flavor)
        {
            this.name = name;
            this.flavor = flavor;
        }
    }
}