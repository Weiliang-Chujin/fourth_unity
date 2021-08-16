using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Your.Namespace.Here.UniqueStringHereToAvoidNamespaceConflicts.Lists;

/*
 * 发送http请求获取数据
 */
public class HeroTowerRankListApi : BaseAPI
{
    public HeroTowerRankListApi(GameObject gameObject) : base(gameObject)
    {
 
        ForceRequest = false;
        Sprite s = Resources.Load<Sprite>("");
    }
    
    //构建httpClientBuilder发送请求
    public void Request(int seasonId = 0, int page = 1, bool isForceRequest = false)
    {
        var httpClientBuilder = CreateHttpClientBuilder(seasonId, page, isForceRequest);
        SendRequest(httpClientBuilder);
    }
   
    private HttpClientBuilder CreateHttpClientBuilder(int seasonId = 0, int page = 1, bool isForceRequest = false)
    {
        ForceRequest = isForceRequest;
        isEncode = false;

        string path = "admin/rankList";
        HttpClientBuilder httpClientBuilder = new HttpClientBuilder(DomainType.Pvp)
            .Path(path)
            .Param("page", page)
            .Param("type", 1)
            .Param("season", 18)
            .Param("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI0MzY4NjY1MjcifQ.drFj2OtLEjgE452sgtHPG73xU-yQ-OXvbz4Utxl2M1k")
            .Method(HttpMethod.Get);
        if (seasonId > 0)
        {
            httpClientBuilder.Param("seasonId", seasonId);
        }

        return httpClientBuilder;
    }
   
    //获取json数据
    public static RankData[] ParseResponse(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }
        
        JSONNode tempJsons = JSON.Parse(data);
        var newItems = new RankData[tempJsons["data"]["list"].Count];
        for (int i = 0; i < tempJsons["data"]["list"].Count; ++i)
        {
            var tempOneJson = tempJsons["data"]["list"][i];
            var model = new RankData()
            {
                uid = tempOneJson["uid"],
                nickName = tempOneJson["nickName"],
                avatar = tempOneJson["avatar"],
                trophy = tempOneJson["trophy"],
                thirdAvatar = tempOneJson["thirdAvatar"],
                onlineStatus = tempOneJson["onlineStatus"],
                role = tempOneJson["onlineStatus"],
                abb = tempOneJson["onlineStatus"],
            };
            newItems[i] = model;
        }
        
        //根据奖杯数降序排序
        Array.Sort(newItems, (x, y) =>
        { 
            return y.trophy - x.trophy;
        });
        
        return newItems;
    }

    
}