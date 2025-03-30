using System.Runtime.Serialization;

namespace Shared;

public enum SpaceCompany
{
    [EnumMember(Value = "Space Voyager")]
    SpaceVoyager,
    [EnumMember(Value = "Explore Origin")]
    ExploreOrigin,
    [EnumMember(Value = "Space Piper")]
    SpacePiper,
    [EnumMember(Value = "Space Odyssey")]
    SpaceOdyssey,
    [EnumMember(Value = "SpaceX")]
    SpaceX,
    [EnumMember(Value = "Explore Dynamite")]
    ExploreDynamite,
    [EnumMember(Value = "Spacelux")]
    Spacelux,
    [EnumMember(Value = "Spacegenix")]
    Spacegenix,
    [EnumMember(Value = "Galaxy Express")]
    GalaxyExpress,
    [EnumMember(Value = "Travel Nova")]
    TravelNova
}

public enum Planet
{
    Earth,
    Mars,
    Venus,
    Jupiter,
    Saturn,
    Uranus,
    Neptune,
    Mercury
}