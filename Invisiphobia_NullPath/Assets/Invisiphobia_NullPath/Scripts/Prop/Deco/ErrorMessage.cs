using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour, IErrorMessageable
{
    [SerializeField] private int itemid;

    [SerializeField] private DoorErrorType doorErrorType;
    protected string errorMessageText;
    public string ErrorMessageText { get { return errorMessageText; } }
    private void Start()
    {
        ItemTable table = DataService.GetItemTableByKey(itemid);
        errorMessageText = DataService.GetItemText(table.errorMessage[(int)doorErrorType]);
    }
}
