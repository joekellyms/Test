using UnityEngine;
using UnityEngine.VR.WSA.Input;
//using UnityEngine.Windows.Speech;

public class DelayedFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followObject;
    [SerializeField]
    private int frameDelay = 0;
    [SerializeField]
    private AudioSource chimeSound;
    [SerializeField]
    private TextMesh numberText;

    private Vector3[] positionFrames;
    private Quaternion[] rotationFrames;
    private int currentFrameCapture = 0;
    private int currentFrameUsed = 0;
    private bool follow = false;
    private GestureRecognizer gestures;
    //private KeywordRecognizer keywords;

    private void Start()
    {
        SetGestureCommands();
        ResetFrames();
	}
	
	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnTap(new InteractionSourceKind(), 1, new Ray());
        }
        if (this.frameDelay <= 0)
        {
            return;
        }

        if (this.currentFrameCapture >= this.positionFrames.Length)
        {
            this.currentFrameCapture = 0;
            if (!this.follow)
            {
                this.follow = true;
            }
        }

        if (this.follow)
        {
            if (this.currentFrameUsed >= this.positionFrames.Length)
            {
                this.currentFrameUsed = 0;
            }
            this.transform.position = this.positionFrames[this.currentFrameUsed];
            this.transform.rotation = this.rotationFrames[this.currentFrameUsed];
            this.currentFrameUsed++;
        }

        this.positionFrames[this.currentFrameCapture] = this.followObject.position;
        this.rotationFrames[this.currentFrameCapture] = this.followObject.rotation;
        this.currentFrameCapture++;
	}

    [ContextMenu("Reset Frames")]
    public void ResetFrames()
    {
        this.follow = false;
        this.currentFrameCapture = 0;
        this.currentFrameUsed = 0;
        this.positionFrames = null;
        this.rotationFrames = null;
        if (this.frameDelay > 0)
        {
            this.positionFrames = new Vector3[this.frameDelay];
            this.rotationFrames = new Quaternion[this.frameDelay];
        }
        this.numberText.text = this.frameDelay.ToString();
    }

    private void SetGestureCommands()
    {
        this.gestures = new GestureRecognizer();
        this.gestures.SetRecognizableGestures(GestureSettings.Tap);
        this.gestures.TappedEvent += OnTap;
        this.gestures.StartCapturingGestures();
    }

    private void OnTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        RaycastHit tempHit;
        if (Physics.Raycast(this.followObject.position, Vector3.forward, out tempHit, 5))
        {
            this.chimeSound.Play();
            string hitName = tempHit.collider.gameObject.name;
            if (hitName == "Increase")
            {
                this.frameDelay++;
                ResetFrames();
            }
            else if (hitName == "Decrease")
            {
                this.frameDelay--;
                ResetFrames();
            }
        }
    }

    //private void SetSpeechCommands()
    //{
    //    string[] keywordList = new string[4];
    //    keywordList[0] = "Up";
    //    keywordList[1] = "Down";
    //    this.keywords = new KeywordRecognizer(keywordList);
    //    this.keywords.OnPhraseRecognized += this.SpeechCommandRecognized;
    //    this.keywords.Start();
    //}

    //private void SpeechCommandRecognized(PhraseRecognizedEventArgs args)
    //{
    //    if (args.text == "Up")
    //    {
    //        this.frameDelay++;
    //        ResetFrames();
    //    }
    //    else if (args.text == "Down")
    //    {
    //        this.frameDelay--;
    //        ResetFrames();
    //    }
    //}
}