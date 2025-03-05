using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManagerEx : MonoBehaviour
{
    // 2개 합쳐서 3중 리스트로 할수도 있지만, 너무 난잡함
    public List<List<Sprite>> realSpriteList;
    public List<List<Sprite>> abstSpriteList;

    void Start()
    {
        InitRealisticSpriteList();
        InitAbstractSpriteList();

        //Debug.Log(realSpriteList.Count);
    }

    void InitRealisticSpriteList()
    {
        // 사실적 스프라이트 리스트 세팅
        realSpriteList = new List<List<Sprite>>();

        // 폴더 내의 하위 폴더 목록 가져오기
        string folderPath = Path.Combine(Application.dataPath, "Resources/Sprites/Realistic");
        string[] subDirectories = Directory.GetDirectories(folderPath);

        // 폴더의 갯수만큼 for문 돌리기
        for (int i = 0; i < subDirectories.Length; ++i)
        {
            List<Sprite> spriteList = new List<Sprite>();

            // Resources 폴더 내의 스프라이트 로드
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>($"Sprites/Realistic/{i}");

            // 로드한 스프라이트를 리스트에 추가
            spriteList.AddRange(loadedSprites);

            // 다차원 리스트에 추가
            realSpriteList.Add(spriteList);
        }
    }

    void InitAbstractSpriteList()
    {
        // 추상적 스프라이트 리스트 세팅
        abstSpriteList = new List<List<Sprite>>();

        // 폴더 내의 하위 폴더 목록 가져오기
        string folderPath = Path.Combine(Application.dataPath, "Resources/Sprites/Abstract");
        string[] subDirectories = Directory.GetDirectories(folderPath);

        // 폴더의 갯수만큼 for문 돌리기
        for (int i = 0; i < subDirectories.Length; ++i)
        {
            List<Sprite> spriteList = new List<Sprite>();

            // Resources 폴더 내의 스프라이트 로드
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>($"Sprites/Abstract/{i}");

            // 로드한 스프라이트를 리스트에 추가
            spriteList.AddRange(loadedSprites);

            // 다차원 리스트에 추가
            abstSpriteList.Add(spriteList);
        }
    }
}
