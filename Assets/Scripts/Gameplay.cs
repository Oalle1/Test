using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneTransitionSystem;


public class Gameplay : MonoBehaviour
{
    private List<Tile> tiles;
    public List<Sprite> spritesLevelAndroid;
    public List<Sprite> spritesLevelApple;
    private List<Tile> shuffleTiles;
    private float time = 180f;
    public Text textChrono;


    /// <summary>
    /// Start is called before the first frame update
    /// We Detect which platform the player use and change the Tiles accordingly
    /// We Shuffle the tiles so they can spwan randomly
    /// We place them on the Canva
    /// Then we start the countdown
    /// </summary>
    void Start()
    {
#if UNITY_ANDROID
		InitTile(spritesLevelAndroid);
#else
        InitTile(spritesLevelApple); 
#endif
        ShuffleTiles();
        Placement();
        StartCoroutine(Chrono());
    }

    /// <summary>
    /// This function take the sprites of the Logo and transform it into Tiles
    /// </summary>
    /// <param name="spritesLevel">The sprite to transform in tile</param>
    void InitTile(List<Sprite> spritesLevel)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tile = transform.GetChild(i);
            tile.GetComponent<Image>().sprite = spritesLevel[i];
            tile.GetComponent<Tile>().id = i;
        }
    }

    /// <summary>
    /// This function is the Chrono which will be used while the player is playing
    /// </summary>
    private IEnumerator Chrono()
    {
        while (time > 0f)
        {
            time--;
            yield return new WaitForSeconds(1f);
            if (time < 0)
                time = 0;
            textChrono.text = (int)time / 60 + " : " + (int)time % 60;
        }
    }

    /// <summary>
    /// This function check if there is another tile where we want to move our tile, and exchange the tiles we want to move and the empty tile (not working yet)
    /// </summary>
    /// <param name="vect">vector where the tile want to go</param>
    /// <param name="id">indice of the tile</param>
    /// <returns>true if there is no tile, false otherwise</returns>
    public bool NoTilesUnder(Vector2 vect,int id)
    {
        int tmp=0,tmp2=0;
        Vector2 vecTemp;
        for (int i = 0; i < shuffleTiles.Count; i++)
        {
            if (shuffleTiles[i].id != 4 && i!=id)
            {
                if (shuffleTiles[i].startPosition == vect)
                    return false;
            }
            else if (shuffleTiles[i].id == 4)
                tmp = i;
            else
                tmp2 = i;
        }
        vecTemp = shuffleTiles[tmp].positionInitial;
        shuffleTiles[tmp].positionInitial = shuffleTiles[tmp2].positionInitial;
        shuffleTiles[tmp2].positionInitial = vecTemp;
        return true;
    }

    /// <summary>
    /// This function randomise the position of our tile
    /// </summary>
    void ShuffleTiles()
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < 9; i++)
        {
            tiles.Add(new Tile(i));
        }
        shuffleTiles = tiles.OrderBy(id => Random.value).ToList();
        while (!IsResolvable())
        {
            shuffleTiles = tiles.OrderBy(id => Random.value).ToList();
        }
        
    }

    /// <summary>
    /// Check if a Tile is correctly placed
    /// </summary>
    /// <param name="ind">indice of the tile</param>
    /// <param name="corr">if the tile is correctly placed or not</param>
    /// <returns>true if the tile is well placed, false otherwise</returns>
    public void CorrectTile(int ind,bool corr)
    {
        for (int i = 0; i<9; i++)
        {
            if (ind== shuffleTiles[i].id)
            {
                shuffleTiles[i].isCorrect = corr;
            }
        }
    }

    /// <summary>
    /// The algorithm used to verify if the puzzle is possible using a select sort so we can count how many time we permutade the number
    /// and verify if it's odd or even and compare it to the number of tile which the empty tile is from his original position. 
    /// If it's the same result as the number of permutation, the puzzle is solvable
    /// </summary>
    /// /// <returns>true if the puzzle is resolvable, false otherwise</returns>
    bool IsResolvable()
    {
        int n = 9;
        int m,temp;
        int nbrPermutation = 0;
        Tile[] copy = shuffleTiles.ToArray();
        int[] idoriginal=new int[9];
        for (int i = 0;i<n;i++)
        {
            idoriginal[i] = copy[i].id;
        }
        for (int i = n-1; i >=0; i--)
        {
            m = i;
            for(int j = i;j>=0;j--)
            {
                if (copy[j].id>copy[m].id)
                {
                    m = j;
                }
            }
            if(m!=i)
            {
                nbrPermutation++;
                temp = copy[m].id;
                copy[m].id = copy[i].id;
                copy[i].id = temp;
            }
        }
        for (int i = 0; i < n; i++)
        {
            copy[i].id = idoriginal[i];
        }
        if (copy[0].id==4|| copy[2].id == 4|| copy[4].id == 4 || copy[6].id == 4|| copy[8].id == 4)
        {
            if(nbrPermutation % 2 == 0)
            {
                return true;
            }  
        }
        else
        {
            if (nbrPermutation % 2 == 1)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// We place every Tile on their position and initiate their initial position that they will keep until the end to check if they are placed correctly
    /// </summary>
    void Placement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == 0)
            {
                shuffleTiles[i].positionInitial = new Vector2(-100, 100);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(-100, 100);
            }
            if (i == 1)
            {
                shuffleTiles[i].positionInitial = new Vector2(0, 100);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(0, 100);
            }
            if (i == 2)
            {
                shuffleTiles[i].positionInitial = new Vector2(100, 100);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(100, 100);
            }
            if (i == 3)
            {
                shuffleTiles[i].positionInitial = new Vector2(-100, 0);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(-100, 0);
            }
            if (i == 4)
            {
                shuffleTiles[i].positionInitial = new Vector2(0, 0);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(0, 0);
            }
            if (i == 5)
            {
                shuffleTiles[i].positionInitial = new Vector2(100, 0);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(100, 0);
            }
            if (i == 6)
            {
                shuffleTiles[i].positionInitial = new Vector2(100, -100);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(-100, -100);
            }
            if (i == 7)
            {
                shuffleTiles[i].positionInitial = new Vector2(100, 0);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(0, -100);
            }
            if (i == 8)
            {
                shuffleTiles[i].positionInitial = new Vector2(100, 100);
                transform.GetChild(shuffleTiles[i].id).localPosition = new Vector3(100, -100);
            }
        } 
    }

    /// <summary>
    /// Check if every Tiles is well placed
    /// </summary>
    /// <returns>true if the player won, false otherwise</returns>
    bool finish()
    {
        for (int i = 0; i < 9; i++)
        { 
            if(shuffleTiles[i].isCorrect==false)
                    return false;
                
        }
        return true;
    }

    /// <summary>
    /// Check if the player win or lose and save his high score.
    /// </summary>
    void Update()
    {
        if (time <= 0)
            STSSceneManager.LoadScene("Home");
        if (finish())
        {
            if (time > PlayerPrefs.GetFloat("Score")) ;
            PlayerPrefs.SetFloat("Score", time);
            PlayerPrefs.Save();
            STSSceneManager.LoadScene("Home");
        }
    }
}
