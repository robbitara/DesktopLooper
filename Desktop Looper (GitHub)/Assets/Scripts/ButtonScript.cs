using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    Manager mng;
    public int ID, level, bpm;
    GameObject face;
    Vector2 default_face_size;

    // Use this for initialization
    void Start () {
        face = GetComponent<Transform>().Find("Face").gameObject;
        default_face_size = face.GetComponent<RectTransform>().sizeDelta;
        mng = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        face = transform.Find("Face").gameObject;
        face.AddComponent<Animation>();
        face.GetComponent<Animation>().playAutomatically = false;
        face.GetComponent<Animation>().AddClip(mng.faceanim, "FaceAnimation");
    }

    public void OnPointerClick(PointerEventData ped) {
        if (mng.nowplaying != null) {
            mng.Stop();
        }

        mng.nowplaying = gameObject;
        mng.Play(ID);
    }

    public void OnPointerEnter(PointerEventData ped) {
        if (!face.GetComponent<Animation>().IsPlaying("FaceAnimation")) {
            face.GetComponent<RectTransform>().sizeDelta *= 1.2f;
        }
    }

    public void OnPointerExit(PointerEventData ped) {
        face.GetComponent<RectTransform>().sizeDelta = default_face_size;
    }
}
