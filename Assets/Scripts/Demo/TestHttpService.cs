using Cysharp.Threading.Tasks;
using Net.HungryBug.Core.DI;
using Net.HungryBug.Galaxy.Data;
using Net.HungryBug.Galaxy.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using System;
using Net.HungryBug.Core.UI.ReusableCollection;
using Net.HungryBug.Core;
using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Network;
using Net.HungryBug.Core.Attribute;
using Net.HungryBug.Core.UI;
using System.IO;
using UnityEngine.Events;

public interface ITestHttpService
{
    UniTask ShowToolTip(string message, int delayTime = 700);
}


public class TestHttpService : UIResolver, ITestHttpService
{
    [Inject] protected readonly IGalaxyConfig Config;
    [Inject] protected readonly IGalaxyServer server;
    [Inject] protected readonly IUnity engine;
    [Inject] protected readonly IHttpService httpService;
    [Inject] protected readonly IMemoryStore memoryStore;
    [Inject] protected readonly IPlayerprefsHelper playerprefsHelper;

    [SerializeField] private UICollection uICollection_Trainee;

    [UIOutlet("@[Text] ExchangePrice_ETH")]
    [SerializeField] private TextMeshProUGUI txtExchangePrice_ETH;
    [UIOutlet("@[Text] ExchangePrice_USD")]
    [SerializeField] private TextMeshProUGUI txtExchangePrice_USD;

    [UIOutlet("@[InputField] ETHAdress")]
    [SerializeField] private TMP_InputField inputField_ETHAdress;

    [UIOutlet("@[Button] Add_ETHAdress")]
    [SerializeField] private Button btnAddETHAdress;

    [UIOutlet("@[Button] Import")]
    [SerializeField] private Button btnImport;
    [UIOutlet("@[Button] Export")]
    [SerializeField] private Button btnExport;

    [UIOutlet("@[Text] ToolTip")]
    [SerializeField] private TextMeshProUGUI txtToolTip;


    [UIOutlet("@[Button] openCoinGecko")]
    [SerializeField] private Button btnOpenCoinGecko;
    [UIOutlet("@[Button] Feedback")]
    [SerializeField] private Button btnFeedback;
    [UIOutlet("@[Button] Support")]
    [SerializeField] private Button btnSupport;
    [UIOutlet("@[Button] SupportLoom")]
    [SerializeField] private Button btnSupportLoom;

    [UIOutlet("@[Button] GasCheck")]
    [SerializeField] private Button btnGasCheck;

    [UIOutlet("@[Button] CloseGuide")]
    [SerializeField] private Button btnCloseGuide;
    [UIOutlet("@[Button] OpenGuide")]
    [SerializeField] private Button btnOpenGuide;
    [UIOutlet("@[Panel] Guide")]
    [SerializeField] private GameObject goGuide;

    private List<string> eth_adress_list = new List<string>()
    {
        "0x2becf9685e02f8fd8c1a4caff7659e843a487eca",
        "0xe4734d75fd9de7b8b89be8b43501cdf5d345b2e6",
        "0xc0ca4e3a65abb532bdb480d8bfbc0362c2fe3cd6",
        "0xaf41b73e8d811d442a3167f29c11f2e98f363d8b",
        "0x1196ed903b8193998affd445712a70cdc80e778c",
        "0xceed17c04e16af2dadc483c7c6acae4ea82ae186"
    };


    void Awake()
    {
        this.txtToolTip.transform.parent.gameObject.SetActive(false);
        this.eth_adress_list.Clear();
        this.eth_adress_list = this.playerprefsHelper.LoadListETHAdress();
        if (this.eth_adress_list == null)
            this.eth_adress_list = new List<string>();

        this.Init().WrapErrors();
    }

    private void Start()
    {
        this.btnAddETHAdress.onClick.AddListener(this.OnClickAddAdress);
        this.uICollection_Trainee.OnCellClicked += UICollection_Trainee_OnCellClicked;

        this.btnImport.onClick.AddListener(this.OnClickImport);
        this.btnExport.onClick.AddListener(this.OnClickExport);

        this.btnOpenCoinGecko.onClick.AddListener(this.openCoinGecko);
        this.btnSupport.onClick.AddListener(this.onClickSupport);
        this.btnSupportLoom.onClick.AddListener(this.onClickSupportLoom);
        this.btnFeedback.onClick.AddListener(this.SendEmail);

        this.btnGasCheck.onClick.AddListener(this.OnClickGasCheck);

        this.btnCloseGuide.onClick.AddListener(this.OnClickCloseGuide);
        this.btnOpenGuide.onClick.AddListener(this.OnClickOpenGuide);
    }

    private void OnDestroy()
    {
        this.btnAddETHAdress.onClick.RemoveListener(this.OnClickAddAdress);
        this.uICollection_Trainee.OnCellClicked -= UICollection_Trainee_OnCellClicked;

        this.btnImport.onClick.RemoveListener(this.OnClickImport);
        this.btnExport.onClick.RemoveListener(this.OnClickExport);

        this.btnOpenCoinGecko.onClick.RemoveListener(this.openCoinGecko);
        this.btnSupport.onClick.RemoveListener(this.onClickSupport);
        this.btnSupportLoom.onClick.RemoveListener(this.onClickSupportLoom);
        this.btnFeedback.onClick.RemoveListener(this.SendEmail);

        this.btnGasCheck.onClick.RemoveListener(this.OnClickGasCheck);

        this.btnCloseGuide.onClick.RemoveListener(this.OnClickCloseGuide);
        this.btnOpenGuide.onClick.RemoveListener(this.OnClickOpenGuide);
    }

    private void OnClickOpenGuide()
    {
        this.goGuide.SetActive(true);
    }

    private void OnClickCloseGuide()
    {
        this.goGuide.SetActive(false);
    }

    private void onClickSupportLoom()
    {
        var s = "loom:bc35cae8d6bf8f6ad56ec59228d66839b2e25725";
        s.CopyToClipboard();

        this.ShowToolTip($"Copied Loom Adress!").WrapErrors();
    }

    private void OnClickGasCheck()
    {
        Application.OpenURL("https://etherscan.io/gastracker");
    }

    private void onClickSupport()
    {
        var s = "0xAF41B73E8D811d442a3167f29c11f2e98F363d8B";
        s.CopyToClipboard();

        this.ShowToolTip($"Copied ETH Adress!").WrapErrors();
    }

    private void openCoinGecko()
    {
        Application.OpenURL("https://www.coingecko.com/en/coins/small-love-potion");
    }

    private void OnClickExport()
    {
        // Create a dummy text file
        string filePath = Path.Combine(Application.temporaryCachePath, "test.csv");

        var s = this.playerprefsHelper.ExportFile(this.eth_adress_list);
        File.WriteAllText(filePath, s);

        // Export the file
        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePath, (success) => { if (success == true) { this.ShowToolTip($"Saved").WrapErrors(); } Debug.Log("File exported: " + success); } );
    }

    private void OnClickImport()
    {
        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.READ_EXTERNAL_STORAGE");
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            // Use MIMEs on Android
            string[] fileTypes = new string[] { "text/csv", "text/*", "text/plain" };

            NativeFilePicker.PickFile((c) => {
                if (string.IsNullOrEmpty(c) == true)
                {
                    //user cancel
                    this.ShowToolTip($"Cancel").WrapErrors();
                }
                else
                {
                    var path = c;

                    //Read the text from directly from the test.txt file
                    StreamReader reader = new StreamReader(path);
                    var content = reader.ReadToEnd();
                    reader.Close();


                    //we read all text in path
                    this.ShowToolTip($"Done!").WrapErrors();

                    var lst = this.playerprefsHelper.ImportFromFile(content);
                    this.eth_adress_list = lst;

                    this.FillData2Collection();

                    //save list
                    this.playerprefsHelper.SaveListETHAdress(this.eth_adress_list);
                }


            }, fileTypes);
        }
        else
        {
            this.ShowToolTip("Permission state: " + result).WrapErrors();
        }

        /*var per = NativeFilePicker.CheckPermission();
        if (per == NativeFilePicker.Permission.Denied)
        {
            this.ShowToolTip($"Permission.Denied").WrapErrors();

            NativeFilePicker.OpenSettings();
            return;
        }*/

        
    }

    public async UniTask ShowToolTip(string message, int delayTime = 700)
    {
        this.txtToolTip.text = message;
        this.txtToolTip.transform.parent.gameObject.SetActive(true);
        await UniTask.Delay(delayTime);
        this.txtToolTip.transform.parent.gameObject.SetActive(false);
    }

    private void UICollection_Trainee_OnCellClicked(UICell arg1, ICellData arg2)
    {
        var cellData = arg2 as TraineeViewData;
        this.eth_adress_list.Remove(cellData.EthAdress);

        this.FillData2Collection();

        //save list
        this.playerprefsHelper.SaveListETHAdress(this.eth_adress_list);
    }

    private void OnClickAddAdress()
    {
        if (string.IsNullOrEmpty(this.inputField_ETHAdress.text) == false)
        {
            //add to list adress
           
            var ethAdress = this.inputField_ETHAdress.text;
            if (this.eth_adress_list.Contains(ethAdress) == false)
            {
                this.eth_adress_list.Add(ethAdress);

                //refresh list 
                this.FillData2Collection();

                //save list
                this.playerprefsHelper.SaveListETHAdress(this.eth_adress_list);
            }
            else
            {
                this.ShowToolTip("adress already add! Pls add another adress!").WrapErrors();
            }
        }
        else
        {
            this.ShowToolTip("adress is empty! Pls add another adress!").WrapErrors();
        }
    }

    void FillData2Collection()
    {
        var cells = new List<TraineeViewData>();
        foreach (var item in this.eth_adress_list)
        {
            var cell = new TraineeViewData(item);
            cells.Add(cell);
        }

        this.uICollection_Trainee.SetData(cells.ToArray());
    }

    async UniTask Init()
    {
        this.ShowToolTip($"get SLP price from https://www.coingecko.com/ ", 10000).WrapErrors();
        this.btnAddETHAdress.interactable = false;
        await this.GetExchangePrice();
        this.btnAddETHAdress.interactable = true;
        this.ShowToolTip($"Done!").WrapErrors();

        this.FillData2Collection();
    }


    async UniTask GetExchangePrice()
    {
        this.txtExchangePrice_ETH.text = string.Empty;
        this.txtExchangePrice_USD.text = string.Empty;

        var url = string.Format(this.Config.ApiGetSLPExchangeCost);

        var res = await this.server.DoUnAuthGet<ResSLPExchangeRate, SmSLPExchangeRate>(url);

        this.memoryStore.SLP_ExchangePrice_ETH = res.Object.SmallLovePotion.eth;
        this.memoryStore.SLP_ExchangePrice_USD = res.Object.SmallLovePotion.usd;

        var eth_format = String.Format("{0:F10}", this.memoryStore.SLP_ExchangePrice_ETH);
        this.txtExchangePrice_ETH.text = eth_format;
        this.txtExchangePrice_USD.text = this.memoryStore.SLP_ExchangePrice_USD.ToString();
    }


    void SendEmail()
    {
        string email = "huynh.axie.01@gmail.com";
        string subject = MyEscapeURL("[Feedback AxieScholar]");
        Application.OpenURL("mailto:" + email + "?subject=" + subject);
    }
    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    #region un-used
    async void GetAllAxies()
    {
        var url = string.Format(this.Config.ApiAllAxies);

        var res = await this.server.DoUnAuthGet<ResAllAxies, SmAllAxies>(url);

        Debug.LogWarning($"{res.Code} | {res.Message} | {res.Object.totalAxies}");
    }

    async void GetSingleAxie()
    {
        var url = string.Format(this.Config.ApiSingleAxie, "48133");

        var res = await this.server.DoUnAuthGet<ResSingleAxie, SmSingleAxie>(url);

        Debug.LogWarning($"{res.Code} | {res.Object.figure.images} | {res.Object.id} ");
    }

    async void GetBodyPart()
    {
        var url = string.Format(this.Config.ApiBodyPart);

        var res = await this.server.DoUnAuthGet<ResBodyPart, SmAllBodyPart>(url);

        foreach (var item in res.Object.MyArray)
        {
            Debug.LogWarning($"{item.partId} | {item.name}");
        }

        this.memoryStore.SmAllBodyPart = res.Object;
    }

    void GetAllCardAbilities()
    {
        var all = this.memoryStore.SmGetAllCardAbilities;

        var cells = new List<CardViewData>();
        foreach (var item in all.CardAbilitiesDict)
        {
            var cell = new CardViewData(item.Value);
            cells.Add(cell);
        }

        //this.uICollection_Cards.SetData(cells.ToArray());
    }
    #endregion
}
