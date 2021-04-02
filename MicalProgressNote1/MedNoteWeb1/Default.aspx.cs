using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedNoteWeb1
{
    public partial class _Default : Page
    {

        string dsnNum = "0";
        string appVersion = "PROD";
        //string USERID = "SHORTJP";
        string USERID = "VESGALOPEZOP";
        string returnText = "";

        

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientName.Attributes.Add("ReadOnly", "ReadOnly");
            ClientName.Attributes.Add("disabled", "disabled");
            SafetyPlan.Attributes.Add("ReadOnly", "ReadOnly");
            SafetyPlan.Attributes.Add("disabled", "disabled");
            PATID.Attributes.Add("ReadOnly", "ReadOnly");
            PATID.Attributes.Add("disabled", "disabled");
            EpNumber.Attributes.Add("ReadOnly", "ReadOnly");
            EpNumber.Attributes.Add("disabled", "disabled");
            ProgramName.Attributes.Add("ReadOnly", "ReadOnly");
            ProgramName.Attributes.Add("disabled", "disabled");
            ProgramCode.Attributes.Add("ReadOnly", "ReadOnly");
            ProgramCode.Attributes.Add("disabled", "disabled");


            if (!IsPostBack)
            {
                ChiefComplaint.Enabled = false;

                // Psycosis Check Boxes current and past // 


                ProgramCode.Style.Add("visibility", "hidden");

                FillCaseloadDropdown(USERID, appVersion, dsnNum);
            }

        }

        protected void CaseloadClientSelected(object sender, EventArgs e)
        {
            string safetyPlan = "";
            string[] parms = StaffCaseload.SelectedItem.Value.Split('&');

            if (true)
            {
                PATID.Text = parms[0];
                ClientName.Text = parms[1];
                EpNumber.Text = parms[2];
                ProgramCode.Text = parms[3];
                ProgramName.Text = parms[4];

                safetyPlan = GetSafetyPlan(PATID.Text, appVersion, dsnNum);
                AddVitalsRows(PATID.Text, appVersion, dsnNum);
                AddCurrMedsRows(PATID.Text, appVersion, dsnNum);
                AddMeicationHistRows(PATID.Text, appVersion, dsnNum); 
                AddDiagnosisRows(PATID.Text, EpNumber.Text, appVersion, dsnNum);
                AddLabRows(PATID.Text, appVersion, dsnNum);
                //GetLabDetails(appVersion, dsnNum); 

                SafetyPlan.Text = safetyPlan;

                ChiefComplaint.Enabled = true;

                // Psycosis Check Boxes current and past // 

            }
        }


        protected void PATIDChanged(object sender, EventArgs e)
        {
            ClientName.Text = "";
            SafetyPlan.Text = "";
        }

        protected void ClickTestButton(object sender, EventArgs e)
        {
            string clientName = "DEFAULT";
            string safetyPlan = "DEFAULT";
            List<string> vitals = new List<string>();

            clientName = GetClientName(PATID.Text,appVersion, dsnNum);
            safetyPlan = GetSafetyPlan(PATID.Text, appVersion, dsnNum);
            AddVitalsRows(PATID.Text, appVersion, dsnNum);
            AddCurrMedsRows(PATID.Text, appVersion, dsnNum); 


            ClientName.Text = clientName;
            SafetyPlan.Text = safetyPlan; 
        }

        private string GetClientName(string patid, string appVer, string dsnNum)
        {
            string returnName = "";

            string sqlString = "";
            string dSNConnect = "";

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "SELECT STRING($PIECE(D.patient_name,',',2),' ',$PIECE(D.patient_name,',',1)) Name FROM SYSTEM.patient_current_demographics D WHERE D.PATID = '" + patid + "'";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "SELECT STRING($PIECE(D.patient_name,',',2),' ',$PIECE(D.patient_name,',',1)) Name FROM SYSTEM.patient_current_demographics D WHERE D.PATID = '" + patid + "'";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    returnName = dreader["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                returnName = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
            return returnName;
        }

        private string GetSafetyPlan(string patid, string appVer, string dsnNum)
        {
            string returnSafetyPlan = "";

            string sqlString = "";
            string dSNConnect = "";

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = " SELECT  desc_spec_con SPlan FROM AVCWSSYSTEM.Special_Considerations " + 
                            " WHERE PATID = '" + patid + "' AND Consideration_Type = '1' AND Special_Consid = '1'";
                 // consideration type 1 = Safety Plan , special_consid = 1 means this one is active or active = YES // 
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = " SELECT  desc_spec_con SPlan FROM AVCWSSYSTEM.Special_Considerations " +
                            " WHERE PATID = '" + patid + "' AND Consideration_Type = '1' AND Special_Consid = '1'";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    returnSafetyPlan = dreader["SPlan"].ToString();
                }
            }
            catch (Exception ex)
            {
                returnSafetyPlan = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }

            if (returnSafetyPlan.Length == 0)
                returnSafetyPlan = "NO CURRENT PLAN"; 

            return returnSafetyPlan;
        }

        private void AddVitalsRows(string patid, string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";

            string vitDateTime = "";
            string pulse = "";
            string bP = "";
            string height = "";
            string weight = "";
            string bMI = "";

            int rowCount = 0; 

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "select top 5 cast(string(cast(admin_date_actual as varchar), ' ', cast(admin_time_actual_h as varchar))as datetime) as VitDateTime " +
                            " , max(case when reading = 'HR' then reading_value end) as Pulse " +
                            " , max(case when reading = 'BP' then reading_value end) as BP " +
                            " , max(case when reading = 'HtFtIn' then reading_value end) as Height " +
                            " , max(case when reading = 'WtLb' then reading_value end) as Weight" +
                            " , max(case when reading = 'BMI' then reading_value end) as  BMI " +
                            "  FROM AVCWSSYSTEM.cw_vital_signs " +
                            " WHERE patid = '" + patid +"'" + 
                            " and reading in ('HR', 'BP', 'HtFtIn', 'WtLb', 'BMI', 'RV') " +
                            " group by unique_row_id, data_entry_date " +
                            " order by cast(string(cast(admin_date_actual as varchar), ' ', cast(admin_time_actual_h as varchar))as datetime)  desc ";
           }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "select top 5 cast(string(cast(admin_date_actual as varchar), ' ', cast(admin_time_actual_h as varchar))as datetime) as VitDateTime " +
                            " , max(case when reading = 'HR' then reading_value end) as Pulse " +
                            " , max(case when reading = 'BP' then reading_value end) as BP " +
                            " , max(case when reading = 'HtFtIn' then reading_value end) as Height " +
                            " , max(case when reading = 'WtLb' then reading_value end) as Weight" +
                            " , max(case when reading = 'BMI' then reading_value end) as  BMI " +
                            "  FROM AVCWSSYSTEM.cw_vital_signs " +
                            " WHERE patid = '" + patid + "'" +
                            " and reading in ('HR', 'BP', 'HtFtIn', 'WtLb', 'BMI', 'RV') " +
                            " group by unique_row_id, data_entry_date " +
                            " order by cast(string(cast(admin_date_actual as varchar), ' ', cast(admin_time_actual_h as varchar))as datetime)  desc ";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    vitDateTime = dreader["VitDateTIme"].ToString();
                    pulse = dreader["Pulse"].ToString();
                    bP = dreader["BP"].ToString();
                    height = dreader["Height"].ToString();
                    weight = dreader["Weight"].ToString();
                    bMI = dreader["BMI"].ToString();

                    rowCount++;


                    TableRow vitalRow = new TableRow();

                    TableCell vitalsDate = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    vitalsDate.Text = vitDateTime;
                    vitalRow.Cells.Add(vitalsDate);

                    TableCell vitalsPulse = new TableCell();
                    //tASAMCell1.ApplyStyle(cellStyle2);
                    vitalsPulse.Text = pulse;
                    vitalRow.Cells.Add(vitalsPulse);

                    TableCell vitalsBP = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    vitalsBP.Text = bP;
                    vitalRow.Cells.Add(vitalsBP);

                    TableCell vitalHeight = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    vitalHeight.Text = height;
                    vitalRow.Cells.Add(vitalHeight);

                    TableCell vitalWeight = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    vitalWeight.Text = weight;
                    vitalRow.Cells.Add(vitalWeight);

                    TableCell vitalBMI = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    vitalBMI.Text = bMI;
                    vitalRow.Cells.Add(vitalBMI);


                    Vitals.Rows.Add(vitalRow);

                }

                if (rowCount == 0)
                {
                    TableRow vitalRow = new TableRow();

                    TableCell vitalsDate = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    vitalsDate.Text = "NO VITALS DATA";
                    vitalRow.Cells.Add(vitalsDate);

                    Vitals.Rows.Add(vitalRow);
                }
            }
            catch (Exception ex)
            {
                // returnSafetyPlan = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
        }

        private void AddCurrMedsRows(string patid, string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";

            string drugName = "";
            string start = "";
            string end = "";
            string prescriber = "";
            string qty = "";
            string dosage = "";

            int rowCount = 0; 

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "SELECT drugname \"Drug Name\", TO_CHAR(start_date,'MM/dd/yy') \"Start\", " + 
                    " TO_CHAR(end_date,'MM/dd/yy') \"End\" , prescriber Prescriber,dispense_qty Qty , dosage_combined Dosage" +
                            " FROM AVCWSInfoScrb.Rx Rx " +
                            " WHERE Rx.PATID = '" + patid + "'" +
                            "   AND((Rx.end_date IS  NULL) OR(Rx.end_date > GETDATE()))";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "SELECT drugname \"Drug Name\",start_date \"Start\", end_date \"End\" , prescriber Prescriber,dispense_qty Qty , dosage_combined Dosage" +
                            " FROM AVCWSInfoScrb.Rx Rx " +
                            " WHERE Rx.PATID = '" + patid + "'" +
                            "   AND((Rx.end_date IS  NULL) OR(Rx.end_date > GETDATE()))";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    drugName = dreader["Drug Name"].ToString();
                    start = dreader["Start"].ToString();
                    end = dreader["End"].ToString();
                    prescriber =  dreader["Prescriber"].ToString();
                    qty = dreader["Qty"].ToString();
                    dosage = dreader["Dosage"].ToString();

                    rowCount++;

                    TableRow currMedRow = new TableRow();

                    TableCell currDrugName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    currDrugName.Text = drugName;
                    currMedRow.Cells.Add(currDrugName);

                    TableCell currStart = new TableCell();
                    //tASAMCell1.ApplyStyle(cellStyle2);
                    currStart.Text= start;
                    currMedRow.Cells.Add(currStart);

                    TableCell currEnd = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currEnd.Text = end;
                    currMedRow.Cells.Add(currEnd);

                    TableCell currPrescriber = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currPrescriber.Text = prescriber;
                    currMedRow.Cells.Add(currPrescriber);

                    TableCell currQty = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currQty.Text = qty;
                    currMedRow.Cells.Add(currQty);

                    TableCell currDosage = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currDosage.Text = dosage;
                    currMedRow.Cells.Add(currDosage);


                    CurrMeds.Rows.Add(currMedRow);

                }

                if (rowCount == 0)
                {
                    TableRow currMedRow = new TableRow();

                    TableCell currDrugName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    currDrugName.Text = "NO CURRENT MEDS";
                    currMedRow.Cells.Add(currDrugName);

                    CurrMeds.Rows.Add(currMedRow);
                }
            }
            catch (Exception ex)
            {
                // returnSafetyPlan = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
        }

        private void AddMeicationHistRows(string patid, string appVer, string dsnNum)
        {
            string sqlString = "";
            string sqlString2 = "";
            string dSNConnect = "";
            string returnSQLInfo = "";

            string drugName = "";
            string start = "";
            string end = "";
            string span = "";
            string rxCount = "";
            string totQty = "";

            int rowCount = 0;

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "SELECT drugname \"Drug Name\", TO_CHAR(min(start_date),'MM/dd/yyyy') \"Start\",  " +
                            "  TO_CHAR(MAX(end_date),'MM/dd/yyyy') \"End\" ,DATEDIFF('wk', min(start_date), MAX(end_date)) span,  " +
                            " Count(drugname) RxCnt ,SUM(dispense_qty) Qty " +
                            "                        FROM AVCWSInfoScrb.Rx Rx " +
                            "                        WHERE Rx.PATID = '" + patid +  "'" +
                            "                         AND (Rx.end_date BETWEEN DATEADD('yy',-3,GETDATE()) AND GETDATE()) " +
                            "           GROUP BY drugname " +
                            "          ORDER BY UPPER(drugname)";

                sqlString2 = "SELECT prescriber, MIN(start_date), TO_CHAR(MIN(start_date),'MM/dd/yyyy') MinDt, TO_CHAR(Max(end_date),'MM/dd/yyyy') MaxDt, " +
                            "        DATEDIFF('wk', min(start_date),MAX(end_date)) Span, COUNT(drugname) RxCnt " + 
                            "                        FROM AVCWSInfoScrb.Rx Rx " +
                            "                        WHERE Rx.PATID = '" + patid + "' " +
                            "                         AND (Rx.end_date BETWEEN DATEADD('yy',-3,GETDATE()) AND GETDATE()) " +
                            "           GROUP BY prescriber " +
                            "          ORDER BY 2 DESC";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "SELECT drugname \"Drug Name\", TO_CHAR(min(start_date),'MM/dd/yyyy') \"Start\",  " +
                            "  TO_CHAR(MAX(end_date),'MM/dd/yyyy') \"End\" ,DATEDIFF('wk', min(start_date), MAX(end_date)) span,  " +
                            " Count(drugname) RxCnt ,SUM(dispense_qty) Qty " +
                            "                        FROM AVCWSInfoScrb.Rx Rx " +
                            "                        WHERE Rx.PATID = '" + patid + "'" +
                            "                         AND (Rx.end_date BETWEEN DATEADD('yy',-3,GETDATE()) AND GETDATE()) " +
                            "           GROUP BY drugname " +
                            "          ORDER BY UPPER(drugname)";

                sqlString2 = "SELECT prescriber, MIN(start_date), TO_CHAR(MIN(start_date),'MM/dd/yyyy') MinDt, TO_CHAR(Max(end_date),'MM/dd/yyyy') MaxDt, " +
                            "        DATEDIFF('wk', min(start_date),MAX(end_date)) Span, COUNT(drugname) RxCnt " +
                            "                        FROM AVCWSInfoScrb.Rx Rx " +
                            "                        WHERE Rx.PATID = '" + patid + "' " +
                            "                         AND (Rx.end_date BETWEEN DATEADD('yy',-3,GETDATE()) AND GETDATE()) " +
                            "           GROUP BY prescriber " +
                            "          ORDER BY 2 DESC";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    drugName = dreader["Drug Name"].ToString();
                    start = dreader["Start"].ToString();
                    end = dreader["End"].ToString();
                    span = dreader["Span"].ToString();
                    rxCount = dreader["RxCnt"].ToString();
                    totQty = dreader["Qty"].ToString();

                    rowCount++;

                    TableRow histMedRow = new TableRow();

                    TableCell histDrugName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    histDrugName.Text = drugName;
                    histMedRow.Cells.Add(histDrugName);

                    TableCell histStart = new TableCell();
                    //tASAMCell1.ApplyStyle(cellStyle2);
                    histStart.Text = start;
                    histMedRow.Cells.Add(histStart);

                    TableCell histEnd = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    histEnd.Text = end;
                    histMedRow.Cells.Add(histEnd);

                    TableCell histSpan = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    histSpan.Text = span;
                    histMedRow.Cells.Add(histSpan);

                    TableCell histRxCount = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    histRxCount.Text = rxCount;
                    histMedRow.Cells.Add(histRxCount);

                    TableCell histTotQty = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    histTotQty.Text = totQty;
                    histMedRow.Cells.Add(histTotQty);


                    HistMeds.Rows.Add(histMedRow);

                }

                if (rowCount == 0)
                {
                    TableRow currMedRow = new TableRow();

                    TableCell currDrugName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    currDrugName.Text = "NO MEDS History";
                    currMedRow.Cells.Add(currDrugName);

                    CurrMeds.Rows.Add(currMedRow);
                }
                else
                {
                    System.Data.Odbc.OdbcCommand cmd2 = new System.Data.Odbc.OdbcCommand(sqlString2, conn);
                    System.Data.Odbc.OdbcDataReader dreader2 = cmd2.ExecuteReader();

                    TableRow histMedRow2 = new TableRow();

                    TableCell histPresciberLable = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    histPresciberLable.Text = "*** PRESCRIBER LIST ***";
                    histMedRow2.Cells.Add(histPresciberLable);
 
                    HistMeds.Rows.Add(histMedRow2);

                    while (dreader2.Read())
                    {
                        drugName = dreader2["prescriber"].ToString();
                        start = dreader2["MinDt"].ToString();
                        end = dreader2["MaxDt"].ToString();
                        span = dreader2["Span"].ToString();
                        rxCount = dreader2["RxCnt"].ToString();

                        TableRow histMedRow3 = new TableRow();

                        TableCell histDrugName = new TableCell();
                        //vitalsDate.ApplyStyle(cellStyle2);
                        histDrugName.Text = drugName;
                        histMedRow3.Cells.Add(histDrugName);

                        TableCell histStart = new TableCell();
                        //tASAMCell1.ApplyStyle(cellStyle2);
                        histStart.Text = start;
                        histMedRow3.Cells.Add(histStart);

                        TableCell histEnd = new TableCell();
                        //vitalsBP.ApplyStyle(cellStyle2);
                        histEnd.Text = end;
                        histMedRow3.Cells.Add(histEnd);

                        TableCell histSpan = new TableCell();
                        //vitalsBP.ApplyStyle(cellStyle2);
                        histSpan.Text = span;
                        histMedRow3.Cells.Add(histSpan);

                        TableCell histRxCount = new TableCell();
                        //vitalsBP.ApplyStyle(cellStyle2);
                        histRxCount.Text = rxCount;
                        histMedRow3.Cells.Add(histRxCount);

                        HistMeds.Rows.Add(histMedRow3);

                    }

                }

            }
            catch (Exception ex)
            {
                 returnSQLInfo = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
        }

        private void AddLabRows(string patid, string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";

            string testName = "";
            string receivedOn = "";
            string orderBy = "";
            string abnormCnt = "";

            int rowCount = 0;

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "SELECT  STRING ( LabH.universal_svc_id_value, ' (' , LabH.universal_svc_id_code, ')') LabName " +
                            "    , LabH.ordering_provider_name prov, TO_CHAR(LabH.received_date, 'MM/dd/yyyy') RcdDt ,  " +
                            "    (SELECT COUNT(*) FROM AVCWSSYSTEM.results_detail RD " +
                            "        WHERE RD.PATID = LabH.PATID AND RD.JOIN_RESULT_HEADER_DETAIL = LabH.JOIN_RESULT_HEADER_DETAIL " +
                            "          AND UPPER(RD.obs_abnormal_flag_value ) <> 'UNKNOWN') Abnorm " +
                            "FROM AVCWSSYSTEM.results_header LabH " +
                            "WHERE LabH.PATID = '" + patid + "' AND LabH.received_date >= DATEADD('yy', -2, GETDATE()) " +
                            "ORDER BY LabH.received_date DESC";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "SELECT  STRING ( LabH.universal_svc_id_value, ' (' , LabH.universal_svc_id_code, ')') LabName " +
                            "    , LabH.ordering_provider_name prov, TO_CHAR(LabH.received_date, 'MM/dd/yyyy') RcdDt ,  " +
                            "    (SELECT COUNT(*) FROM AVCWSSYSTEM.results_detail RD " +
                            "        WHERE RD.PATID = LabH.PATID AND RD.JOIN_RESULT_HEADER_DETAIL = LabH.JOIN_RESULT_HEADER_DETAIL " +
                            "          AND UPPER(RD.obs_abnormal_flag_value ) <> 'UNKNOWN') Abnorm " +
                            "FROM AVCWSSYSTEM.results_header LabH " +
                            "WHERE LabH.PATID = '" + patid + "' AND LabH.received_date >= DATEADD('yy', -2, GETDATE()) " +
                            "ORDER BY LabH.received_date DESC";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    testName = dreader["LabName"].ToString();
                    receivedOn = dreader["RcdDt"].ToString();
                    orderBy = dreader["prov"].ToString();
                    abnormCnt = dreader["Abnorm"].ToString();

                    rowCount++;

                    TableRow currLabRow = new TableRow();

                    TableCell labName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    labName.Text = testName;
                    currLabRow.Cells.Add(labName);

                    TableCell labRcvd = new TableCell();
                    //tASAMCell1.ApplyStyle(cellStyle2);
                    labRcvd.Text = receivedOn;
                    currLabRow.Cells.Add(labRcvd);

                    TableCell provider = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    provider.Text = orderBy;
                    currLabRow.Cells.Add(provider);

                    TableCell abnorm = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    abnorm.Text = abnormCnt;
                    currLabRow.Cells.Add(abnorm);

                    Labs.Rows.Add(currLabRow);

                }

                if (rowCount == 0)
                {
                    TableRow currLabRow = new TableRow();

                    TableCell currDrugName = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    currDrugName.Text = "NO CURRENT LABS";
                    currLabRow.Cells.Add(currDrugName);

                    Labs.Rows.Add(currLabRow);
                }
            }
            catch (Exception ex)
            {
                // returnSafetyPlan = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
        }

        private void GetLabDetails( string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";
            string returnString = ""; 

            string labsData = "";
            string fullLabOutput = ""; 


            int rowCount = 0;

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSNCWS" + dsnNum;
                sqlString = "SELECT   LabH.PATID ,    LabH.JOIN_RESULT_HEADER_DETAIL ,    LabH.universal_svc_id_code , 'A' rowSort , ";
                sqlString = sqlString + "   STRING ( '<b><font color=\"blue\">(((  Lab Type: ', LabH.universal_svc_id_value , ' )))</b></font>'   ) AS LabsData ";
                sqlString = sqlString + " FROM SYSTEM.results_header LabH ";
                sqlString = sqlString + " WHERE LabH.PATID = '819829' AND LabH.JOIN_RESULT_HEADER_DETAIL = '14321' AND LabH.universal_svc_id_code = '331430' ";
                sqlString = sqlString + " HAVING LabH.received_date = MAX( LabH.received_date ) ";
                sqlString = sqlString + " UNION ";
                sqlString = sqlString + " SELECT  LabH.PATID   ,    LabH.JOIN_RESULT_HEADER_DETAIL  ,    LabH.universal_svc_id_code  , 'AA' rowSort   , STRING('>> Ordered By : ',LabH.ordering_provider_name,'<br>>> Data  Recieved On:<font color=\"blue\"> ', TO_CHAR(LabH.received_date,'MM/DD/YYYY') ,'</font>') LabsData ";
                sqlString = sqlString + " FROM SYSTEM.results_header LabH ";
                sqlString = sqlString + " WHERE LabH.PATID = '819829' AND LabH.JOIN_RESULT_HEADER_DETAIL = '14321' AND LabH.universal_svc_id_code = '331430' ";
                sqlString = sqlString + " HAVING LabH.received_date = MAX( LabH.received_date ) ";
                sqlString = sqlString + " UNION ";
                sqlString = sqlString + " SELECT  LabH.PATID ,    LabH.JOIN_RESULT_HEADER_DETAIL ,    LabH.universal_svc_id_code  , 'AA' rowSort  , STRING('-----------------------------------------------------------------' ) LabsData ";
                sqlString = sqlString + " FROM SYSTEM.results_header LabH ";
                sqlString = sqlString + " WHERE LabH.PATID = '819829' AND LabH.JOIN_RESULT_HEADER_DETAIL = '14321' AND LabH.universal_svc_id_code = '331430' ";
                sqlString = sqlString + " HAVING LabH.received_date = MAX( LabH.received_date ) ";
                sqlString = sqlString + " UNION ";
                sqlString = sqlString + " SELECT ";
                sqlString = sqlString + " LabH.PATID  ,    LabH.JOIN_RESULT_HEADER_DETAIL  ,    LabH.universal_svc_id_code  , 'C' rowSort , ";
                sqlString = sqlString + " STRING( LabR.observation_id_value,' ---> ', IFNULL( LabR.observation_value, '*',LabR.observation_value) ";
                sqlString = sqlString + " ,IFNULL( LabR.observation_value_unit, '', LabR.observation_value_unit),' ',CASE WHEN UPPER( LabR.obs_abnormal_flag_value ) <> 'UNKNOWN' ";
                sqlString = sqlString + "       THEN '<b><font color=\"red\">****'  ELSE ''  END , ' ',  ";
                sqlString = sqlString + "    CASE WHEN UPPER(LabR.obs_abnormal_flag_value ) = 'UNKNOWN' ";
                sqlString = sqlString + "      THEN '' ELSE  ";
                sqlString = sqlString + "                     CASE WHEN  LabR.obs_abnormal_flag_code  =  'A'  THEN    'ABNORMAL </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  '>'  THEN   'ABOVE Abs High </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  'H'  THEN   'ABOVE High Normal </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  'HH'  THEN 'ABOVE Upr Panic Lim </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  '<'  THEN   'BELOW Abs Lowl </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  'L'  THEN    'BELOW Low Normal </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  'LL'  THEN   'BELOW  Lwr Panic Lim </b></font>' ";
                sqlString = sqlString + "                          WHEN  LabR.obs_abnormal_flag_code  =  'N'  THEN    'NORMAL </b></font>' ";
                sqlString = sqlString + "                    END  ";
                sqlString = sqlString + " END  ) LabsData ";
                sqlString = sqlString + " FROM SYSTEM.results_header LabH ";
                sqlString = sqlString + "      INNER JOIN SYSTEM.results_detail LabR ";
                sqlString = sqlString + "         ON ( LabH.JOIN_RESULT_HEADER_DETAIL = LabR.JOIN_RESULT_HEADER_DETAIL ) ";
                sqlString = sqlString + " WHERE LabH.PATID = '819829' AND LabH.JOIN_RESULT_HEADER_DETAIL = '14321' AND LabH.universal_svc_id_code = '331430' ";
                sqlString = sqlString + " HAVING LabH.received_date = MAX( LabH.received_date ) ";
                sqlString = sqlString + " ORDER BY 1,2,3,4 ";

            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSNCWS" + dsnNum;

            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    labsData = dreader["LabsData"].ToString();

                    rowCount++;

                    fullLabOutput = fullLabOutput + labsData + "/n"; 
                }

                LabResults.Text = fullLabOutput;
            }
            catch (Exception ex)
            {
                 returnString = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }
        }

        private string FillCaseloadDropdown(string userid, string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";

            string value = "";
            string listText = "";

            string returnError = "n/a";

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                // Replaced by Stan on 2/2/2021 to include currently open episode information // 
                //sqlString = "SELECT STRING('(',PATID,') - ',$PIECE(Name,',',2),' ',$PIECE(Name,',',1)) Text, PATID Value " +
                //            " FROM( " +
                //            " SELECT DISTINCT C.PATID, " +
                //            " (SELECT patient_name FROM SYSTEM.patient_current_demographics D WHERE D.PATID = C.PATID) Name " +
                //            " FROM SYSTEM.RADplus_caseload C " +
                //            " WHERE C.USERID = '" + userid + "') " +
                //            " ORDER BY $PIECE(Name, ',', 1), $PIECE(Name, ',', 2) ";
                sqlString = " SELECT STRING('(',C.PATID,') - ',$PIECE(C.Name,',',2),' ',$PIECE(C.Name,',',1),' ----> EP:', " +
                            " EC.EPISODE_NUMBER, ' / ', EC.program_value) Text, " +
                            " STRING(C.PATID, '&',$PIECE(C.Name, ',', 2), ' ',$PIECE(C.Name, ',', 1), '&'," +
                            " C.EPISODE_NUMBER, '&', EC.program_code, '&', program_value) Value " +
                            " FROM( " +
                            " SELECT DISTINCT C.PATID, C.EPISODE_NUMBER, " +
                            "   (SELECT patient_name FROM SYSTEM.patient_current_demographics D WHERE D.PATID = C.PATID) Name " +
                            " FROM SYSTEM.RADplus_caseload C " +
                            " WHERE C.USERID = '" + userid + "') C " +
                            " INNER JOIN SYSTEM.view_episode_summary_current EC " +
                            " ON(EC.PATID = C.PATID and EC.EPISODE_NUMBER = C.EPISODE_NUMBER) " +
                            " ORDER BY $PIECE(Name, ',', 1), $PIECE(Name, ',', 2), C.EPISODE_NUMBER";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = " SELECT STRING('(',C.PATID,') - ',$PIECE(C.Name,',',2),' ',$PIECE(C.Name,',',1),' ----> EP:', " +
                            " EC.EPISODE_NUMBER, ' / ', EC.program_value) Text, " +
                            " STRING(C.PATID, '&',$PIECE(C.Name, ',', 2), ' ',$PIECE(C.Name, ',', 1), '&'," +
                            " C.EPISODE_NUMBER, '&', EC.program_code, '&', program_value) Value " +
                            " FROM( " +
                            " SELECT DISTINCT C.PATID, C.EPISODE_NUMBER, " +
                            "   (SELECT patient_name FROM SYSTEM.patient_current_demographics D WHERE D.PATID = C.PATID) Name " +
                            " FROM SYSTEM.RADplus_caseload C " +
                            " WHERE C.USERID = '" + userid + "') C " +
                            " INNER JOIN SYSTEM.view_episode_summary_current EC " +
                            " ON(EC.PATID = C.PATID and EC.EPISODE_NUMBER = C.EPISODE_NUMBER) " +
                            " ORDER BY $PIECE(Name, ',', 1), $PIECE(Name, ',', 2), C.EPISODE_NUMBER";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    value = dreader["Value"].ToString();
                    listText = dreader["Text"].ToString();

                    StaffCaseload.Items.Add(new ListItem(listText, value));

                }
            }
            catch (Exception ex)
            {
                 returnError = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }

            return returnError; 
        }

        private string AddDiagnosisRows(string patid, string episode,  string appVer, string dsnNum)
        {
            string sqlString = "";
            string dSNConnect = "";

            string dxDate = "";
            string dxType = "";
            string dxRank = "";
            string dxPrac = "";
            string dxInfo = "";

            int rowCount = 0;

            string returnError = "n/a";

            if (appVer.Equals("PROD"))
            {
                dSNConnect = "DSN=LIVESystem32DSN" + dsnNum;
                sqlString = "SELECT TO_CHAR(CDR.date_of_diagnosis,'MM/dd/yyyy') DxDt, CDR.diagnosis_type_value DxType, " + 
                            "       STRING(CDE.billing_order, CASE WHEN CDE.ranking_code IS NULL THEN '' ELSE ' - ' END, " +
                            "                                   CASE WHEN CDE.ranking_code IS NULL THEN '' ELSE CDE.ranking_value END) DxRank  ,  " +
                            "       CDE.diagnosing_clinician_value DxPrac, STRING('(', CDC.diagnosis_code, ') ', CDC.diagnosis_value) DxInfo " +
                            "  FROM SYSTEM.client_diagnosis_record CDR " +
                            "  INNER JOIN SYSTEM.client_diagnosis_entry CDE " +
                            "    ON(CDE.PATID = CDR.PATID AND CDE.DiagnosisRecord = CDR.ID) " +
                            "    INNER JOIN SYSTEM.client_diagnosis_codes CDC " +
                            "    ON(CDC.PATID = CDE.PATID AND CDC.DiagnosisEntry = CDE.ID AND CDC.code_set_code = 'DSM5') " +
                            " WHERE CDR.PATID = '" + patid + "' AND CDR.EPISODE_NUMBER = " + episode + " AND CDR.user_row_access_code = 1 " +
                            " ORDER BY 1 DESC, CDE.billing_order";
            }
            else
            {
                dSNConnect = "DSN=UATSystem32DSN" + dsnNum;
                sqlString = "SELECT TO_CHAR(CDR.date_of_diagnosis,'MM/dd/yyyy') DxDt, CDR.diagnosis_type_value DxType, " +
                            "       STRING(CDE.billing_order, CASE WHEN CDE.ranking_code IS NULL THEN '' ELSE ' - ' END, " +
                            "                                   CASE WHEN CDE.ranking_code IS NULL THEN '' ELSE CDE.ranking_value END) DxRank  ,  " +
                            "       CDE.diagnosing_clinician_value DxPrac, STRING('(', CDC.diagnosis_code, ') ', CDC.diagnosis_value) DxInfo " +
                            "  FROM SYSTEM.client_diagnosis_record CDR " +
                            "  INNER JOIN SYSTEM.client_diagnosis_entry CDE " +
                            "    ON(CDE.PATID = CDR.PATID AND CDE.DiagnosisRecord = CDR.ID) " +
                            "    INNER JOIN SYSTEM.client_diagnosis_codes CDC " +
                            "    ON(CDC.PATID = CDE.PATID AND CDC.DiagnosisEntry = CDE.ID AND CDC.code_set_code = 'DSM5') " +
                            " WHERE CDR.PATID = '" + patid + "' AND CDR.EPISODE_NUMBER = " + episode + " AND CDR.user_row_access_code = 1 " +
                            " ORDER BY 1 DESC, CDE.billing_order";
            }

            System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(dSNConnect);

            try
            {
                conn.Open();
                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(sqlString, conn);
                System.Data.Odbc.OdbcDataReader dreader = cmd.ExecuteReader();

                while (dreader.Read())
                {
                    dxDate = dreader["DxDt"].ToString();
                    dxType = dreader["DxType"].ToString();
                    dxRank = dreader["DxRank"].ToString();
                    dxPrac = dreader["DxPrac"].ToString();
                    dxInfo = dreader["DxInfo"].ToString();

                    rowCount++;

                    TableRow diagnosisRow = new TableRow();

                    TableCell currStartDate = new TableCell();
                    //tASAMCell1.ApplyStyle(cellStyle2);
                    currStartDate.Text = dxDate;
                    diagnosisRow.Cells.Add(currStartDate);

                    TableCell currType = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currType.Text = dxType;
                    diagnosisRow.Cells.Add(currType);

                    TableCell currRank = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currRank.Text = dxRank;
                    diagnosisRow.Cells.Add(currRank);

                    TableCell currPractioner = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currPractioner.Text = dxPrac;
                    diagnosisRow.Cells.Add(currPractioner);

                    TableCell currDxInfo = new TableCell();
                    //vitalsBP.ApplyStyle(cellStyle2);
                    currDxInfo.Text = dxInfo;
                    diagnosisRow.Cells.Add(currDxInfo);


                    Diagnosis.Rows.Add(diagnosisRow);

                }
                if (rowCount == 0)
                {
                    TableRow diagnosisRow = new TableRow();

                    TableCell currStartDate = new TableCell();
                    //vitalsDate.ApplyStyle(cellStyle2);
                    currStartDate.Text = "NO Dianosis Record";
                    diagnosisRow.Cells.Add(currStartDate);

                    Diagnosis.Rows.Add(diagnosisRow);
                }


            }
            catch (Exception ex)
            {
                returnError = ex.Message; // Console.WriteLine("An error occurred.");
            }
            finally
            {
                conn.Close();
            }

            return returnError;
        }
    }
}