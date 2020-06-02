using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PolluterContent : PopUpPieChartContent
{
    public GameObject objectPieChart = null;

    public WorldWindow worldWindow { get; set; }
    
    [SerializeField]
    private SideTextController sideTextController = null;
    [SerializeField]
    private HealthBar healthBar = null;

    [SerializeField]
    private TextMeshProUGUI textTitle = null;
    [SerializeField]
    private TextMeshProUGUI pieChartTitle = null;
    [SerializeField]
    private TextMeshProUGUI maxPieText = null;

    [Header("Icons")]
    [SerializeField]
    private PolluterIcon factoryIcon = null;
    [SerializeField]
    private PolluterIcon filterIcon = null;
    [SerializeField]
    private PolluterIcon recyclerIcon = null;

    [Header("Economic")]
    [SerializeField]
    private TextMeshProUGUI textCoins = null;
    [SerializeField]
    private TextMeshProUGUI textIncome = null;
    [SerializeField]
    private TextMeshProUGUI textRemoval = null;

    private Color defaultColorText = Color.black;
    
    [Header("Vulnerabilities")]
    [SerializeField]
    private GameObject objectVulnerabilities = null;
    [SerializeField]
    private TextMeshProUGUI textVulnerabilities = null;
    [SerializeField]
    private bool showVulnerabilities = false;

    [Header("Resources")]
    [SerializeField]
    private GameObject objectResources = null;
    [SerializeField]
    private TextMeshProUGUI textResources = null;

    [Header("Recycles")]
    [SerializeField]
    private GameObject objectRecyclers = null;
    [SerializeField]
    private TextMeshProUGUI textRecyclers = null;

    private bool purchaseCheck = false;
    private bool iconCheck = false;

    private Sequence currentSequence = null;

    private void Start()
    {
        defaultColorText = textCoins.color;

        factoryIcon.gameObject.SetActive(false);
        factoryIcon.SetInteractible(false);

        filterIcon.gameObject.SetActive(false);
        filterIcon.SetInteractible(false);

        recyclerIcon.gameObject.SetActive(false);
        recyclerIcon.SetInteractible(false);
    }

    public bool CheckGraphicPolluter(PurchasableIcon purchasableIcon)
    {
        PolluterAttrib attrib = purchasableIcon.GetPolluterAttributes();

        bool check = CheckPolluter(attrib, purchasableIcon.polluterId);

        purchaseCheck = check;

        healthBar.Hide();

        imageIsDisaster = false;

        return check;
    }

    public bool CheckGraphicPolluter(PolluterIcon polluterIcon)
    {
        PolluterAttrib attrib = polluterIcon.GetPolluterAttributes();

        bool check = CheckPolluter(attrib, polluterIcon.polluterId);

        iconCheck = check;

        healthBar.Hide();

        imageIsDisaster = false;

        return check;
    }

    public bool CheckPolluter(Polluter polluter)
    {
        if(polluter != null)
        {
            imageIsDisaster = !polluter.IsAlive();
            
            bool check = CheckPolluter(polluter.GetAttrib(), polluter.polluterId);

            // special case in game
            PollutionMap map = new PollutionMap(polluter.GetPollutionMap());

            if (Util.SumMap(map) == 0)
            {
                objectPieChart.SetActive(false);
            }
            else
            {
                objectPieChart.SetActive(true);
            }

            if (Util.SumMap(map) < 0)
            {
                map = Util.MultiplyMap(map, (-1));
            }

            if(polluter is Filter)
            {
                pieChartTitle.text = "Reducing:";
                maxPieText.text = "/" + Mathf.Abs(Util.SumMap(new PollutionMap(polluter.GetAttrib().pollutionAttrib.emissions)));
            }

            if(polluter is Factory)
            {
                pieChartTitle.text = "Emitting:";
                maxPieText.text = "";
            }

            if(polluter is Recycler)
            {
                pieChartTitle.text = "Transforming:";
                maxPieText.text = "";

                if(polluter.GetAttrib().recycleAttrib.conversions != null)
                {
                    foreach (RecycleAttrib.Conversion conversion in polluter.GetAttrib().recycleAttrib.conversions)
                    {
                        maxPieText.text += "/" + conversion.maxConversion.ToString() + "\n";
                    }
                }
            }
            
            if(polluter.GetProfit() == 0)
                textIncome.text = "";


            SetPieChart(pieChart, map);
            
            healthBar.Show(polluter.GetHealthComp());

            return check;
        }

        return false;
    }

    public bool CheckPolluter(PolluterAttrib polluterAttrib, int id = 0)
    {
        bool hasFoundData = false;

        if (polluterAttrib != null)
        {
            hasFoundData = true;
            
            textTitle.text = polluterAttrib.title;

            textCoins.text = "Price: " + polluterAttrib.economicAttrib.price;
            textIncome.text = " Income: " + polluterAttrib.economicAttrib.profitPerTurn;
            textRemoval.text = "Removal cost: " + polluterAttrib.economicAttrib.removalCost;

            PollutionMap map = new PollutionMap(polluterAttrib.pollutionAttrib.emissions);
            
            if(Util.SumMap(map) == 0)
            {
                objectPieChart.SetActive(false);
            } else
            {
                objectPieChart.SetActive(true);
            }

            if (Util.SumMap(map) < 0)
            {
                map = Util.MultiplyMap(map, (-1));
                pieChartTitle.text = "Reduces:";
            } else
            {
                pieChartTitle.text = "Emits:";
            }

            maxPieText.text = "";

            SetPieChart(pieChart, map);

            if (showVulnerabilities)
            {
                VulnerabilityAttrib vulnerabilityAttrib = polluterAttrib.vulnerabilityAttrib;
                if (vulnerabilityAttrib != null)
                {
                    if (vulnerabilityAttrib.vulnerabilities != null)
                    {
                        objectVulnerabilities.SetActive(true);

                        string vulnerabilityString = "Vulnerable to ";
                        foreach (VulnerabilityAttrib.Vulnerability v in vulnerabilityAttrib.vulnerabilities)
                        {
                            vulnerabilityString += v.disasterName + ":" + v.factor + " ";
                        }
                        textVulnerabilities.text = vulnerabilityString;
                    }
                    else
                    {
                        objectVulnerabilities.SetActive(false);

                        textVulnerabilities.text = "";
                    }
                }
            } else
            {
                objectVulnerabilities.SetActive(false);
            }

            ResourceAttrib resourceAttrib = polluterAttrib.resourceAttrib;
            if(resourceAttrib != null)
            {
                if(resourceAttrib.products != null)
                {
                    objectResources.SetActive(true);

                    textResources.text = "";

                    foreach (ResourceAttrib.Product product in resourceAttrib.products)
                    {
                        textResources.text += "Produces: " + product.productPerTurn + " of " + product.resourceName + " per turn\n";
                    }

                } else
                {
                    objectResources.SetActive(false);
                }
            }

            RecycleAttrib recycleAttrib = polluterAttrib.recycleAttrib;
            if (recycleAttrib != null)
            {
                if (recycleAttrib.conversions != null)
                {
                    pieChartTitle.text = "Transforms:";

                    objectRecyclers.SetActive(true);

                    textRecyclers.text = "";

                    PollutionMap mapRecycle = new PollutionMap();

                    foreach(RecycleAttrib.Conversion conversion in recycleAttrib.conversions)
                    {
                        mapRecycle.Add(conversion.pollutantName, conversion.maxConversion);

                        textRecyclers.text += "" + conversion.pollutantName + " to " + conversion.convertTo + "\n"
                            + "Rate: " + conversion.conversionRate + "\n"; //+ " Max: " + conversion.maxConversion + "\n";
                    }

                    if(Util.SumMap(mapRecycle) != 0)
                    {
                        objectPieChart.SetActive(true);
                        SetPieChart(pieChart, mapRecycle);
                    }
                }
                else
                {
                    objectRecyclers.SetActive(false);
                }
            }

            VisualAttrib visualAttrib = polluterAttrib.visualAttrib;
            if (visualAttrib.imageName != "")
            {
                worldWindow.imageLoader.LoadImage(visualAttrib.imageName);
                imageToShow = true;
            }
            else
            {
                imageToShow = false;
            }

            //icons
            factoryIcon.gameObject.SetActive(false);
            filterIcon.gameObject.SetActive(false);
            recyclerIcon.gameObject.SetActive(false);

            if(polluterAttrib is FactoryAttrib)
            {
                factoryIcon.SetPolluterAttributes(polluterAttrib);
                factoryIcon.SetText(id.ToString());
                factoryIcon.gameObject.SetActive(true);
            }

            if(polluterAttrib is FilterAttrib)
            {
                filterIcon.SetPolluterAttributes(polluterAttrib);
                filterIcon.SetText(id.ToString());
                filterIcon.gameObject.SetActive(true);
            }

            if(polluterAttrib is RecyclerAttrib)
            {
                recyclerIcon.SetPolluterAttributes(polluterAttrib);
                recyclerIcon.SetText(id.ToString());
                recyclerIcon.gameObject.SetActive(true);
            }

            //text
            sideTextController.SetTemporaryText(polluterAttrib.title);
        }

        return hasFoundData;
    }

    public override void ShowPopup()
    {
        if (currentSequence != null)
        {
            currentSequence.Restart();
            currentSequence.Kill();
            currentSequence = null;
        }

        base.ShowPopup();
    }

    public override void HidePopup(bool instant = false)
    {
        base.HidePopup(instant);

        //text
        sideTextController.ResetTemporaryText();
    }

    public void FeedbackCoins()
    {
        if(currentSequence != null)
        {
            currentSequence.Restart();
            currentSequence.Kill();
            currentSequence = null;
        }

        currentSequence = Feedback.Instance.ErrorText(textCoins, defaultColorText);

        currentSequence.Play();

    }

    public void FeedbackRemovalCoins()
    {
        if (currentSequence != null)
        {
            currentSequence.Restart();
            currentSequence.Kill();
            currentSequence = null;
        }

        currentSequence = Feedback.Instance.ErrorText(textRemoval, defaultColorText);

        currentSequence.Play();

    }
}
