using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Shared;
using LitJson;

public class LoginManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> SubPanelList = new List<GameObject>();

    [SerializeField] private List<Toggle> AvatarGroup = new List<Toggle>();

    [SerializeField] private InputField Login_NicknameTxt;
    [SerializeField] private InputField Login_PwdTxt;

    [SerializeField] private InputField SignUp_NicknameTxt;
    [SerializeField] private InputField SignUp_EmailTxt;
    [SerializeField] private InputField SignUp_PwdTxt;
    [SerializeField] private InputField SignUp_ConfirmPwdTxt;

    [SerializeField] private InputField Forgot_EmailTxt;
    [SerializeField] private InputField Forgot_VCodeTxt;

    [SerializeField] private InputField ChangePwd_NewPwdTxt;
    [SerializeField] private InputField ChangePwd_ConfirmTxt;

    [SerializeField] private Image MyAvatar;

    [SerializeField] private List<Sprite> AvatarList = new List<Sprite>();

    [SerializeField] private GameObject QuitPanel;


    public GameObject m_loading;

    private string stVCode = "";

    private int AvatarID = 0;


    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < 13; i++)
        {
            Sprite temp = Resources.Load("Sprites/Avatar/avatar" + i.ToString(), typeof(Sprite)) as Sprite;
            AvatarList.Add(temp);
        }

        QuitPanel = GameObject.Find("UI2").transform.Find("20-QuitPanel").gameObject;

        InitInputField();

        ShowPanelInternal(0);
        OptionManager.instance.PlayLoginBG();
    }

    void InitInputField()
    {
        if (PlayerPrefs.HasKey("MyNickName"))
        {
            Login_NicknameTxt.text = PlayerPrefs.GetString("MyNickName");
        }
        else
        {
            Login_NicknameTxt.text = "";
        }

        if (PlayerPrefs.HasKey("MyPassword"))
        {
            Login_PwdTxt.text = PlayerPrefs.GetString("MyPassword");
        }
        else
        {
            Login_PwdTxt.text = "";
        }

        if (PlayerPrefs.HasKey("MyAvatar"))
        {
            AvatarID = PlayerPrefs.GetInt("MyAvatar");
        }
        else
        {
            AvatarID = 0;
        }
    }

    public void ShowPanel(int iIndex)
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        ShowPanelInternal(iIndex);
    }

    private void ShowPanelInternal(int index)
    {
        for (int i = 0; i < SubPanelList.Count; i++)
        {
            if (i == index)
                SubPanelList[i].SetActive(true);
            else
                SubPanelList[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowQuitPanel();
        }
    }

    public void ShowQuitPanel()
    {
        QuitPanel.SetActive(true);
    }

    public void FBLoginBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void TwitterLoginBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void AvatarSelectBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        for (int i = 0; i < AvatarGroup.Count; i++)
        {
            if (AvatarGroup[i].isOn)
            {
                AvatarID = i + 1;
            }
        }

        MyAvatar.sprite = AvatarList[AvatarID];

        ShowPanel(1);
    }

    public void LoginBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (CoreApp.GetInstance.IsInternetConnection(true))
        {
            if (isValidLoginData())
            {
                m_loading.SetActive(true);
                StartCoroutine(UserSignIn());
            }
        }
    }

    public void SignUpBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (CoreApp.GetInstance.IsInternetConnection(true))
        {
            if (isValidSignupData())
            {
                m_loading.SetActive(true);
                StartCoroutine(UserSignUp());
            }
        }
    }

    public void ConfirmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (isValidForgotPwdData())
        {
            ShowPanel(4);
        }
    }

    public void ChangePassBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (isValidConfirmPwdData())
        {
            StartCoroutine(ChangePassword());
        }
    }

    public void GetVCodeBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (CoreApp.GetInstance.IsInternetConnection(true))
        {
            if (isValidSendVerificationData())
            {
                m_loading.SetActive(true);
                StartCoroutine(UserForgotPwd());
            }
        }
    }

    public void GuestBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (CoreApp.GetInstance.IsInternetConnection(true))
        {
            m_loading.SetActive(true);

            if (PlayerPrefs.HasKey("isGuest"))
            {
                StartCoroutine(GuestSignIn(PlayerPrefs.GetString("GuestName")));
            }
            else
            {
                StartCoroutine(GuestSignUp());
            }
        }
    }

    IEnumerator UserSignIn()
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserLogin;
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("username", Login_NicknameTxt.text);
        param.Add("password", Login_PwdTxt.text);

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            Debug.Log("Login: " + www.text);
            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {

                GB.g_MyID = int.Parse(json["result"]["user_id"].ToString());
                GB.g_MyNickname = Login_NicknameTxt.text;
                GB.g_MyMail = json["result"]["mail"].ToString();
                GB.g_MyGems = int.Parse(json["result"]["gems"].ToString());
                GB.g_MyRank = int.Parse(json["result"]["rank"].ToString());
                GB.g_MyWin = int.Parse(json["result"]["win"].ToString());
                GB.g_MyLoss = int.Parse(json["result"]["loss"].ToString());
                GB.g_MyAvatarID = int.Parse(json["result"]["avatar_id"].ToString());

                PlayerPrefs.SetString("MyNickName", Login_NicknameTxt.text);
                PlayerPrefs.SetString("MyPassword", Login_PwdTxt.text);

                PlayerPrefs.DeleteKey("isGuest");
                PlayerPrefs.DeleteKey("GuestName");

                StartCoroutine(LoadMainScene());

            }
            else if (status == "fail")
            {
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }

    IEnumerator UserSignUp()
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserRegister;
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("username", SignUp_NicknameTxt.text);
        param.Add("email", SignUp_EmailTxt.text);
        param.Add("password", SignUp_PwdTxt.text);
        param.Add("avatar_id", AvatarID.ToString());

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            m_loading.SetActive(false);
            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {
                MobileNative.Alert("", GB.g_FSignUpSuccess, "OK");
                ShowPanel(0);
            }
            else
            {
                MobileNative.Alert("Error", json["status"].ToString(), "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }

    IEnumerator GuestSignUp()
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserRegister;
        Dictionary<string, string> param = new Dictionary<string, string>();

        string NickName = "";
        string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        int charAmount = UnityEngine.Random.Range(2, 7);
        for (int i = 0; i < charAmount; i++)
        {
            NickName += characters[UnityEngine.Random.Range(0, characters.Length)];
        }

        param.Add("username", "Guest_" + NickName);
        param.Add("email", NickName + "@spam");
        param.Add("password", "");
        param.Add("avatar_id", "0");

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            m_loading.SetActive(false);
            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {

                StartCoroutine(GuestSignIn("Guest_" + NickName));

                PlayerPrefs.SetInt("isGuest", 1);
                PlayerPrefs.SetString("GuestName", "Guest_" + NickName);
            }
            else
            {
                MobileNative.Alert("Error", json["status"].ToString(), "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }

    IEnumerator GuestSignIn(string NickName)
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserLogin;
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("username", NickName);
        param.Add("password", "");

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {

                GB.g_MyID = int.Parse(json["result"]["user_id"].ToString());
                GB.g_MyNickname = NickName;
                GB.g_MyMail = json["result"]["mail"].ToString();
                GB.g_MyGems = int.Parse(json["result"]["gems"].ToString());
                GB.g_MyRank = int.Parse(json["result"]["rank"].ToString());
                GB.g_MyWin = int.Parse(json["result"]["win"].ToString());
                GB.g_MyLoss = int.Parse(json["result"]["loss"].ToString());
                GB.g_MyAvatarID = int.Parse(json["result"]["avatar_id"].ToString());

                PlayerPrefs.SetString("MyNickName", Login_NicknameTxt.text);
                PlayerPrefs.SetString("MyPassword", Login_PwdTxt.text);

                StartCoroutine(LoadMainScene());

            }
            else if (status == "fail")
            {
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }

    IEnumerator UserForgotPwd()
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserForgotPwd;
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("email", Forgot_EmailTxt.text);

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            m_loading.SetActive(false);
            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {

                stVCode = json["result"].ToString();

                MobileNative.Alert("", GB.g_FVCodeSent, "OK");
            }
            else
            {
                MobileNative.Alert("Error", "The user email is not in the server.", "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }

    IEnumerator ChangePassword()
    {
        string strUrl = GB.g_BASE_URL + GB.g_APIUserChangePwd;
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("email", Forgot_EmailTxt.text);
        param.Add("password", ChangePwd_NewPwdTxt.text);

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in param)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(strUrl, form);
        yield return www;

        if (www.error == null)
        {

            m_loading.SetActive(false);
            JsonData json = JsonMapper.ToObject(www.text);

            string status = json["status"].ToString();
            if (status == "success")
            {

                PlayerPrefs.SetString("MyPassword", ChangePwd_NewPwdTxt.text);

                MobileNative.Alert("", GB.g_FChangePwdSuccess, "OK");
                ShowPanel(0);
            }
            else
            {
                MobileNative.Alert("Error", json["status"].ToString(), "OK");
            }
        }
        else
        {
            MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
        }

        m_loading.SetActive(false);
    }


    bool isValidLoginData()
    {
        if (Login_NicknameTxt.text == "" && Login_PwdTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FLoginFailedEmpty, "OK");
            return false;

        }
        else if (Login_NicknameTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FEmailEmpty, "OK");
            return false;

        }
        else if (Login_PwdTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FPasswordEmpty, "OK");
            return false;
        }

        return true;
    }

    bool isValidSignupData()
    {
        if (SignUp_NicknameTxt.text == "" && SignUp_EmailTxt.text == "" && SignUp_PwdTxt.text == "" && SignUp_ConfirmPwdTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FSignUpFailedEmpty, "OK");
            return false;

        }
        else if (SignUp_NicknameTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FNicknameEmpty, "OK");
            return false;

        }
        else if (SignUp_EmailTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FEmailEmpty, "OK");
            return false;

        }
        else if (!CoreApp.GetInstance.isValidEmail(SignUp_EmailTxt.text))
        {

            MobileNative.Alert("", GB.g_FEmailInvalid, "OK");
            return false;

        }
        else if (SignUp_PwdTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FPasswordEmpty, "OK");
            return false;

        }
        else if (SignUp_PwdTxt.text != SignUp_ConfirmPwdTxt.text)
        {

            MobileNative.Alert("", GB.g_FPasswordAlert, "OK");
            return false;
        }

        return true;
    }

    bool isValidSendVerificationData()
    {
        if (Forgot_EmailTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FEmailEmpty, "OK");
            return false;

        }
        else if (!CoreApp.GetInstance.isValidEmail(Forgot_EmailTxt.text))
        {

            MobileNative.Alert("", GB.g_FEmailInvalid, "OK");
            return false;

        }

        return true;
    }

    bool isValidForgotPwdData()
    {
        if (Forgot_EmailTxt.text == "" && Forgot_VCodeTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FSignUpFailedEmpty, "OK");
            return false;

        }
        else if (Forgot_EmailTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FEmailEmpty, "OK");
            return false;

        }
        else if (!CoreApp.GetInstance.isValidEmail(Forgot_EmailTxt.text))
        {

            MobileNative.Alert("", GB.g_FEmailInvalid, "OK");
            return false;

        }
        else if (Forgot_VCodeTxt.text != stVCode)
        {

            MobileNative.Alert("", GB.g_FVCodeAlert, "OK");
            return false;
        }

        return true;
    }

    bool isValidConfirmPwdData()
    {
        if (ChangePwd_NewPwdTxt.text == "" && ChangePwd_ConfirmTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FPasswordEmpty, "OK");
            return false;

        }
        else if (ChangePwd_NewPwdTxt.text == "")
        {

            MobileNative.Alert("", GB.g_FPasswordEmpty, "OK");
            return false;

        }
        else if (ChangePwd_NewPwdTxt.text != ChangePwd_ConfirmTxt.text)
        {

            MobileNative.Alert("", GB.g_FPasswordAlert, "OK");
            return false;

        }

        return true;
    }

    IEnumerator LoadMainScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("03-Main");
        SceneLoadingBar.instance.Show();

        while (!asyncOperation.isDone)
        {
            SceneLoadingBar.instance.SetProgressBar(asyncOperation.progress);
            yield return null;
        }
    }
}
