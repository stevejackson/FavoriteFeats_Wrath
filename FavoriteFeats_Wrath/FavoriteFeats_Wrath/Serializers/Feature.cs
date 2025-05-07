using System.Runtime.Serialization;

namespace FavoriteFeats_Wrath.Serializers;

[DataContract]
internal class Feature
{
    [DataMember]
    public string Name { get; set; }
}
