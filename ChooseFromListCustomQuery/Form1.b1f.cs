using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;
using Microsoft.VisualBasic;

namespace ChooseFromListCustomQuery
{
    [FormAttribute("ChooseFromListCustomQuery.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            oApplication = (SAPbouiCOM.Application)Application.SBO_Application;
            this.oForm = this.oApplication.Forms.ActiveForm;
            this.oCompany = ((SAPbobsCOM.Company)(this.oApplication.Company.GetDICompany()));
            this.txtPONo = ((SAPbouiCOM.EditText)(this.GetItem("txtPONo").Specific));

            //this.txtPONo = (SAPbouiCOM.EditText)oForm.Items.Item("UID").Specific;
            this.txtPONo.ChooseFromListUID = "CFL_0";
            this.txtPONo.ChooseFromListAlias = "DocEntry";

            this.txtPONo.ChooseFromListBefore += 
                new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.txtPONo_ChooseFromListBefore);
                        
            // SAPbouiCOM.ChooseFromList oCFL = oForm.ChooseFromLists.Item("CFL_0");
            // string cflQuery = "SELECT * FROM OPOR WHERE OPOR.DocStatus = 'O' ";
            // CFLPopupWindow(oCFL, cflQuery, "DocEntry");
            this.OnCustomInitialize(); 
        }

        //public void CFLPopupWindow(SAPbouiCOM.ChooseFromList oCFL, string Query, string alias)
        //{
        //    try
        //    {
        //        oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //        oRecordset.DoQuery(Query);
        //        oCons = oCFL.GetConditions();
        //        oCons = null;
        //        oCFL.SetConditions(oCons);
        //        oCons = oCFL.GetConditions();
        //        if (oRecordset.RecordCount > 0)
        //        {
        //            do
        //            {
        //                oCon = oCons.Add();
        //                oCon.Alias = alias;
        //                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
        //                oCon.CondVal = oRecordset.Fields.Item(0).Value.ToString();
        //                oRecordset.MoveNext();

        //                if (!oRecordset.EoF)
        //                {
        //                    oCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
        //                }
        //            }
        //            while (!oRecordset.EoF);
        //        }
        //        oCFL.SetConditions(oCons);
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        public SAPbouiCOM.EditText txtPONo;
        public SAPbouiCOM.Application oApplication;
        public SAPbouiCOM.Form oForm;
        public SAPbobsCOM.Company oCompany;
        public SAPbobsCOM.Recordset oRecordset;
        public SAPbouiCOM.Conditions oConditions;
        public SAPbouiCOM.Conditions oEmptyConditions;
        public SAPbouiCOM.Condition oCondition;
        private SAPbouiCOM.ChooseFromList oCFL;

        private void OnCustomInitialize()
        {
        }

        private void txtPONo_ChooseFromListBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            oCFL = this.oForm.ChooseFromLists.Item("CFL_0");
            oEmptyConditions = new SAPbouiCOM.Conditions();
            oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oCFL.SetConditions(oEmptyConditions);
            oConditions = oCFL.GetConditions();

            oRecordset.DoQuery("SELECT OPOR.DocEntry FROM OPOR WHERE OPOR.DocStatus = 'O' ");
            oRecordset.MoveFirst();

            for (int i = 1; i <= oRecordset.RecordCount; i++)
            {
                if (i == (oRecordset.RecordCount))
                {
                    oCondition = oConditions.Add();
                    oCondition.Alias = "DocEntry";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = oRecordset.Fields.Item(0).Value.ToString();
                }
                else
                {
                    oCondition = oConditions.Add();
                    oCondition.Alias = "DocEntry";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = oRecordset.Fields.Item(0).Value.ToString();
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                }
                oRecordset.MoveNext();

                if (oRecordset.RecordCount == 0)
                {
                    oCondition = oConditions.Add();
                    oCondition.Alias = "DocEntry";
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_NONE;
                    oCondition.CondVal = "-1";
                }
            }
            oCFL.SetConditions(oConditions); 
        }
    }
}