using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {

	public GameObject diskPrefab;

	private List<DiskObj> used = new List<DiskObj>();
	private List<DiskObj> free = new List<DiskObj>();

	private void Awake()
	{
		diskPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
		diskPrefab.SetActive(false);
	}

	public GameObject GetDisk(int round)
	{
		GameObject newDisk = null;
		if (free.Count > 0)
		{
			newDisk = free[0].gameObject;
			free.Remove(free[0]);
		}
		else
		{
			newDisk = GameObject.Instantiate<GameObject>(diskPrefab, Vector3.zero, Quaternion.identity);
			newDisk.AddComponent<DiskObj>();
		}

		int start = 0;
		if (round == 1) start = 100;
		if (round == 2) start = 250;
		int selectedColor = Random.Range(start, round * 499);

		if (selectedColor > 500)
		{
			round = 2;
		}
		else if (selectedColor > 300)
		{
			round = 1;
		}
		else
		{
			round = 0;
		}

		switch (round)
		{

		case 0:
			{
				newDisk.GetComponent<DiskObj>().color = Color.yellow;
				newDisk.GetComponent<DiskObj>().speed = 4.0f;
				float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
				newDisk.GetComponent<DiskObj>().direction = new Vector3(RanX, 1, 0);
				newDisk.GetComponent<Renderer>().material.color = Color.yellow;
				break;
			}
		case 1:
			{
				newDisk.GetComponent<DiskObj>().color = Color.red;
				newDisk.GetComponent<DiskObj>().speed = 6.0f;
				float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
				newDisk.GetComponent<DiskObj>().direction = new Vector3(RanX, 1, 0);
				newDisk.GetComponent<Renderer>().material.color = Color.red;
				break;
			}
		case 2:
			{
				newDisk.GetComponent<DiskObj>().color = Color.green;
				newDisk.GetComponent<DiskObj>().speed = 8.0f;
				float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
				newDisk.GetComponent<DiskObj>().direction = new Vector3(RanX, 1, 0);
				newDisk.GetComponent<Renderer>().material.color = Color.black;
				break;
			}
		}

		used.Add(newDisk.GetComponent<DiskObj>());
		//newDisk.SetActive(true);
		newDisk.name = newDisk.GetInstanceID().ToString();
		return newDisk;
	}

	public void FreeDisk(GameObject disk)
	{
        DiskObj tmp = null;
		foreach (DiskObj i in used)
		{
			if (disk.GetInstanceID() == i.gameObject.GetInstanceID())
			{
				tmp = i;
			}
		}
		if (tmp != null) {
			tmp.gameObject.SetActive(false);
			free.Add(tmp);
			used.Remove(tmp);
		}
	}
}
