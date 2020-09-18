using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ActionAnimationInfo 
{
    public enum Source
    {
        Entity,
        Weapon
    }

    public enum Category
	{
        Offensive,
        Defensive
	}

    public Source source;
    public Category category;
    public int id;


}
