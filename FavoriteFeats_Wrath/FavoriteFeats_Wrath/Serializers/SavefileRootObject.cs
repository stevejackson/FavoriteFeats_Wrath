using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FavoriteFeats_Wrath.Serializers;

[DataContract]
internal class SavefileRootObject
{
    [DataMember]
    public List<Feature> features { get; set; }
}
