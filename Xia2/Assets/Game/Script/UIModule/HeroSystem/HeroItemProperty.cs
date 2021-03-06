using UnityEngine;
using System.Collections;

public class HeroItemProperty : MonoBehaviour {

	public UILabel  Name;            //英雄名字
	public UILabel  FightPowerLabel; //战斗力数字
	public UISprite FightIcon;       //出战图标
	public UISprite HelpIcon;        //助战图标
	public UISprite Icon;            //头像图标
	public UISprite []Stars = new UISprite[5];
	public UISprite AtkType;         //攻击类型图标
	public UILabel  RealmLevel;      //境界等级

	//lv process
	public UILabel Level;       //等级
	public UILabel LevelNum;    //等级
	public UIProgressBar LvBar; //等级进度条
	//debirs porcess
	public UILabel debirsNum;       //碎片个数
	public UIProgressBar debirsBar; //碎片进度条

	public int  idx{ get; set;}     // 记录索引
	public long heroGuid{ get; set;}// 记录唯一ID

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(targetNum > 0)
		{
			targetNum -= barSpeed;
			barcurNum += barSpeed;

			float percent = (float)barcurNum / (float)barMaxNum;
			LvBar.value = percent;
			if(barcurNum >= barMaxNum)
			{
				barcurNum -= barMaxNum;
				tempLevel++;
				Level.text = "Lv." + tempLevel;
				LevelNum.text = "Lv." + tempLevel;
				barMaxNum = StaticLevel.Instance ().getInt (tempLevel, "exp");
				barSpeed = barMaxNum / 120;
				if (barSpeed < 1)
					barSpeed = 1;
			}
		}
	}

	public void refreshInfo(HeroData data)
	{
		if (null == data)
			return;
		setTempDefaultVal (data.level, data.exp);

		Icon.spriteName = data.icon_middle;
		setStar (data.starLevel, data.activate);
		setCountry (data.property.country, data.name);
		setAtkIcon (data.heroType, data.activate);
		if(LevelNum != null) LevelNum.text = "Lv." + data.level;
		if(RealmLevel != null) RealmLevel.text = "+"+data.realmList.curRealmLevel;
		if(HelpIcon != null)HelpIcon.gameObject.SetActive(false);
		if(FightIcon != null)FightIcon.gameObject.SetActive(false);
		if(data.battle >= 0 && data.battle < 3)
		{
			if(FightIcon != null)
				FightIcon.gameObject.SetActive(true);
		}
		else if(data.battle >= 3)
		{
			if(HelpIcon != null)
				HelpIcon.gameObject.SetActive(true);
		}
		if (FightPowerLabel != null)
			FightPowerLabel.text = ""+data.calcFight ();
//
//		if(data.activate)
//		{
//			if(LvBar != null)
//				LvBar.gameObject.SetActive(true);
//			if(debirsBar != null)
//				debirsBar.gameObject.SetActive(false);
//			updateLvBar(data);
//		}
//		else 
//		{
			if(LvBar != null)
				LvBar.gameObject.SetActive(false);
			if(debirsBar != null)
				debirsBar.gameObject.SetActive(true);
			updateDebrids(data);
//		}

	}

	void OnClick()
	{
		UIManager manager = (UIManager)FindObjectOfType(typeof(UIManager));
		if(manager != null)
		{
			MonoInstancePool.getInstance<HeroManager>().curSelectHero = idx;
			MonoInstancePool.getInstance<HeroManager>().curSelectHeroGUID = heroGuid;
			manager.show("HeroSysModules");
		}
	}
	//
	public void center()
	{
		UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(gameObject);
		UIPanel panel = NGUITools.FindInParents<UIPanel>(gameObject);
		center.enabled = true;
		if (center != null)
		{
			if (center.enabled)
				center.CenterOn(transform);
		}
	}
	public void setCountry(int c, string name)
	{
		Name.text = name;
		return;
		string country = "";
		switch(c)
		{
		case (int)GlobalDef.HeroCountry.ROLE_WEI:   //魏
			country = "魏";
			break;
		case (int)GlobalDef.HeroCountry.ROLE_SHU:   //蜀
			country = "蜀";
			break;
		case (int)GlobalDef.HeroCountry.ROLE_WU:    //吴
			country = "吴";
			break;
		case (int)GlobalDef.HeroCountry.ROLE_QUN:   //群
			country = "群";
			break;
		case (int)GlobalDef.HeroCountry.ROLE_JIN:   //晋
			country = "晋";
			break;
		}
		if(string.Compare(Name.text,name) != 0) 
			Name.text = name + "-(" +country+")";
	}
	public void setAtkIcon (int type, bool active)
	{
		string spriteName = "";
		switch(type)
		{
		case (int)GlobalDef.EquipNeedRoleType.E_KNIFE:           //刀类
			spriteName = active ? "ui_voc_daoke" : "ui_voc_gray_daoke";
			break;
		case (int)GlobalDef.EquipNeedRoleType.E_TWOKNIFE:        //双刀类
			spriteName = active ? "ui_voc_cike" : "ui_voc_gray_cike";
			break;
		case (int)GlobalDef.EquipNeedRoleType.E_LONGKNIFE:  	 //长柄利器类
			spriteName = active ? "ui_voc_mengshi" : "ui_voc_gray_mengshi";
			break;
		case (int)GlobalDef.EquipNeedRoleType.E_LONGWEAPON:      //长柄类
			spriteName = active ? "ui_voc_lishi" : "ui_voc_gray_lishi";
			break;
		case (int)GlobalDef.EquipNeedRoleType.E_LONGDISTANCE:    //远程类
			spriteName = active ? "ui_voc_sheshou" : "ui_voc_gray_sheshou";
			break;
		case (int)GlobalDef.EquipNeedRoleType.E_MAGE:            //法师类
			spriteName = active ? "ui_voc_moushi" : "ui_voc_gray_moushi";
			break;
		}
		if(AtkType != null) AtkType.spriteName = spriteName;
	}
	public void setFight(int pos)
	{
		FightIcon.gameObject.SetActive (true);
		if(pos < 3)
		{
			FightIcon.spriteName = "hb（new)_11";
		}
		else
		{
			FightIcon.spriteName = "hb（new)_11_1";
		}
	}
	public void cancelFight()
	{
		FightIcon.gameObject.SetActive (false);
	}
	public void updateLvBar(HeroData data)
	{
		if (data == null)
			return;
		if(LvBar == null)
			return;

		if(!LvBar.gameObject.activeSelf) LvBar.gameObject.SetActive(true);
		string lv = "Lv." + data.level;
		if(string.Compare(Level.text, lv) != 0)
		{
			Level.text = lv;
			LevelNum.text = lv;
		}
		int exp = data.exp;
		int level = data.level;
		int needExp = StaticLevel.Instance ().getInt (level, "exp");
		float percent = (float)exp / (float)needExp;
		LvBar.value = percent;
	}
	public void updateDebrids(HeroData data)
	{
		if (debirsBar == null)
			return;
		if(!debirsBar.gameObject.activeSelf) debirsBar.gameObject.SetActive(true);
		int have = data.debris;
		int level = data.level;
//		int initStar = StaticHero.Instance ().getInt(data.templateID, "init_star"); 
		int needNum = StaticHero.Instance ().getInt(data.templateID,"itemnum");
		float percent = (float)have / (float)needNum;
		debirsBar.value = percent;
		string str = "" + data.debris + "/" + needNum;
		if(string.Compare(debirsNum.text, str) != 0)
			debirsNum.text = str;
	}

	public void setFightClick()
	{
		HeroData data = MonoInstancePool.getInstance<HeroManager> ().getHeroByIdx (idx);
		if(data == null)
		{
			Debug.LogError("idx is error!");
			return;
		}
		HeroListWindowManager listWindow = (HeroListWindowManager)FindObjectOfType(typeof(HeroListWindowManager));
		if(listWindow)
		{
			listWindow.curSlectedIdx = idx;
		}
		HeroSysUIManager uiManager = (HeroSysUIManager)FindObjectOfType(typeof(HeroSysUIManager));
		if(uiManager)
		{
			uiManager.showFightListWindow(1);//1:设置出战 选择位置状态 
			uiManager.hideExpWindow();
			uiManager.hideDebirsWindow();
		}
	}
	public void cancelFightClick()
	{
		HeroData data = MonoInstancePool.getInstance<HeroManager> ().getHeroByIdx (idx);
		if(data == null)
		{
			Debug.LogError("idx is error!");
			return;
		}
		MonoInstancePool.getInstance<SendHeroSysMsgHander>().sendCancelFightHeroReq(data.guid);
	}
	public void callHero()
	{
		HeroData data = MonoInstancePool.getInstance<HeroManager> ().getHeroByIdx (idx);
		if(data == null)
		{
			Debug.LogError("idx is error!");
			return;
		}
		MonoInstancePool.getInstance<SendHeroSysMsgHander> ().SendHeroCall (data.guid);
	}
	//记录下等级和经验 用于模拟
	int tempLevel;
	int tempExp;
	int targetNum = 0;
	int barMaxNum = 0;
	int barcurNum = 0;
	int barSpeed = 10;
	public void setTempDefaultVal(int level, int exp)
	{
		tempLevel = level;
		tempExp = exp;

		barSpeed = barMaxNum / 120;
		if (barSpeed < 1)
			barSpeed = 1;
		barcurNum = tempExp;
		int needExp = StaticLevel.Instance ().getInt (tempLevel, "exp");
		barMaxNum = needExp;
		float percent = (float)barcurNum / (float)barMaxNum;
		if(LvBar != null)LvBar.value = percent;

	}
	public void stopTempExpAni()
	{
		targetNum = 0;
		HeroData data = MonoInstancePool.getInstance<HeroManager> ().getHero(heroGuid);
		if(null != data)
		{
			refreshInfo(data);
		}
	}
	public void tempAddExp(int num)
	{
		targetNum += num;
	}
	public void setStar(int star, bool active)
	{
		for(int i = 0; i < Stars.Length; i++)
		{
			if(i < star)
			{
				Stars[i].gameObject.SetActive(true);

				Vector3 pos = Vector3.zero;
				pos.x += i * Stars[i].width + 6;
				Stars[i].transform.localPosition = pos;
			}
			else
			{
				Stars[i].gameObject.SetActive(false);
			}
		}
		int width = (star-1) * Stars [0].width + (star - 1) * 6;
		Vector3 parentPos = Stars [0].transform.parent.localPosition;
		parentPos.x = 0;
		parentPos.x -= width / 2;
		Stars [0].transform.parent.localPosition = parentPos;
	}
	public void showLvBar()
	{

	}
//	public void showAddStarDeberisWindow()
//	{
//		UIManager manager = (UIManager)FindObjectOfType (typeof(UIManager));
//		if(manager != null)
//		{
//			manager.show("HeroSysDeberisWindow");
//		}
//	}
	public void showAddDeberisWindow()
	{
		UIManager manager = (UIManager)FindObjectOfType (typeof(UIManager));
		if(manager != null)
		{
			MonoInstancePool.getInstance<HeroManager>().curSelectHero = idx;
			MonoInstancePool.getInstance<HeroManager>().curSelectHeroGUID = heroGuid;
			manager.show("HeroSysDeberisWindow");
		}
	}
	public void hideFightIcon()
	{
		if(FightIcon != null) FightIcon.gameObject.SetActive (false);
		if(HelpIcon  != null) HelpIcon.gameObject.SetActive(false);
	}
}
