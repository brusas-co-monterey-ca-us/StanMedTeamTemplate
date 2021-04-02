<%@ Page Title="Medical Team Tools" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MedNoteWeb1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MedTeamContent" runat="server">

      <div class="jumbotron">
        <%--<h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>--%>

           <p>
              <asp:DropDownList ID="StaffCaseload" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="CaseloadClientSelected"></asp:DropDownList>
              <asp:Label ID="ClientLabel1" runat="server" Font-Size="Medium" Font-Bold="true" Text="Label"> PATID Info </asp:Label>
              <asp:TextBox ID="PATID" ForeColor="blue" runat="server" Width="60px" AutoPostBack="True" OnTextChanged="PATIDChanged"></asp:TextBox>
              <%--<asp:Button ID="TestButton" runat="server" Text="Client Lookup" OnClick="ClickTestButton" />--%>
              <asp:TextBox ID="ClientName" ForeColor="blue" Width="200px" MaxWidth="200px" runat="server"></asp:TextBox>
              <asp:TextBox ID="EpNumber" ForeColor="blue" Width="30px" MaxWidth="30px" runat="server" ></asp:TextBox>
              <asp:TextBox ID="ProgramName" Width="300px" MaxWidth="300px" ForeColor="blue" runat="server" ></asp:TextBox>
              <asp:TextBox ID="ProgramCode" runat="server" ></asp:TextBox> 
          </p> 
         
              <div style="width: 1000px; max-width: 1000px"> <%--<div style="overflow: auto; height: 50px; width:750px;">--%>
                  <p>
                      <asp:Label ID="SafetyLabel" runat="server"  Text="Label">Safety Plan </asp:Label>
                      <asp:TextBox textmode="multiline" ID="SafetyPlan" runat="server" style="OVERFLOW:auto; width:1250px; height:50px; color:red"></asp:TextBox>
                  </p>
              </div> 

          <%--<p><a class="btn btn-primary bth-md" href="http://www.msn.com">Get Some news &raquo;</a></p>--%>  
    </div>
    <br />
    <label for="intakeDate">Intake Note Date</label>
    <input type="date" name="intakeDate" value="" />
    <br />
    
    <div class="row">
         <div class="col-md-8">
            <h4>Diagnosis Info </h4>
             <asp:Table ID="Diagnosis" runat="server"
                Font-Size="Medium" 
                Width="1000" 
                Font-Names="Calibri"
                BackColor="LightBlue"
                BorderColor="DarkGray"
                BorderWidth="2"
                ForeColor="Blue"
                CellPadding="10"
                CellSpacing="10">
                <asp:TableHeaderRow 
                    runat="server" 
                    ForeColor="Snow"
                    BackColor="CadetBlue"
                    Font-Bold="true">
                    <asp:TableHeaderCell>Dx Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>type</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Rank</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Dx By</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Diagnosis</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow BackColor="LightBlue" ForeColor="Black"></asp:TableRow>
            </asp:Table>
        </div>
        <br />
        <br />
        <div class="col-md-8">
            <h4>Current Medictions - Order Connect</h4>
                        <asp:Table ID="CurrMeds" runat="server"
                Font-Size="Medium" 
                Width="1000" 
                Font-Names="Calibri"
                BackColor="LightBlue"
                BorderColor="DarkGray"
                BorderWidth="2"
                ForeColor="Blue"
                CellPadding="10"
                CellSpacing="10">
                <asp:TableHeaderRow 
                    runat="server" 
                    ForeColor="Snow"
                    BackColor="CadetBlue"
                    Font-Bold="true"
                    >
                    <asp:TableHeaderCell>Drug Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Start Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>End Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Prescriber</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Dosage</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow BackColor="LightBlue" ForeColor="Black"></asp:TableRow>
            </asp:Table>
        </div>
        <br />
        <br />    
        <div class="col-md-8">
            <h4>Mediction History - Summary - Order Connect (Last 5 Years) </h4>
            <p>( Does not include current medications )</p>
            <asp:Table ID="HistMeds" runat="server"
                Font-Size="Medium" 
                Width="1000" 
                Font-Names="Calibri"
                BackColor="LightBlue"
                BorderColor="DarkGray"
                BorderWidth="2"
                ForeColor="Blue"
                CellPadding="10"
                CellSpacing="10">
                <asp:TableHeaderRow 
                    runat="server" 
                    ForeColor="Snow"
                    BackColor="CadetBlue"
                    Font-Bold="true"
                    >
                    <asp:TableHeaderCell>Drug Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Min Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Max Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Span Weeks</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Rx Count</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Tot Qty</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow BackColor="LightBlue" ForeColor="Black"></asp:TableRow>
            </asp:Table>
        </div>
        <br />


         <div class="col-md-6">
            <h4>Recent Vitals</h4>
<%--            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>--%>
            <asp:Table ID="Vitals" runat="server"
                Font-Size="Medium" 
                Width="550" 
                Font-Names="Calibri"
                BackColor="LightBlue"
                BorderColor="DarkGray"
                BorderWidth="2"
                ForeColor="Blue"
                CellPadding="5"
                CellSpacing="5">
                <asp:TableHeaderRow 
                    runat="server" 
                    ForeColor="Snow"
                    BackColor="CadetBlue"
                    Font-Bold="true"
                    >
                    <asp:TableHeaderCell>Vitals Collected On</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Pulse</asp:TableHeaderCell>
                    <asp:TableHeaderCell>BP</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Height</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell>BMI</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow BackColor="LightBlue" ForeColor="Black"></asp:TableRow>
            </asp:Table>
        </div>

        <div class="col-md-8">
            <h4>Recent Lab Results (Last 2 Years From Order Connect)</h4>
<%--            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>--%>
            <asp:Table ID="Labs" runat="server"
                Font-Size="Medium" 
                Width="1000" 
                Font-Names="Calibri"
                BackColor="LightBlue"
                BorderColor="DarkGray"
                BorderWidth="2"
                ForeColor="Blue"
                CellPadding="5"
                CellSpacing="5">
                <asp:TableHeaderRow 
                    runat="server" 
                    ForeColor="Snow"
                    BackColor="CadetBlue"
                    Font-Bold="true"
                    >
                    <asp:TableHeaderCell>Lab Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Received Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Order By</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Abnormal Count</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Review</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow BackColor="LightBlue" ForeColor="Black"></asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div>
        <br />
        <br />
        <h3>Clients Chief Complaint : </h3><br>
        <p>
            <asp:TextBox ID="ChiefComplaint" TextMode="MultiLine" style="OVERFLOW:auto; width:1250px; height:100px" selectoinstrart="0" runat="server"></asp:TextBox>
        </p>
    </div>
    <div>
        <br />
        <br />
        <h3>Symptoms Section: </h3>
        <h4>Psychosis - Current Symptoms</h4>

        <div>
            <input type="Checkbox" id="currNoneEvident" value="None Evident" onclick="changeCurrBoxs()" /> 
            <label for="currNoneEvident">None Evident</label>
            <input type="Checkbox" id="currDelusions"  value="Delusions" /> 
            <label for="currDelusions">Delusions</label>
            <input type="Checkbox" id="currHalucinations"  value="Halucinations" />
            <label for="currHalucinations">Halucinations</label>
            <input type="Checkbox" id="currParanoia"  value="Paranoia" />
            <label for="currParanoia">Paranoia</label>        

        </div>

        <h5>Describe Current Psychosis</h5>
        <textarea name="CurrentPsychosis" id="CurrentPsychosis" rows="5" cols="200"></textarea>

        <script type="text/javascript">
            function changeCurrBoxs() {

                var none = document.getElementById("currNoneEvident");
                var delu = document.getElementById("currDelusions");
                var halu = document.getElementById("currHalucinations"); 
                var pari = document.getElementById("currParanoia");

                if (none.checked == true) {
                    delu.disabled = true; delu.checked = false;
                    halu.disabled = true; halu.checked = false;
                    pari.disabled = true; pari.checked = false;
                }
                else {
                    delu.disabled = false;
                    halu.disabled = false;
                    pari.disabled = false;
                }
            }
        </script>

        <br />
        <br />
        <h4>Psychosis - Past Symptoms</h4>
        <div>
            <input type="Checkbox" id="pastNoneEvident" value="None Evident" onclick="changePastBoxs()" />
            <label for="pastNoneEvident">None Evident</label>
            <input type="Checkbox" id="pastDelusions"  value="Delusions" />
            <label for="pastDelusions">Delusions</label>
            <input type="Checkbox" id="pastHalucinations"  value="Halucinations" /> 
            <label for="pastHalucinations">Halucinations</label>
            <input type="Checkbox" id="pastParanoia"  value="Paranoia"/> 
            <label for="pastParanoia">Paranoia</label>
        </div>

        <h5>Describe Past Psychosis</h5>
        <textarea name="PastPsychosis" id="PastPsychosis" rows="5" cols="200"></textarea>

        <script type="text/javascript">
            function changePastBoxs() {

                var none = document.getElementById("pastNoneEvident");
                var delu = document.getElementById("pastDelusions");
                var halu = document.getElementById("pastHalucinations"); 
                var pari = document.getElementById("pastParanoia");

                if (none.checked == true) {
                    delu.disabled = true; delu.checked = false;
                    halu.disabled = true; halu.checked = false;
                    pari.disabled = true; pari.checked = false;
                }
                else {
                    delu.disabled = false;
                    halu.disabled = false;
                    pari.disabled = false;
                }
            }
        </script>
        <br />
        <br />
        <h4>Mood-Depressive - Current Symptoms</h4>

        <div>
            <input type="Checkbox" id="currMoodNoneEvident" value="1" onclick="changeCurrMoodBoxs()" /> 
            <label for="currMoodNoneEvident">None Evident</label>
            <input type="Checkbox" id="currDepress"  value="1" /> 
            <label for="currDepress">Depressed or Irritable Mood</label>
            <input type="Checkbox" id="currDecreasseInterst"  value="1" />
            <label for="currDecreasseInterst">Decreased Interest or Pleasure</label>
            <input type="Checkbox" id="currFeelWorthless"  value="1" />
            <label for="currFeelWorthless">Feeling Worthless or Guilty</label>        
        </div>
        
        <script type="text/javascript">
            function changeCurrMoodBoxs() {

                var none = document.getElementById("currMoodNoneEvident");
                var cb1 = document.getElementById("currDepress");
                var cb2 = document.getElementById("currDecreasseInterst"); 
                var cb3 = document.getElementById("currFeelWorthless");

                if (none.checked == true) {
                    cb1.disabled = true; cb1.checked = false;
                    cb2.disabled = true; cb2.checked = false;
                    cb3.disabled = true; cb3.checked = false;
                }
                else {
                    cb1.disabled = false;
                    cb2.disabled = false;
                    cb3.disabled = false;
                }
            }
        </script>
        <h4>Mood-Manic - Current Symptoms</h4>

        <div>
            <input type="Checkbox" id="currManicNoneEvident" value="1" onclick="changeCurrManicBoxs()" /> 
            <label for="currManicNoneEvident">None Evident</label>
            <input type="Checkbox" id="currPalpitaions"  value="1" /> 
            <label for="currPalpitaions">Palpitations, Pounding Heart</label>
            <input type="Checkbox" id="currShortnessOfBreath"  value="1" />
            <label for="currShortnessOfBreath">Shortness of Breath</label>
            <input type="Checkbox" id="currChestPain"  value="1" />
            <label for="currChestPain">Chest Pain or Discomfort</label>        

        </div>
 
        <h5>Describe Current Mood-Depressive Mood-Manic Symptoms</h5>
        <textarea name="CurrentMood" id="CurrentMood" rows="5" cols="200"></textarea>

        <script type="text/javascript">
            function changeCurrManicBoxs() {

                var none = document.getElementById("currManicNoneEvident");
                var cb1 = document.getElementById("currPalpitaions");
                var cb2 = document.getElementById("currShortnessOfBreath"); 
                var cb3 = document.getElementById("currChestPain");

                if (none.checked == true) {
                    cb1.disabled = true; cb1.checked = false;
                    cb2.disabled = true; cb2.checked = false;
                    cb3.disabled = true; cb3.checked = false;
                }
                else {
                    cb1.disabled = false;
                    cb2.disabled = false;
                    cb3.disabled = false;
                }
            }
        </script>

        <br />
        <br />
          <h4>Mood-Depressive - Past Symptoms</h4>

        <div>
            <input type="Checkbox" id="pastMoodNoneEvident" value="1" onclick="changePastMoodBoxs()" /> 
            <label for="pastMoodNoneEvident">None Evident</label>
            <input type="Checkbox" id="pastDepress"  value="1" /> 
            <label for="pastDepress">Depressed or Irritable Mood</label>
            <input type="Checkbox" id="pastDecreasseInterst"  value="1" />
            <label for="pastDecreasseInterst">Decreased Interest or Pleasure</label>
            <input type="Checkbox" id="pastFeelWorthless"  value="1" />
            <label for="pastFeelWorthless">Feeling Worthless or Guilty</label>        
        </div>
        
        <script type="text/javascript">
            function changePastMoodBoxs() {

                var none = document.getElementById("pastMoodNoneEvident");
                var cb1 = document.getElementById("pastDepress");
                var cb2 = document.getElementById("pastDecreasseInterst"); 
                var cb3 = document.getElementById("pastFeelWorthless");

                if (none.checked == true) {
                    cb1.disabled = true; cb1.checked = false;
                    cb2.disabled = true; cb2.checked = false;
                    cb3.disabled = true; cb3.checked = false;
                }
                else {
                    cb1.disabled = false;
                    cb2.disabled = false;
                    cb3.disabled = false;
                }
            }
        </script>
        <h4>Mood-Manic - Past Symptoms</h4>

        <div>
            <input type="Checkbox" id="pastManicNoneEvident" value="1" onclick="changePastManicBoxs()" /> 
            <label for="pastManicNoneEvident">None Evident</label>
            <input type="Checkbox" id="pastPalpitaions"  value="1" /> 
            <label for="pastPalpitaions">Palpitations, Pounding Heart</label>
            <input type="Checkbox" id="pastShortnessOfBreath"  value="1" />
            <label for="pastShortnessOfBreath">Shortness of Breath</label>
            <input type="Checkbox" id="pastChestPain"  value="1" />
            <label for="pastChestPain">Chest Pain or Discomfort</label>        

        </div>
 
        <h5>Describe Past Mood-Depressive Mood-Manic Symptoms</h5>
        <textarea name="PastMood" id="PastMood" rows="5" cols="200"></textarea>

        <script type="text/javascript">
            function changePastManicBoxs() {

                var none = document.getElementById("pastManicNoneEvident");
                var cb1 = document.getElementById("pastPalpitaions");
                var cb2 = document.getElementById("pastShortnessOfBreath"); 
                var cb3 = document.getElementById("pastChestPain");

                if (none.checked == true) {
                    cb1.disabled = true; cb1.checked = false;
                    cb2.disabled = true; cb2.checked = false;
                    cb3.disabled = true; cb3.checked = false;
                }
                else {
                    cb1.disabled = false;
                    cb2.disabled = false;
                    cb3.disabled = false;
                }
            }
        </script>
    </div>
    <div>
        <asp:TextBox ID="LabResults" textmode="MultiLine" runat="server"></asp:TextBox>
    </div>
</asp:Content>
