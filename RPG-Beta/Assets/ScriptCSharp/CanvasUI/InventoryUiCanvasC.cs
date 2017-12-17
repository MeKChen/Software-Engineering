using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUiCanvasC : MonoBehaviour {
	public GameObject player;
	
	public Text moneyText;
	
	public Image[] itemIcons = new Image[16];
	public Text[] itemQty = new Text[16];
	public Image[] equipmentIcons = new Image[16];
	
	public Image weaponIcons;
	public Image armorIcons;
	public Image accIcons;
	
	public GameObject tooltip;
	public Image tooltipIcon;
	public Text tooltipName;
	public Text tooltipText1;
	public Text tooltipText2;
	public Text tooltipText3;
	
	public GameObject usableTab;
	public GameObject equipmentTab;
	
	public GameObject database;
	private ItemDataC db; 
	
	void Start(){
		db = database.GetComponent<ItemDataC>();
	}
	
	void Update (){
		if(tooltip && tooltip.activeSelf == true){
			Vector2 tooltipPos = Input.mousePosition;
			tooltipPos.x += 7;
			tooltip.transform.position = tooltipPos;
		}
		if(!player){
			return;
		}
		
		for(int a = 0; a < itemIcons.Length; a++){
			itemIcons[a].GetComponent<Image>().sprite = db.usableItem[player.GetComponent<InventoryC>().itemSlot[a]].iconSprite;
			itemIcons[a].GetComponent<Image>().color = db.usableItem[player.GetComponent<InventoryC>().itemSlot[a]].spriteColor;
		}
		
		for(int q = 0; q < itemQty.Length; q++){
			string qty = player.GetComponent<InventoryC>().itemQuantity[q].ToString();
			if(qty == "0"){
				qty = "";
			}
			itemQty[q].GetComponent<Text>().text = qty;
		}
		
		for(int b = 0; b < equipmentIcons.Length; b++){
			equipmentIcons[b].GetComponent<Image>().sprite = db.equipment[player.GetComponent<InventoryC>().equipment[b]].iconSprite;
			equipmentIcons[b].GetComponent<Image>().color = db.equipment[player.GetComponent<InventoryC>().equipment[b]].spriteColor;
		}
		
		if(weaponIcons){
			weaponIcons.GetComponent<Image>().sprite = db.equipment[player.GetComponent<InventoryC>().weaponEquip].iconSprite;
			weaponIcons.GetComponent<Image>().color = db.equipment[player.GetComponent<InventoryC>().weaponEquip].spriteColor;
		}
		if(armorIcons){
			armorIcons.GetComponent<Image>().sprite = db.equipment[player.GetComponent<InventoryC>().armorEquip].iconSprite;
			armorIcons.GetComponent<Image>().color = db.equipment[player.GetComponent<InventoryC>().armorEquip].spriteColor;
		}
		if(accIcons){
			accIcons.GetComponent<Image>().sprite = db.equipment[player.GetComponent<InventoryC>().accessoryEquip].iconSprite;
			accIcons.GetComponent<Image>().color = db.equipment[player.GetComponent<InventoryC>().accessoryEquip].spriteColor;
		}
		if(moneyText){
			moneyText.GetComponent<Text>().text = player.GetComponent<InventoryC>().cash.ToString();
		}
	}
	
	public void ShowItemTooltip(int slot){
		if(!tooltip || !player){
			return;
		}
		if(player.GetComponent<InventoryC>().itemSlot[slot] <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.GetComponent<Image>().sprite = db.usableItem[player.GetComponent<InventoryC>().itemSlot[slot]].iconSprite;
		tooltipName.GetComponent<Text>().text = db.usableItem[player.GetComponent<InventoryC>().itemSlot[slot]].itemName;
		
		tooltipText1.GetComponent<Text>().text = db.usableItem[player.GetComponent<InventoryC>().itemSlot[slot]].description;
		tooltipText2.GetComponent<Text>().text = db.usableItem[player.GetComponent<InventoryC>().itemSlot[slot]].description2;
		tooltipText3.GetComponent<Text>().text = db.usableItem[player.GetComponent<InventoryC>().itemSlot[slot]].description3;
		
		tooltip.SetActive(true);
	}
	
	public void ShowEquipmentTooltip(int slot){
		if(!tooltip || !player){
			return;
		}
		if(player.GetComponent<InventoryC>().equipment[slot] <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.GetComponent<Image>().sprite = db.equipment[player.GetComponent<InventoryC>().equipment[slot]].iconSprite;
		tooltipName.GetComponent<Text>().text = db.equipment[player.GetComponent<InventoryC>().equipment[slot]].itemName;
		
		tooltipText1.GetComponent<Text>().text = db.equipment[player.GetComponent<InventoryC>().equipment[slot]].description;
		tooltipText2.GetComponent<Text>().text = db.equipment[player.GetComponent<InventoryC>().equipment[slot]].description2;
		tooltipText3.GetComponent<Text>().text = db.equipment[player.GetComponent<InventoryC>().equipment[slot]].description3;
		
		tooltip.SetActive(true);
	}
	
	public void ShowOnEquipTooltip(int type){
		if(!tooltip || !player){
			return;
		}

		int id = 0;
		if(type == 0){
			id = player.GetComponent<InventoryC>().weaponEquip;
		}
		if(type == 1){
			id = player.GetComponent<InventoryC>().armorEquip;
		}
		if(type == 2){
			id = player.GetComponent<InventoryC>().accessoryEquip;
		}
		
		if(id <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.GetComponent<Image>().sprite = db.equipment[id].iconSprite;
		tooltipName.GetComponent<Text>().text = db.equipment[id].itemName;
		
		tooltipText1.GetComponent<Text>().text = db.equipment[id].description;
		tooltipText2.GetComponent<Text>().text = db.equipment[id].description2;
		tooltipText3.GetComponent<Text>().text = db.equipment[id].description3;
		
		tooltip.SetActive(true);
	}
	
	public void HideTooltip(){
		if(!tooltip){
			return;
		}
		tooltip.SetActive(false);
	}
	
	public void UseItem(int itemSlot){
		if(!player){
			return;
		}
		player.GetComponent<InventoryC>().UseItem(itemSlot);
		ShowItemTooltip(itemSlot);
		
	}
	
	public void EquipItem(int itemSlot){
		if(!player){
			return;
		}
		player.GetComponent<InventoryC>().EquipItem(player.GetComponent<InventoryC>().equipment[itemSlot] , itemSlot);
		ShowEquipmentTooltip(itemSlot);
	}
	
	public void UnEquip(int type){
		if(!player){
			return;
		}
		int id = 0;
		if(type == 0){
			id = player.GetComponent<InventoryC>().weaponEquip;
		}
		if(type == 1){
			id = player.GetComponent<InventoryC>().armorEquip;
		}
		if(type == 2){
			id = player.GetComponent<InventoryC>().accessoryEquip;
		}
		player.GetComponent<InventoryC>().UnEquip(id);
		ShowOnEquipTooltip(type);
	}
	
	public void CloseMenu(){
		Time.timeScale = 1.0f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		gameObject.SetActive(false);
	}
	
	public void OpenUsableTab(){
		usableTab.SetActive(true);
		equipmentTab.SetActive(false);
	}
	
	public void OpenEquipmentTab(){
		usableTab.SetActive(false);
		equipmentTab.SetActive(true);
	}
	
	
}