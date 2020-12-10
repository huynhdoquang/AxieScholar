using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net.HungryBug.Core.Attribute;
using TMPro;
using Net.HungryBug.Core.UI;
using Net.HungryBug.Core.UI.ReusableCollection;
using GraphQL;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using Zenject;
using Net.HungryBug.Galaxy.Network;
using Net.HungryBug.Core.DI;
using Net.HungryBug.Galaxy.Data;

public class TraineeViewData : BaseCellData
{
    public string EthAdress;

    public TraineeViewData(string ethAdress)
    {
        this.EthAdress = ethAdress;
    }
}

public class TraineeView : UICell
{
    [Inject] protected readonly IGalaxyConfig Config;
    [Inject] protected readonly IGalaxyServer server;
    [Inject] protected readonly IMemoryStore memoryStore;
    [Inject] protected readonly ITestHttpService testHttpService;

    [UIOutlet("@[Text] TraineeName")]
    [SerializeField] private TextMeshProUGUI txtTraineeName;
    [UIOutlet("@[Text] ETHAdress")]
    [SerializeField] private TextMeshProUGUI txtTraineeETHadress;

    [UIOutlet("@[Text] SLP_value")]
    [SerializeField] private TextMeshProUGUI txtSLP;

    [UIOutlet("@[Text] USD_value")]
    [SerializeField] private TextMeshProUGUI txtUSD;

    [UIOutlet("@[Text] ETH_value")]
    [SerializeField] private TextMeshProUGUI txtETH;

    [UIOutlet("@[RawImage] Axie")]
    [SerializeField] private UIWebImage axieImagePrefab;

    [UIOutlet("@[Button] Copy2Clipboard")]
    [SerializeField] private UIButton btnCopy2Clipboard;
    [UIOutlet("@[Button] WinRate")]
    [SerializeField] private UIButton btnCheckWinRate;

    private string query_GetPublicAdress =
             @"query GetProfileNameByEthAddress($ethereumAddress: String!) {
                  publicProfileWithEthereumAddress(ethereumAddress: $ethereumAddress) {
                    accountId
                    name
                    __typename
                  }
                }";


    private void Awake()
    {
        if (this.Config == null) GlobalApp.Inject(this);

        this.btnCopy2Clipboard.onClick.AddListener(this.OnClickClipBoard);
        this.btnCheckWinRate.onClick.AddListener(this.OnClickCheckWinRate);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        this.btnCopy2Clipboard.onClick.RemoveListener(this.OnClickClipBoard);
        this.btnCheckWinRate.onClick.RemoveListener(this.OnClickCheckWinRate);
    }

    private void OnClickCheckWinRate()
    {
        var data = this.DataContext as TraineeViewData;
        if (data != null) {
            string url = "https://axie.zone/profile?eth_addr=" + data.EthAdress;
            Application.OpenURL(url);
        }

    }
    private void OnClickClipBoard()
    {
        var data = this.DataContext as TraineeViewData;
        if (data != null)
        {
            data.EthAdress.CopyToClipboard();
            this.testHttpService.ShowToolTip("Copied!").WrapErrors();
        }  
    }

    public override void Refresh()
    {
        base.Refresh();

        var data = this.DataContext as TraineeViewData;
        if (data != null)
        {
            this.txtSLP.text = string.Empty;
            this.txtETH.text = string.Empty;
            this.txtUSD.text = string.Empty;

            this.txtTraineeName.text = string.Empty;
            this.txtTraineeETHadress.text = data.EthAdress;

            APIGraphQL.Query(query_GetPublicAdress, new { ethereumAddress = data.EthAdress }, response => {
                QmGetPublicAdress myDeserializedClass = JsonConvert.DeserializeObject<QmGetPublicAdress>(response.Raw);

                if (myDeserializedClass.data.publicProfileWithEthereumAddress != null)
                {
                    var name = myDeserializedClass.data.publicProfileWithEthereumAddress.name;
                    this.txtTraineeName.text = name;
                }
                else
                {
                    this.txtTraineeName.text = "Cannot get username! Please check your input adress: " + data.EthAdress;
                }
            });

            this.GetSLPInInventory(data.EthAdress).WrapErrors();
        }


    }

    async UniTask GetSLPInInventory(string adress)
    {
        var url = string.Format(this.Config.ApiGetInventory, adress);

        var res = await this.server.DoUnAuthGet<ResInventory, SmInventory>(url);

        if(res == null || res.IsSuccess == false)
        {
            this.txtTraineeETHadress.text = "Cannot get inventory! Please check your input adress: " + adress;
        }
        else
        {
            foreach (var item in res.Object.items)
            {
                if (item.item_id == 1) //slp
                {
                    this.txtSLP.text = item.total.ToString();

                    var eth = (double)item.total * this.memoryStore.SLP_ExchangePrice_ETH;
                    this.txtETH.text = eth.ToString();

                    var usd = (double)item.total * this.memoryStore.SLP_ExchangePrice_USD;
                    this.txtUSD.text = usd.ToString();

                    break;
                }
            }
        }
    }
}
