using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class Manager : MonoBehaviour {

    public bool started, TimeMode;
    public GameObject nowplaying;
    public Slider pitch_slider;
    public Text TimeText, BPMText;
    public AnimationClip faceanim;
    public float total_time;

    GameObject[] loops;
    GameObject[,] livelli;
    int[] indici;
    Text loop_name;
    float time;
    int counter, mins, idx_c, lvs;

    void Awake() {
        lvs = 5;
        livelli = new GameObject[lvs, 16];
        nowplaying = null;
        started = false;
        loops = GameObject.FindGameObjectsWithTag("Loop");
        SetIds();
        indici = new int[loops.Length];
        setLevels();
        idx_c = 0;
        total_time = PlayerPrefs.GetFloat("TotalPractice");
    }

    void Start() {
        TimeText.text = "0h0m0s";
        time = 0;
        loop_name = GameObject.FindGameObjectWithTag("Loop Name").GetComponent<Text>();
        loop_name.text = " ";
        Application.runInBackground = true;
        TimeMode = false;
    }

    public void SetIds() {
        for (int i=0; i<loops.Length; i++) {
            loops[i].GetComponent<ButtonScript>().ID = i;
        }
    }

    void setLevels() {
        for (int i=0; i<lvs; i++) {
            int maxidx = 0;
            for (int j = 0; j < loops.Length; j++) {
                maxidx++;
                if (maxidx == 16) {
                    maxidx = 0;
                }
                if (loops[j].GetComponent<ButtonScript>().level == i+1) {
                    livelli[i, maxidx] = loops[j];
                }
            }
        }

        for (int i=0; i<lvs; i++) {
            print(i + ":" );
            for (int j=0; j<16; j++) {
                if (livelli[i,j] != null) {
                    print(livelli[i, j].GetComponent<ButtonScript>().level);
                } else {
                    print(0);
                }
            }
            print("\n");
        }
    }

    void Update() {

        if (nowplaying != null) {
            time += Time.deltaTime;
            total_time += Time.deltaTime;
            string hours = ((int)time / 3600).ToString();
            string minutes = (((int)time / 60) % 60).ToString();
            string seconds = ((int)time % 60).ToString();
            TimeText.text = hours + "h" + minutes + "m" + seconds + "s";
            nowplaying.GetComponent<AudioSource>().pitch = pitch_slider.value;
            lockSlidervalue();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Random_Play();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Stop();
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            if (nowplaying != null && nowplaying.GetComponent<AudioSource>().volume != 0) {
                nowplaying.GetComponent<AudioSource>().volume = 0;
            } else {
                nowplaying.GetComponent<AudioSource>().volume = 1;
            }
        }
    }

    void TimeModeF() {
        counter++;
        print(counter / 60);

        if (counter == 1) {
            Random_Play();
        }

        if (counter % (60 * (60 * time)) == 0) {
            print(Time.time);
            Random_Play();
        }
    }

    void lockSlidervalue() {
        if (pitch_slider.value > 0.88f && pitch_slider.value < 0.92f) {
            pitch_slider.value = 0.9f;
        }

        if (pitch_slider.value > 0.98f && pitch_slider.value < 1.02f) {
            pitch_slider.value = 1;
        }

        if (pitch_slider.value > 1.08f && pitch_slider.value < 1.12f) {
            pitch_slider.value = 1.1f;
        }

        if (nowplaying != null) {
            BPMText.text = (nowplaying.GetComponent<ButtonScript>().bpm * pitch_slider.value).ToString("F1");
        }

    }

    public void Random_Play() {
        int random_idx = Random.Range(0, loops.Length);             // Genero numero casuale tra 0 (incluso) e la lunghezza dell'array dei beat (escluso)
        if (idx_c > 0 && idx_c < loops.Length - 1) {
            while (ArrayContains(random_idx)) {                     // finché il numero generato è stato già generato
                random_idx = Random.Range(0, loops.Length);         // genero un numero casuale
            }
        }

        if (nowplaying != null) {                                   // se c'è già un beat in riproduzione
            Stop();                                                 // stoppa il beat
        }

        nowplaying = loops[random_idx];                             // assegno il beat con l'indice generato
        Play(random_idx);                                           // riproduco il beat pseduo-casuale
    }

    bool ArrayContains(int n) {
        for (int i=0; i<idx_c; i++) {                               // ciclo for per scorrimento array indici fino a indice stack
            if (n == indici[i]) {                                   // se il valore passato è già presente nell'array
                return true;                                        // ritorna vero
            }
        }
        return false;                                               // altrimenti ritorna falso
    }

    public void Play(int id) {

        if (ArrayContains(id)) {
            idx_c = 0;
            indici = new int[loops.Length];
            indici[idx_c] = id;
        }

        if (idx_c > 0 && idx_c < loops.Length - 1) {              
            indici[idx_c] = id;
        } else {                                    
            idx_c = 0;
            indici = new int[loops.Length];
            indici[idx_c] = id;
        }
                                  
        idx_c++;

        nowplaying.transform.Find("OuterCircle").GetComponent<Animation>().Play();
        pitch_slider.value = 1;
        nowplaying.GetComponent<AudioSource>().Play();
        loop_name.text = nowplaying.gameObject.name;
        BPMText.text = nowplaying.GetComponent<ButtonScript>().bpm.ToString("F1");
    }

    public void Stop() {
        if (nowplaying != null) {
            nowplaying.GetComponent<AudioSource>().Stop();
            nowplaying.transform.Find("OuterCircle").GetComponent<Animation>().Stop();
            nowplaying.transform.Find("OuterCircle").GetComponent<Image>().fillAmount = 1f;
            BPMText.text = "";
            nowplaying = null;
            loop_name.text = "";
        }
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetFloat("TotalPractice", total_time);
    }

    public void Close() {
        Application.Quit();
    }

}
