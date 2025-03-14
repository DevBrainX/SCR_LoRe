using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerEx
{
    // 2개 합쳐서 3중 리스트로 할수도 있지만, 너무 난잡함
    public List<List<Sprite>> realSpriteList;
    public List<List<Sprite>> abstSpriteList;

    public void Init()
    {
        InitRealisticSpriteList();
        InitAbstractSpriteList();
    }

    private void InitRealisticSpriteList()
    {
        // 사실적 스프라이트 리스트 세팅
        realSpriteList = new List<List<Sprite>>();

        int index = 0;

        // 폴더가 존재하지 않을 때까지 반복
        while (true)
        {
            // Resources 폴더 내의 스프라이트 로드
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Realistic/" + index);

            // 스프라이트가 로드되지 않았다면 반복 종료
            if (loadedSprites.Length == 0)
            {
                break; // 폴더가 없거나 스프라이트가 없으면 종료
            }

            // 스프라이트 리스트 생성 및 추가
            List<Sprite> spriteList = new List<Sprite>(loadedSprites);
            realSpriteList.Add(spriteList);

            // 다음 폴더로 이동
            index++;
        }

        Logger.Log("Complete InitRealisticSpriteList()");
    }

    private void InitAbstractSpriteList()
    {
        // 추상적 스프라이트 리스트 세팅
        abstSpriteList = new List<List<Sprite>>();

        int index = 0;

        // 폴더가 존재하지 않을 때까지 반복
        while (true)
        {
            // Resources 폴더 내의 스프라이트 로드
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Abstract/" + index);

            // 스프라이트가 로드되지 않았다면 반복 종료
            if (loadedSprites.Length == 0)
            {
                break; // 폴더가 없거나 스프라이트가 없으면 종료
            }

            // 스프라이트 리스트 생성 및 추가
            List<Sprite> spriteList = new List<Sprite>(loadedSprites);
            abstSpriteList.Add(spriteList);

            // 다음 폴더로 이동
            index++;
        }

        Logger.Log("Complete InitAbstractSpriteList()");
    }
}
