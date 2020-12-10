using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Net.HungryBug.Core.UI;
using Zenject;
using Net.HungryBug.Galaxy.Network;
using Net.HungryBug.Core.DI;
using Net.HungryBug.Core.UI.ReusableCollection;
using Net.HungryBug.Galaxy.UI.Shared;
using Cysharp.Threading.Tasks;

public class CardViewData : BaseCellData
{
    public SmCardAbilities SmCardAbilities;

    public CardViewData(SmCardAbilities smCardAbilities)
    {
        this.SmCardAbilities = smCardAbilities;
    }
}

public class CardView : UICell
{
    [Inject] protected readonly IGalaxyConfig Config;

    [SerializeField] private TextMeshProUGUI txtSkillName;
    [SerializeField] private TextMeshProUGUI txtCardDes;

    [SerializeField] private TextMeshProUGUI txtCardAttack;
    [SerializeField] private TextMeshProUGUI txtCardDefense;
    [SerializeField] private TextMeshProUGUI txtCardEnergy;

    [SerializeField] private UIWebImage cardImage;

    [SerializeField] private UIStateNotifyView uIStateNotifyView;

    private void Awake()
    {
        if (this.Config == null) GlobalApp.Inject(this);
    }

    public override void Refresh()
    {
        base.Refresh();

        var data = this.DataContext as CardViewData;
        if (data != null)
        {
            this.SetData(data.SmCardAbilities).WrapErrors();
        }
    }

    public async UniTask SetData(SmCardAbilities smCardAbilities)
    {
        this.txtSkillName.text = smCardAbilities.skillName;
        this.txtCardDes.text = smCardAbilities.description;

        this.txtCardAttack.text = smCardAbilities.defaultAttack.ToString();
        this.txtCardDefense.text = smCardAbilities.defaultDefense.ToString();
        this.txtCardEnergy.text = smCardAbilities.defaultEnergy.ToString();

        this.uIStateNotifyView.EnableLoading(true);
        var imgLink = string.Format(this.Config.ApiGetCardImage, smCardAbilities.id);
        var isSuccess = await this.cardImage.RefreshImage(imgLink, $"{smCardAbilities.id}.png");

        if (isSuccess == true)
        {
            this.uIStateNotifyView.EnableLoading(false);
        }
        else
        {
            this.uIStateNotifyView.EnableError();
        }
    }
}
