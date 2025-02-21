using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class BoMis
{
    public string token { get; set; }
    public string l1 { get; set; }
    public string l2 { get; set; }
    public string projcode { get; set; }
    public string timestamp { get; set; }
    public string nonce { get; set; }
    public string opt1 { get; set; }
    public string opt2 { get; set; }
}

public class BoMispayload
{
    public string data { get; set; }
    public string hash { get; set; }
    public string projcode { get; set; }
    public string instcode { get; set; }
    public string iv { get; set; }
}

//public class BoMisResponse
//{
//    public string output { get; set; }
//    public string NewToken { get; set; }
//    public string outputHasg { get; set; }
//}

//public class BoMisDataSummary
//{
//    public string KPIIDString { get; set; }
//    public string KPIValueString { get; set; }
//    public string Data_lvl { get; set; }
 
//}
