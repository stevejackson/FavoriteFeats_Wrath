using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using FavoriteFeats_Wrath.Serializers;

namespace FavoriteFeats_Wrath;

internal class StateManager
{
    // don't access this directly to access favorites, use the Favorites(characterName) method for that.
    // m_Favorites acts as a cache so we only have to read the save file once when the character changes.
    static string[] m_Favorites = [];
    static string m_CurrentCharacter = null;

    /*
     * if we're on a different character, or haven't loaded a character yet, then try loading from a file into m_Favorites.
     * otherwise, provide the data from the m_Favorites variable, and we'll keep the m_Favorites variable in sync with
     * the save file.
     */
    public static string[] Favorites(string characterName)
    {
        if (m_CurrentCharacter == characterName)
        {
            // Main.logger.Log("Favorites: Loading favorites from cache. Cache length: " + m_Favorites.Length.ToString());
            return m_Favorites;
        }

        StateManager.ClearFavoritesCache();
        m_CurrentCharacter = characterName;

        Main.Logger.Log("Favorites: " + characterName);
        string filename = StateManager.Filename(characterName);

        if (!File.Exists(filename))
        {
            Main.Logger.Log("Favorites: save file for character did not exist. filename: " + filename);
            return [];
        }

        try
        {
            using (FileStream reader = new FileStream(filename, FileMode.Open))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SavefileRootObject));
                SavefileRootObject rootObject = (SavefileRootObject)serializer.ReadObject(reader);
                reader.Close();

                string[] deserializedFavorites = StateManager.DeserializeFavorites(rootObject);

                Main.Logger.Log("Favorites: Completed loading favorites for character " + characterName);

                m_Favorites = deserializedFavorites;
                return deserializedFavorites;
            }
        }
        catch (Exception e)
        {
            Main.Logger.Log($"Failed loading: {e}");
        }

        return [];
    }

    internal static void SaveFavorites(string characterName, string[] favorites)
    {
        m_Favorites = favorites; // keep m_Favorites cache up to date with the save file.

        string filename = StateManager.Filename(characterName);
        SavefileRootObject rootObject = StateManager.SerializeFavorites(favorites);

        try
        {

            Main.Logger.Log("Saving favorites to a file.");

            using (FileStream writer = new FileStream(filename, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SavefileRootObject));
                serializer.WriteObject(writer, rootObject);
                writer.Close();
            }

            Main.Logger.Log("Successfully saved favorites to a file.");
        }
        catch (Exception e)
        {
            Main.Logger.Log($"Failed to save favorites to a file: {e}");
        }
    }

    internal static bool IsFeatFavorited(string characterName, string featName)
    {
        //Main.logger.Log("IsFeatFavorited: " + characterName + ", " + featName);

        string[] favoritesArray = Favorites(characterName);
        bool isFeatFavorited = favoritesArray.Contains(featName);

        //Main.logger.Log("IsFeatFavorited: result = " + isFeatFavorited.ToString());

        return isFeatFavorited;
    }

    internal static void ToggleFavoriteStatus(string characterName, string featName)
    {
        Main.Logger.Log("ToggleFavoriteStatus: " + characterName + ", " + featName);

        bool previouslyFavorited = IsFeatFavorited(characterName, featName);

        string[] newFavorites = Favorites(characterName);
        List<string> newFavoritesList = newFavorites.OfType<string>().ToList();

        if (previouslyFavorited)
        {
            newFavoritesList.RemoveAll(x => x == featName);
        }
        else
        {
            newFavoritesList.Add(featName);
        }

        newFavorites = newFavoritesList.Distinct().ToArray();

        StateManager.SaveFavorites(characterName, newFavorites);
    }

    internal static string Filename(string characterName)
    {
        int versionNumber = 1; // if changing save formats, update this to force a new format without deleting user's old save files
        return Path.Combine("Mods\\FavoriteFeats_Wrath", "v" + versionNumber + "_" + characterName + ".json");
    }

    internal static SavefileRootObject SerializeFavorites(string[] favorites)
    {
        SavefileRootObject rootObject = new SavefileRootObject();
        rootObject.features = new List<Feature>();

        foreach (string favString in favorites)
        {
            Feature feat = new Feature();
            feat.Name = favString;
            rootObject.features.Add(feat);
        }

        return rootObject;
    }

    internal static string[] DeserializeFavorites(SavefileRootObject rootObject)
    {
        List<string> favoriteStrings = new List<string>();

        foreach (Feature feat in rootObject.features)
        {
            favoriteStrings.Add(feat.Name);
        }

        return favoriteStrings.ToArray();
    }

    /* when switching characters, ensure we're resetting the data store. */
    internal static void ClearFavoritesCache()
    {
        m_Favorites = [];
        m_CurrentCharacter = null;
    }
}
