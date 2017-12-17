using UnityEngine;
using System.Collections;

public class SkillDataC : MonoBehaviour {
	public SkillSetting[] skill = new SkillSetting[3];
}

[System.Serializable]
public class SkillSetting{
	public string skillName = "";
	public Texture2D icon;
	public Transform skillPrefab;
	public AnimationClip skillAnimation;
	public int manaCost = 10;
	public float castTime = 0.5f;
	public float skillDelay = 0.3f;
	public string description = "";
	public GameObject castEffect;
	public string sendMsg = "";
	public whileAtk whileAttack = whileAtk.Immobile;
}