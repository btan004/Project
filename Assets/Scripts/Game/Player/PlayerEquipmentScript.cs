using UnityEngine;
using System.Collections;

public class PlayerEquipmentScript : MonoBehaviour {

	public int ActiveWeapon;
	public GameObject[] Weapons;

	public int ActiveShield;
	public GameObject[] Shields;

	void Update()
	{
		//make sure the active weapon and shield are active (in the case that they have been changed)
		if (!Weapons[ActiveWeapon].activeInHierarchy)
		{
			Debug.Log("Need to active the correct weapon...");

			//disable all weapons, and active the correct one
			for (int i = 0; i < Weapons.Length; i++)
				if (i == ActiveWeapon)
					Weapons[i].SetActive(true);
				else
					Weapons[i].SetActive(false);
		}

		if (!Shields[ActiveShield].activeInHierarchy)
		{
			Debug.Log("Need to active the correct shield...");

			//disable all weapons, and active the correct one
			for (int i = 0; i < Shields.Length; i++)
				if (i == ActiveShield)
					Shields[i].SetActive(true);
				else
					Shields[i].SetActive(false);
		}
	}

	public void ChangeToWeapon(int weaponNumber)
	{
		//deactivate the current weapon
		Weapons[ActiveWeapon].SetActive(false);

		//set our new active index
		ActiveWeapon = weaponNumber;

		//active our new weapon
		Weapons[ActiveWeapon].SetActive(true);
	}

	public void UpgradeWeapon()
	{
		if (ActiveWeapon < Weapons.Length)
			ChangeToWeapon(ActiveWeapon + 1);
	}

	public void ChangeToShield(int shieldNumber)
	{
		//deactive the current shield
		Shields[ActiveShield].SetActive(false);

		//set our new active index
		ActiveShield = shieldNumber;

		//activate our new shield
		Shields[ActiveShield].SetActive(true);
	}

	public void UpgradeShield()
	{
		if (ActiveShield < Shields.Length)
			ChangeToShield(ActiveShield + 1);
	}
}
