using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeRefresh : MonoBehaviour {

    Manager mng;

	// Use this for initialization
	void Start () {
        mng = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
	}

    public void Up() {
        gameObject.GetComponent<Animation>().Play("UpTimeBox");
    }

    public void Down() {
        gameObject.GetComponent<Animation>().Play("DownTimeBox");
    }

    // Update is called once per frame
    void Update () {
        string hours = ((int)mng.total_time / 3600).ToString();
        string minutes = (((int)mng.total_time / 60) % 60).ToString();
        string seconds = ((int)mng.total_time % 60).ToString();
        GetComponent<Transform>().Find("Text").GetComponent<Text>().text = hours + "h" + minutes + "m" + seconds + "s";
    }
}
