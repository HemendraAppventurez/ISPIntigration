using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CCSHealthFamilyWelfareDept.WebServices
{
    /// <summary>
    /// Summary description for darpan2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class darpan2 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string push()
        {
            Darpan2.DarpanService ds_NUH = new Darpan2.DarpanService("6dO11hZVP0AktfjCKqIZYj3v2HYxq4Qiqk6G31mTLT4=", "yY+blv1wGszP5RTnevAbd7L0Jz3EtXeCYdqm8+D76qk=", 2159, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_NUH_2_0");

            ds_NUH.pushData();

            Darpan2.DarpanService ds_ILC = new Darpan2.DarpanService("bMC2WfHol/WeZfFAU/kmZ7alGIuBE3p602wQJYJcKVU=", "dAAxaRg0FAxU6HchnPCFokPeeasSSa90VQcWasFLy6U=", 2371, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_ILC_2_0");

            ds_ILC.pushData();

            Darpan2.DarpanService ds_FIC = new Darpan2.DarpanService("0lV6+6IjCAoZWKnCC1OsqbzqG1vrRHCgxySixxA6tkY=", "BCbIOjc+a09aBE+46PZ3hYncxs9mn0N4PHa5muv1nRI=", 2372, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_FIC_2_0");

            ds_FIC.pushData();

            Darpan2.DarpanService ds_DIC = new Darpan2.DarpanService("Ih/mL6RkYYiXLDipgL37Tg+mdvC1HHBhnmJBtuWbq+0=", "naeyR+7hi0GzCpixEr6ojDClEb6/Z8RQg2Wiycp7Mys=", 2373, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_DIC_2_0");

            ds_DIC.pushData();

            Darpan2.DarpanService ds_IMC = new Darpan2.DarpanService("iOfhfRUmdxd0N5jxFpHnshl+9HfNU2mkxHDD8hP6Tvk=", "qBLnwlLvwy2/QBw2BIKy5pCKkKVacDZ2XNHjHhknV7I=", 2374, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_IMC_2_0");

            ds_IMC.pushData();

            Darpan2.DarpanService ds_DEC = new Darpan2.DarpanService("WjzxcZQlhZ812zsx1c8tHXuMnyg4js1iljeVbs+GG00=", "we+SRgbprVwDlQDrdxkMyhwHeiN75+7kE0TU4P0EP2Q=", 2375, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_DEC_2_0");

            ds_DEC.pushData();

            Darpan2.DarpanService ds_FAP = new Darpan2.DarpanService("yhs6bqAv2hLhtYvtn6kby2OSdXh+WTPJ5tqRPlbP4R4=", "4DPz6W0bgImYSfwwBO6ZbRF5S/PuXSsxVLlPAx70rAw=", 2376, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_FAP_2_0");

            ds_FAP.pushData();

            Darpan2.DarpanService ds_MER = new Darpan2.DarpanService("BWdVFqchGOdc0p02QDQpkBh8hDBXAzPIVaRe3IHlK7M=", "QBoBUESfz0pZpIPRcf6Z5+3TbGnWgUFjTOOgFgB6mnM=", 2377, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_MER_2_0");

            ds_MER.pushData();

            Darpan2.DarpanService ds_MLC = new Darpan2.DarpanService("B5g15aHriiSX6QxXGrv0vUUFYvwxvOScz1A8IVixjh0=", "Dh8LVEoEPnhhKaHU7yNBb/CaeKiwvSW8vrI8NgjSKqQ=", 2378, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_MLC_2_0");

            ds_MLC.pushData();

            Darpan2.DarpanService ds_AGC = new Darpan2.DarpanService("FHdaQTDRgAlCLRqPfKa285VrOCwqlGy1REWf3aKD7cY=", "4lFVRkE15FDYISniGp0prVBpf0j37jdaNs1YNYuJ9Ik=", 2380, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_AGC_2_0");

            ds_AGC.pushData();

            Darpan2.DarpanService ds_ICC = new Darpan2.DarpanService("BmggbXjJ2K80P9+CkYhrauUiR1/ul8sSBlmD+yYwos4=", "9wRKFbUKd8YseSoXEYcVRtPe9b4CmOYEjEDYkv/x+N0=", 2382, 2, "proc_I_DMDashServiceLog_2_0", "proc_GetApplicationStatusByDateRange_ICC_2_0");

            ds_ICC.pushData();

            return "Hello World";
        }
    }
}
