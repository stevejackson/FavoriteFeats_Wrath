namespace FavoriteFeats_Wrath;

internal class Utilities
{
    static public string FavoriteFeatTextMarker()
    {
        return "(***) ";
    }

    static public string CleanFeatName(string text)
    {
        if (text.StartsWith(FavoriteFeatTextMarker()))
        {
            return text.Remove(FavoriteFeatTextMarker().Length);
        }

        return text;
    }
}

