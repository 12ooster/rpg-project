using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager {

    private Dictionary<int, CharStatsScript> characterData;

    public GameDataManager() {

    }

    public GameDataManager( CharStatsScript[] characters) {
        this.SetCharacterData(characters);
    }

    public void SetCharacterData(CharStatsScript[] characters) {
        this.characterData = new Dictionary<int, CharStatsScript>();
        for (int i = 0; i < characters.Length; i++) {
            this.characterData.Add(characters[i].ID, characters[i]);
        }
    }

    public Sprite GetCharacterUIImage( int charId) {
        CharStatsScript charStats = this.characterData[charId];
        Sprite sprite = charStats.charSprite;
        return sprite;
    }
}
