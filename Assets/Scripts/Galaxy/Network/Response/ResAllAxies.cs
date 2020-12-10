using Net.HungryBug.Core.Network;

namespace Net.HungryBug.Galaxy.Network
{
    public class ResAllAxies : GalaxyResponse<SmAllAxies>
    {
        public ResAllAxies(NetResponse res) : base(res) { }

        //todo: need parse data
        protected override SmAllAxies Deserialize(byte[] data) { return new SmAllAxies(); }

        protected override SmAllAxies FromJson(string json)
        {
            return UnityEngine.JsonUtility.FromJson<SmAllAxies>(json);
        }
    }
}
